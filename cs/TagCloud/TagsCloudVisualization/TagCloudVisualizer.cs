using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizer
{
    private readonly string _outputDirectory;
    private readonly Func<string, Size, Color?, Color?, Color?, ImageGenerator> _imageGeneratorFactory;

    public TagCloudVisualizer(string outputDirectory)
    {
        _outputDirectory = outputDirectory;
        _imageGeneratorFactory =
            (path, size, bg, rect, center) => new ImageGenerator(path, size, bg, rect, center);
        Directory.CreateDirectory(_outputDirectory);
    }

    public string SaveLayoutVisualization(IEnumerable<Rectangle> rectangles, Point center, string fileName, Size? customImageSize = null)
    {
        try
        {
            var filePath = Path.Combine(_outputDirectory, fileName);
            var imageSize = customImageSize ?? CalculateOptimalImageSize(rectangles);
            
            var visualizer = _imageGeneratorFactory(
                filePath, 
                imageSize, 
                Color.White, 
                Color.Blue, 
                Color.Red
            );

            visualizer.Visualize(rectangles, center);
            
            return Path.GetFullPath(filePath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save visualization: {ex.Message}", ex);
        }
    }

    private Size CalculateOptimalImageSize(IEnumerable<Rectangle> rectangles)
    {
        var rectList = rectangles.ToList();
        
        if (!rectList.Any())
            return new Size(800, 600);

        var minX = rectList.Min(r => r.Left);
        var maxX = rectList.Max(r => r.Right);
        var minY = rectList.Min(r => r.Top);
        var maxY = rectList.Max(r => r.Bottom);

        var width = Math.Max(800, (maxX - minX) + 200);
        var height = Math.Max(600, (maxY - minY) + 200);

        return new Size(width, height);
    }

    public string GenerateFileName(string fileName)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"{fileName}_{timestamp}.png";
    }
}