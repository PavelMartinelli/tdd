using System.Drawing;

namespace TagsCloudVisualization;

public interface IVisualizer
{
    Bitmap CreateVisualization(IEnumerable<Rectangle> rectangles, Point center, TagCloudVisualizationConfig config);
    string SaveVisualization(IEnumerable<Rectangle> rectangles, Point center, TagCloudVisualizationConfig config);
}