using System.Drawing;

namespace ImageDifferentiator
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            // Default configuration values that were previously set in the UI.
            var config = new ProcessorConfig
            {
                StreamUrl = "http://192.168.1.97:81/stream",
                CheckIntervalMs = 200,
                NormalizeSource = false,
                DifferenceThreshold = 25,
                PercentDifferenceThreshold = 0.003m,
                SaveOnDetect = true,
                OutputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "detected")
            };

            var processor = new ImageProcessor(config);
            await processor.RunAsync();
        }
    }
}