using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizationConfig
{
    public string OutputFileName { get; }
    public Size? ImageSize { get; }
    public Color BackgroundColor { get; }
    public Color RectangleColor { get; }
    public Color CenterColor { get; }
    public int PenWidth { get; }
    public int CenterPointSize { get; }
    public int RectangleFillAlpha { get; }

    public TagCloudVisualizationConfig(
        string outputFileName,
        Size? imageSize = null,
        Color? backgroundColor = null,
        Color? rectangleColor = null,
        Color? centerColor = null,
        int penWidth = 2,
        int centerPointSize = 5,
        int rectangleFillAlpha = 50)
    {
        OutputFileName = outputFileName ?? throw new ArgumentNullException(nameof(outputFileName));
        ImageSize = imageSize;
        BackgroundColor = backgroundColor ?? Color.White;
        RectangleColor = rectangleColor ?? Color.Blue;
        CenterColor = centerColor ?? Color.Red;
        PenWidth = penWidth;
        CenterPointSize = centerPointSize;
        RectangleFillAlpha = rectangleFillAlpha;
    }
}