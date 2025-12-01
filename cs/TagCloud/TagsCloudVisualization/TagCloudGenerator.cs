using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudGenerator
{
    private readonly IVisualizer _visualizer;
    private readonly Func<Point, ILayouter> _layouterFactory;
    private readonly IRectangleSizeProvider _sizeProvider;

    public TagCloudGenerator(
        IVisualizer visualizer,
        Func<Point, ILayouter> layouterFactory,
        IRectangleSizeProvider sizeProvider)
    {
        _visualizer = visualizer ?? throw new ArgumentNullException(nameof(visualizer));
        _layouterFactory = layouterFactory ?? throw new ArgumentNullException(nameof(layouterFactory));
        _sizeProvider = sizeProvider ?? throw new ArgumentNullException(nameof(sizeProvider));
    }

    public string Generate(TagCloudGenerationConfig config)
    {
        var layouter = _layouterFactory(config.Center);
        var sizes = _sizeProvider.GetSizes(config.RectangleCount, config.MinSize, config.MaxSize);
        
        foreach (var size in sizes)
            layouter.PutNextRectangle(size);
        
        return _visualizer.SaveVisualization(
            rectangles: layouter.PlacedRectangles,
            center: config.Center,
            fileName: config.OutputFileName,
            imageSize: config.ImageSize
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
}