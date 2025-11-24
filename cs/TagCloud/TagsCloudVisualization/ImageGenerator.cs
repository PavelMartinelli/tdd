using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class ImageGenerator
{
    private readonly string outputPath;
    private readonly Size imageSize;
    private readonly Color backgroundColor;
    private readonly Color rectangleColor;
    private readonly Color centerColor;

    public ImageGenerator(string outputPath, Size imageSize, 
        Color? backgroundColor = null, 
        Color? rectangleColor = null,
        Color? centerColor = null)
    {
        this.outputPath = outputPath;
        this.imageSize = imageSize;
        this.backgroundColor = backgroundColor ?? Color.White;
        this.rectangleColor = rectangleColor ?? Color.Blue;
        this.centerColor = centerColor ?? Color.Red;
    }

    public void Visualize(IEnumerable<Rectangle> rectangles, Point center)
    {
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(backgroundColor);
        
        using var pen = new Pen(rectangleColor, 2);
        using var brush = new SolidBrush(Color.FromArgb(50, rectangleColor));
        
        foreach (var rectangle in rectangles)
        {
            graphics.FillRectangle(brush, rectangle);
            graphics.DrawRectangle(pen, rectangle);
        }
        
        using var centerBrush = new SolidBrush(centerColor);
        var centerSize = 5;
        var centerRect = new Rectangle(
            center.X - centerSize / 2, 
            center.Y - centerSize / 2, 
            centerSize, 
            centerSize);
        graphics.FillEllipse(centerBrush, centerRect);
        
        bitmap.Save(outputPath, ImageFormat.Png);
    }
}