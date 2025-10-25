﻿﻿﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageDifferentiator
{
    public class MjpegDecoder
    {
        // magic 2 byte header for JPEG images
        private readonly byte[] _jpegHeader = { 0xff, 0xd8 };

        // pull down 1024 bytes at a time
        private const int ChunkSize = 4096;

        // It's best practice to reuse a single HttpClient instance.
        private static readonly HttpClient _client = new HttpClient();

        private CancellationTokenSource? _cancellationTokenSource;

        public event EventHandler<FrameReadyEventArgs>? FrameReady;
        public event EventHandler<ErrorEventArgs>? Error;

        public MjpegDecoder()
        {
        }

        public void ParseStream(Uri uri)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => CaptureStream(uri, _cancellationTokenSource.Token));
        }

        public void StopStream()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Task CaptureStream(Uri uri, CancellationToken token)
        {
            var imageBuffer = new MemoryStream();

            try
            {
                using var response = await _client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, token);
                response.EnsureSuccessStatusCode();

                // find our magic boundary value
                var contentType = response.Content.Headers.ContentType?.ToString();
                if (string.IsNullOrEmpty(contentType) || !contentType.Contains("boundary="))
                {
                    throw new Exception("Invalid content-type header.  The camera is likely not returning a proper MJPEG stream.");
                }

                var boundary = contentType.Split("boundary=")[1].Replace("\"", "");
                var boundaryBytes = Encoding.UTF8.GetBytes("--" + boundary);

                using var stream = await response.Content.ReadAsStreamAsync(token);
                var reader = new BinaryReader(stream);

                while (!token.IsCancellationRequested)
                {
                    var buff = reader.ReadBytes(ChunkSize);
                    if (buff.Length == 0)
                    {
                        await Task.Delay(100, token); // Stream paused, wait a bit
                        continue;
                    }

                    // find the JPEG header
                    int imageStart = buff.Find(_jpegHeader);

                    if (imageStart != -1)
                    {
                        // copy the start of the JPEG image to the imageBuffer
                        imageBuffer.Write(buff, imageStart, buff.Length - imageStart);

                        while (true)
                        {
                            buff = reader.ReadBytes(ChunkSize);

                            // find the boundary text
                            int imageEnd = buff.Find(boundaryBytes);
                            if (imageEnd != -1)
                            {
                                // copy the remainder of the JPEG to the imageBuffer
                                imageBuffer.Write(buff, 0, imageEnd);

                                // We have a complete frame.
                                ProcessFrame(imageBuffer.ToArray());

                                // Reset for the next frame, carrying over any leftover data.
                                imageBuffer.SetLength(0);
                                imageBuffer.Write(buff, imageEnd, buff.Length - imageEnd);
                                break;
                            }

                            // copy all of the data to the imageBuffer
                            imageBuffer.Write(buff, 0, buff.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, new ErrorEventArgs(ex.Message));
            }
        }

        private void ProcessFrame(byte[] frame)
        {
            FrameReady?.Invoke(this, new FrameReadyEventArgs(frame));
        }
    }

    internal static class Extensions
    {
        public static int Find(this byte[] buff, byte[] search)
        {
            // enumerate the buffer but don't overstep the bounds
            for (int start = 0; start < buff.Length - search.Length; start++)
            {
                // we found the first character
                if (buff[start] == search[0])
                {
                    int next;

                    // traverse the rest of the bytes
                    for (next = 1; next < search.Length; next++)
                    {
                        // if we don't match, bail
                        if (buff[start + next] != search[next])
                            break;
                    }

                    if (next == search.Length)
                        return start;
                }
            }
            // not found
            return -1;
        }
    }

    public class FrameReadyEventArgs : EventArgs
    {
        public byte[] FrameBuffer { get; }

        public FrameReadyEventArgs(byte[] buffer)
        {
            FrameBuffer = buffer;
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; }

        public ErrorEventArgs(string message)
        {
            Message = message;
        }
    }
}
