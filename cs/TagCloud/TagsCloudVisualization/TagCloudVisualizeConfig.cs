using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizeConfig
{
    public Size? ImageSize { get; }
    public Color BackgroundColor { get; }
    public Color RectangleColor { get; }
    public Color CenterColor { get; }
    public int PenWidth { get; }
    public int CenterPointSize { get; }
    public int RectangleFillAlpha { get; }

    public TagCloudVisualizeConfig(
        Color backgroundColor,
        Color rectangleColor,
        Color centerColor,
        int penWidth = 2,
        int centerPointSize = 5,
        int rectangleFillAlpha = 50, 
        Size? imageSize = null)
    {
        ImageSize = imageSize;
        BackgroundColor = backgroundColor;
        RectangleColor = rectangleColor;
        CenterColor = centerColor;
        PenWidth = penWidth;
        CenterPointSize = centerPointSize;
        RectangleFillAlpha = rectangleFillAlpha;
    }
    
    public static TagCloudVisualizeConfig Default => new TagCloudVisualizeConfig(
        backgroundColor: Color.White,
        rectangleColor: Color.Blue,
        centerColor: Color.Red);
}