using System.Drawing;
using TagsCloudVisualization;

public class CircularCloudLayouter : ILayouter
{
    private readonly Point _center;
    private readonly List<Rectangle> _placedRectangles;
    private readonly ISpiralPointsProvider _pointsProvider;
    private int _minDimension;

    public CircularCloudLayouter(Point center) : this(center, new SpiralPointsProvider())
    { }

    public CircularCloudLayouter(Point center, ISpiralPointsProvider pointsProvider)
    {
        _center = center;
        _placedRectangles = new List<Rectangle>();
        _pointsProvider = pointsProvider;
        _minDimension = int.MaxValue;
    }

    public IReadOnlyList<Rectangle> PlacedRectangles => _placedRectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must have positive dimensions");

        UpdateMinDimension(rectangleSize);
        
        foreach (var point in _pointsProvider.GetSpiralPoints(_center, _minDimension))
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
        var canMove = true;

        while (canMove && !IntersectsWithAny(rectangle))
        {
            var movedX = TryMoveTowardsCenter(ref rectangle, Axis.X);
            var movedY = TryMoveTowardsCenter(ref rectangle, Axis.Y);
            canMove = movedX || movedY;
        }
    }

    private bool TryMoveTowardsCenter(ref Rectangle rectangle, Axis axis)
    {
        var centerCoord = axis == Axis.X ? rectangle.X + rectangle.Width / 2 : rectangle.Y + rectangle.Height / 2;
        
        var targetCoord = axis == Axis.X ? _center.X : _center.Y;
        var direction = Math.Sign(targetCoord - centerCoord);

        if (direction == 0)
            return false;

        var movedRectangle = rectangle;
        if (axis == Axis.X)
            movedRectangle.X += direction;
        else
            movedRectangle.Y += direction;

        if (IntersectsWithAny(movedRectangle)) 
            return false;
        
        rectangle = movedRectangle;
        return true;
    }

    private bool IntersectsWithAny(Rectangle rectangle)
    {
        return _placedRectangles.Any(rect => rect.IntersectsWith(rectangle));
    }

    private enum Axis { X, Y }
}