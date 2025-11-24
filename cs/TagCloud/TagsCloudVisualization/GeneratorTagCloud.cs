using System.Drawing;

namespace TagsCloudVisualization;

public class GeneratorTagCloud
{
    private readonly TagCloudVisualizer _cloudVisualizer;
    private readonly Func<Point, CircularCloudLayouter> _layouterFactory;
    private readonly Random _random;

    public GeneratorTagCloud(TagCloudVisualizer cloudVisualizer)
    {
        _cloudVisualizer = cloudVisualizer;
        _layouterFactory = center => new CircularCloudLayouter(center);
        _random = new Random();
    }

    public string Generate(TagCloudGenerationConfig config)
    {
        var layouter = _layouterFactory(config.Center);
        
        var sizes = GenerateSizes(config.RectangleCount, config.MinSize, config.MaxSize);
        
        foreach (var size in sizes)
            layouter.PutNextRectangle(size);
        
        return _cloudVisualizer.SaveLayoutVisualization(
            rectangles: layouter.PlacedRectangles, 
            center: config.Center, 
            fileName: config.OutputFileName,
            customImageSize: config.ImageSize
        );
    }

    public List<string> GenerateMultiple(IEnumerable<TagCloudGenerationConfig> configs)
    {
        var results = new List<string>();
        
        foreach (var config in configs)
        {
            try
            {
                var filePath = Generate(config);
                results.Add(filePath);
            }
            catch (Exception ex)
            {
                results.Add($"ERROR for {config.OutputFileName}: {ex.Message}");
            }
        }
        
        return results;
    }

    private List<Size> GenerateSizes(int count, Size minSize, Size maxSize)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive", nameof(count));
        
        if (minSize.Width <= 0 || minSize.Height <= 0)
            throw new ArgumentException("Min size must have positive dimensions", nameof(minSize));
        
        if (maxSize.Width < minSize.Width || maxSize.Height < minSize.Height)
            throw new ArgumentException("Max size must be greater than or equal to min size", nameof(maxSize));

        var sizes = new List<Size>();
        for (var i = 0; i < count; i++)
        {
            var width = _random.Next(minSize.Width, maxSize.Width + 1);
            var height = _random.Next(minSize.Height, maxSize.Height + 1);
            sizes.Add(new Size(width, height));
        }
        return sizes;
    }
}