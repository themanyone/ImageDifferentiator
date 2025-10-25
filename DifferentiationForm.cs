using System.Collections.Concurrent;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageDifferentiator
{
    public class ProcessorConfig
    {
        public string StreamUrl { get; set; } = "http://192.168.1.97:81/stream";
        public int CheckIntervalMs { get; set; } = 200;
        public bool NormalizeSource { get; set; } = false;
        public int DifferenceThreshold { get; set; } = 25;
        public decimal PercentDifferenceThreshold { get; set; } = 0.003m;
        public bool SaveOnDetect { get; set; } = true;
        public string OutputDirectory { get; set; } = "detected";
    }

    public class ImageProcessor
    {
        private readonly ProcessorConfig _config;
        private readonly MjpegDecoder _mjpeg;
        private readonly ConcurrentQueue<Image<Rgba32>> _imageQueue = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private Image<Rgba32>? _previousImage;
        private DateTime _lastFrameTime = DateTime.MinValue;
        private int _detections = 0;
        private int _restarts = 0;
        private int _discards = 0;

        public ImageProcessor(ProcessorConfig config)
        {
            _config = config;
            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += OnFrameReady;
            _mjpeg.Error += OnError;
        }

        public async Task RunAsync()
        {
            Console.WriteLine($"Starting stream from: {_config.StreamUrl}");
            Console.WriteLine($"Saving detected images to: {Path.GetFullPath(_config.OutputDirectory)}");

            if (_config.SaveOnDetect && !Directory.Exists(_config.OutputDirectory))
            {
                Directory.CreateDirectory(_config.OutputDirectory);
                Directory.CreateDirectory(Path.Combine(_config.OutputDirectory, "diff"));
            }

            StartStream();

            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Stopping...");
                _mjpeg.StopStream();
                _cancellationTokenSource.Cancel();
                e.Cancel = true;
            };

            await ProcessImageQueueAsync(_cancellationTokenSource.Token);
        }

        private void StartStream()
        {
            Console.WriteLine("Connecting to stream...");
            _mjpeg.ParseStream(new Uri(_config.StreamUrl));
            _lastFrameTime = DateTime.UtcNow;
        }

        private void OnFrameReady(object? sender, FrameReadyEventArgs e)
        {
            _lastFrameTime = DateTime.UtcNow;
            if (_imageQueue.Count < 100) // Prevent queue from growing indefinitely
            {
                _imageQueue.Enqueue(Image.Load<Rgba32>(e.FrameBuffer));
            }
            else
            {
                _discards++;
            }
        }

        private void OnError(object? sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Stream Error: {e.Message}. Attempting to restart.");
            RestartStream();
        }

        private void RestartStream()
        {
            _mjpeg.StopStream();
            Thread.Sleep(1000); // Wait a bit before reconnecting
            _restarts++;
            Console.WriteLine($"Restarting stream... (Attempt #{_restarts})");
            StartStream();
        }

        private async Task ProcessImageQueueAsync(CancellationToken token)
        {
            var lastCheck = DateTime.UtcNow;

            while (!token.IsCancellationRequested)
            {
                // Check for stalled stream
                if (DateTime.UtcNow.Subtract(_lastFrameTime).TotalSeconds > 5 && _imageQueue.IsEmpty)
                {
                    Console.WriteLine("Stream seems to have stalled.");
                    RestartStream();
                }

                if (_imageQueue.TryDequeue(out var currentImage))
                {
                    if (DateTime.UtcNow > lastCheck.AddMilliseconds(_config.CheckIntervalMs))
                    {
                        if (_config.NormalizeSource)
                        {
                            currentImage = Normalize(currentImage);
                        }

                        if (_previousImage != null)
                        {
                            CalculateDifference(_previousImage, currentImage);
                        }

                        _previousImage?.Dispose();
                        _previousImage = currentImage;
                        lastCheck = DateTime.UtcNow;
                    }
                    else
                    {
                        currentImage.Dispose(); // Discard frame if not checking
                    }
                }
                else
                {
                    await Task.Delay(10, token); // Wait for a new frame
                }
            }
        }

        private void CalculateDifference(Image<Rgba32> image1, Image<Rgba32> image2)
        {
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                Console.WriteLine("Image dimensions do not match. Skipping difference calculation.");
                return;
            }
 
            using var diff = new Image<Rgba32>(image1.Width, image1.Height);
            long overThresh = 0;

            // Use ImageSharp's fast pixel access
            image1.ProcessPixelRows(image2, diff, (accessor1, accessor2, diffAccessor) =>
            {
                for (int y = 0; y < accessor1.Height; y++)
                {
                    var pixelRow1 = accessor1.GetRowSpan(y);
                    var pixelRow2 = accessor2.GetRowSpan(y);
                    var diffPixelRow = diffAccessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRow1.Length; x++)
                    {
                        ref var p1 = ref pixelRow1[x];
                        ref var p2 = ref pixelRow2[x];

                        var diffR = (byte)Math.Abs(p1.R - p2.R);
                        var diffG = (byte)Math.Abs(p1.G - p2.G);
                        var diffB = (byte)Math.Abs(p1.B - p2.B);

                        diffPixelRow[x] = new Rgba32(diffR, diffG, diffB);

                        if (diffR > _config.DifferenceThreshold) overThresh++;
                        if (diffG > _config.DifferenceThreshold) overThresh++;
                        if (diffB > _config.DifferenceThreshold) overThresh++;
                    }
                }
            });

            decimal totalPixels = diff.Height * diff.Width * 3;
            decimal percentMotion = totalPixels > 0 ? (decimal)overThresh / totalPixels : 0;
 
            Console.Write($"\rQueue: {_imageQueue.Count}, Detections: {_detections}, Restarts: {_restarts}, Discards: {_discards}, Motion: {percentMotion:P4}      ");
 
            if (percentMotion > _config.PercentDifferenceThreshold)
            {
                _detections++;
                Console.WriteLine($"\nMotion detected! (Total: {_detections})");
 
                if (_config.SaveOnDetect)
                {
                    // Pass the new, faster-generated diff bitmap
                    SaveImagesAsync(image2, diff);
                }
            }
            else
            {
                // We created the diff bitmap, so we must dispose of it if not used.
                diff.Dispose();
            }
        }

        private async void SaveImagesAsync(Image<Rgba32> original, Image<Rgba32> difference)
        {
            var now = DateTime.Now;
            var fileNameRoot = $"{now:yyyy-MM-dd-HH-mm-ss-fff}";
            var originalFileName = Path.Combine(_config.OutputDirectory, fileNameRoot + ".jpg");
            var diffFileName = Path.Combine(_config.OutputDirectory, "diff", fileNameRoot + ".jpg");
 
            try
            {
                await original.SaveAsJpegAsync(originalFileName);
                await difference.SaveAsJpegAsync(diffFileName);
                Console.WriteLine($"Saved {originalFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError saving file: {ex.Message}");
            }
            finally
            {
                // The difference bitmap is created in CalculateDifference and needs to be disposed of.
                difference.Dispose();
            }
        }

        private static Image<Rgba32> Normalize(Image<Rgba32> image)
        {
            (byte max, byte min) = GetMaxMin(image);

            double factor = (max - min) == 0 ? 1.0 : 255.0 / (max - min);
            if (Math.Abs(factor - 1.0) < 0.01) return image;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    foreach (ref var pixel in pixelRow)
                    {
                        pixel.R = (byte)Math.Clamp((pixel.R - min) * factor, 0, 255);
                        pixel.G = (byte)Math.Clamp((pixel.G - min) * factor, 0, 255);
                        pixel.B = (byte)Math.Clamp((pixel.B - min) * factor, 0, 255);
                    }
                }
            });

            return image;
        }

        private static (byte max, byte min) GetMaxMin(Image<Rgba32> image)
        {
            byte max = 0;
            byte min = 255;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    foreach (var pixel in accessor.GetRowSpan(y))
                    {
                        max = Math.Max(max, pixel.R);
                        max = Math.Max(max, pixel.G);
                        max = Math.Max(max, pixel.B);
                        min = Math.Min(min, pixel.R);
                        min = Math.Min(min, pixel.G);
                        min = Math.Min(min, pixel.B);
                    }
                }
            });

            return (max, min);
        }
    }
}
