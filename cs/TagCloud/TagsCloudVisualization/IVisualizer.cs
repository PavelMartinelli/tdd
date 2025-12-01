using System.Drawing;

namespace TagsCloudVisualization;

public interface IVisualizer
{
    Bitmap CreateVisualization(IEnumerable<Rectangle> rectangles, Point center, Size? imageSize = null);
    string SaveVisualization(IEnumerable<Rectangle> rectangles, Point center, string fileName, Size? imageSize = null);
}