using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualization.ConsoleApp;

class Program
{
    static void Main()
    {
        var dir = Directory.CreateDirectory($"../../../../TagsCloudVisualization/out");
        var cloudVisualizer = new TagCloudVisualizer(dir.FullName);
        var cloudGenerator = new GeneratorTagCloud(cloudVisualizer);

        Console.WriteLine("Generating tag cloud visualizations...");
        
        var configs = new[]
        {
            new TagCloudGenerationConfig(
                center: new Point(400, 300),
                rectangleCount: 30,
                minSize: new Size(20, 10),
                maxSize: new Size(50, 30),
                outputFileName: "cloud_small.png",
                imageSize: new Size(800, 600)
            ),
            new TagCloudGenerationConfig(
                center: new Point(500, 400),
                rectangleCount: 100,
                minSize: new Size(30, 15),
                maxSize: new Size(80, 40),
                outputFileName: "cloud_medium.png",
                imageSize: new Size(1000, 800)
            ),
            new TagCloudGenerationConfig(
                center: new Point(600, 450),
                rectangleCount: 150,
                minSize: new Size(25, 12),
                maxSize: new Size(120, 60),
                outputFileName: "cloud_large.png",
                imageSize: new Size(1200, 900)
            )
        };
        
        var results = cloudGenerator.GenerateMultiple(configs);
        
        foreach (var result in results)
        {
            Console.WriteLine(result.StartsWith("ERROR") ? $"  - {result}" : $"- Created cloud at {result}");
        }
    }
}