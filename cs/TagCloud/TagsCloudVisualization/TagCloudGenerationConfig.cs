using System.Drawing;

namespace TagsCloudVisualization;


public class TagCloudGenerationConfig
{
    public Point Center { get; set; }
    public int RectangleCount { get; set; }
    public Size MinSize { get; set; }
    public Size MaxSize { get; set; }
    public string OutputFileName { get; set; }
    public Size? ImageSize { get; set; }

    public TagCloudGenerationConfig(Point center, int rectangleCount, Size minSize, Size maxSize, 
        string outputFileName, Size? imageSize = null)
    {
        Center = center;
        RectangleCount = rectangleCount;
        MinSize = minSize;
        MaxSize = maxSize;
        OutputFileName = outputFileName;
        ImageSize = imageSize;
    }
}