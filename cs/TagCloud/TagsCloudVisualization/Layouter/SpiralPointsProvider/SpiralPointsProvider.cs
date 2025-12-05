using System.Drawing;

namespace TagsCloudVisualization;

public class SpiralPointsProvider : ISpiralPointsProvider
{
    private const double AngleStep = 0.05;
    private const double RadiusStepFactor = 0.05;
    
    private double _currentRadius;
    private double _currentAngle;

    public SpiralPointsProvider()
    {
        _currentRadius = 0;
        _currentAngle = 0;
    }

    public IEnumerable<Point> GetSpiralPoints(Point center, int minDimension)
    {
        while (true)
        {
            var point = PolarMath.PolarToCartesian(_currentRadius, _currentAngle, center);
            
            _currentAngle += AngleStep;
            _currentRadius += minDimension * RadiusStepFactor;

            yield return point;
        }
    }
}