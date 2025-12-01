using System.Drawing;

namespace TagsCloudVisualization;

public interface ILayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    IEnumerable<Rectangle> PlacedRectangles { get; }

}