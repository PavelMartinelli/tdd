using System.Drawing;

namespace TagsCloudVisualization;

public interface ISpiralPointsProvider
{
    IEnumerable<Point> GetSpiralPoints(Point center, int minDimension);
}