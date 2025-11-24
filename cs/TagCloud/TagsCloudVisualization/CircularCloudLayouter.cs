using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const double AngleStep = 0.05;
    private const double RadiusStepFactor = 0.05;
    
    private readonly Point _center;
    private readonly List<Rectangle> _placedRectangles;
    private double _currentRadius;
    private double _currentAngle;
    private int _minDimension;

    public CircularCloudLayouter(Point center)
    {
        _center = center;
        _placedRectangles = new List<Rectangle>();
        _currentRadius = 0;
        _currentAngle = 0;
        _minDimension = int.MaxValue;
    }

    public IEnumerable<Rectangle> PlacedRectangles => _placedRectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must have positive dimensions");

        UpdateMinDimension(rectangleSize);
        
        foreach (var point in GetSpiralPoints())
        {
            var candidateRectangle = CreateRectangleAtPoint(rectangleSize, point);

            if (IntersectsWithAny(candidateRectangle)) 
                continue;
            
            CompactRectangle(ref candidateRectangle);
            _placedRectangles.Add(candidateRectangle);
            return candidateRectangle;
        }

        throw new InvalidOperationException("Failed to find position for rectangle");
    }

    private void UpdateMinDimension(Size size)
    {
        _minDimension = Math.Min(_minDimension, Math.Min(size.Width, size.Height));
    }

    private Rectangle CreateRectangleAtPoint(Size size, Point point)
    {
        return new Rectangle(
            point.X - size.Width / 2,
            point.Y - size.Height / 2,
            size.Width,
            size.Height);
    }

    private void CompactRectangle(ref Rectangle rectangle)
    {
        var canMoveX = true;
        var canMoveY = true;

        while ((canMoveX || canMoveY) && !IntersectsWithAny(rectangle))
        {
            canMoveX = TryMoveTowardsCenterX(ref rectangle);
            canMoveY = TryMoveTowardsCenterY(ref rectangle);
        }
    }

    private bool TryMoveTowardsCenterX(ref Rectangle rectangle)
    {
        var centerX = rectangle.X + rectangle.Width / 2;
        var direction = Math.Sign(_center.X - centerX);

        if (direction == 0)
            return false;

        var movedRectangle = rectangle;
        movedRectangle.X += direction;

        if (IntersectsWithAny(movedRectangle)) 
            return false;
        
        rectangle = movedRectangle;
        return true;
    }

    private bool TryMoveTowardsCenterY(ref Rectangle rectangle)
    {
        var centerY = rectangle.Y + rectangle.Height / 2;
        var direction = Math.Sign(_center.Y - centerY);

        if (direction == 0)
            return false;

        var movedRectangle = rectangle;
        movedRectangle.Y += direction;

        if (IntersectsWithAny(movedRectangle)) 
            return false;
        
        rectangle = movedRectangle;
        return true;
    }

    private IEnumerable<Point> GetSpiralPoints()
    {
        while (true)
        {
            var point = PolarMath.PolarToCartesian(_currentRadius, _currentAngle, _center);
            
            _currentAngle += AngleStep;
            _currentRadius += _minDimension * RadiusStepFactor;

            yield return point;
        }
    }

    private bool IntersectsWithAny(Rectangle rectangle)
    {
        return _placedRectangles.Any(rect => rect.IntersectsWith(rectangle));
    }
}