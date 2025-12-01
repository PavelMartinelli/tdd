using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizer : IVisualizer
{
    private const int DefaultWidth = 800;
    private const int DefaultHeight = 600;
    private const int Padding = 200;
    
    private readonly TagCloudVisualizeConfig _config;
    private readonly IImageSaver _imageSaver;

    public TagCloudVisualizer(TagCloudVisualizeConfig config, IImageSaver imageSaver)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _imageSaver = imageSaver ?? throw new ArgumentNullException(nameof(imageSaver));
    }

    public Bitmap CreateVisualization(IEnumerable<Rectangle> rectangles, Point center, Size? imageSize = null)
    {
        var actualImageSize = imageSize ?? CalculateOptimalImageSize(rectangles);
        var bitmap = new Bitmap(actualImageSize.Width, actualImageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(_config.BackgroundColor);
        
        using var pen = new Pen(_config.RectangleColor, _config.PenWidth);
        using var brush = new SolidBrush(Color.FromArgb(_config.RectangleFillAlpha, _config.RectangleColor));
        
        foreach (var rectangle in rectangles)
        {
            graphics.FillRectangle(brush, rectangle);
            graphics.DrawRectangle(pen, rectangle);
        }
        
        using var centerBrush = new SolidBrush(_config.CenterColor);
        var centerRect = new Rectangle(
            center.X - _config.CenterPointSize / 2, 
            center.Y - _config.CenterPointSize / 2, 
            _config.CenterPointSize, 
            _config.CenterPointSize);
        graphics.FillEllipse(centerBrush, centerRect);
        
        return bitmap;
    }

    public string SaveVisualization(IEnumerable<Rectangle> rectangles, Point center, string fileName, Size? imageSize = null)
    {
        using var bitmap = CreateVisualization(rectangles, center, imageSize);
        return _imageSaver.SaveBitmap(bitmap, fileName);
    }

    private Size CalculateOptimalImageSize(IEnumerable<Rectangle> rectangles)
    {
        var rectList = rectangles.ToList();
        
        if (!rectList.Any())
            return new Size(DefaultWidth, DefaultHeight);

        var minX = rectList.Min(r => r.Left);
        var maxX = rectList.Max(r => r.Right);
        var minY = rectList.Min(r => r.Top);
        var maxY = rectList.Max(r => r.Bottom);

        var width = Math.Max(DefaultWidth, (maxX - minX) + Padding);
        var height = Math.Max(DefaultHeight, (maxY - minY) + Padding);

        return new Size(width, height);
    }
}