using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private ILayouter layouter;
    private Point center;
    private TagCloudVisualizer testVisualizer;
    private IImageSaver imageSaver;

    [SetUp]
    public void SetUp()
    {
        center = new Point(100, 100);
        layouter = new CircularCloudLayouter(center);
        var dir = Directory.CreateDirectory($"../../../../TagsCloudVisualizationTests/test_results");
        imageSaver = new ImageSaver(dir.FullName);
        var visualizerConfig = TagCloudVisualizeConfig.Default;
        testVisualizer = new TagCloudVisualizer(visualizerConfig, imageSaver);
        
    }
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) 
            return;
        try
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var fileName = imageSaver.GenerateFileName(testName);
            var filePath = testVisualizer.SaveVisualization(
                layouter.PlacedRectangles, 
                center, 
                fileName
            );
            
            TestContext.Out.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Failed to save visualization: {ex.Message}");
        }
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_PlacedInCenter()
    {
        var size = new Size(10, 10);
        
        var actual = layouter.PutNextRectangle(size);
        
        var expected = new Rectangle(
            new Point(center.X - size.Width / 2, center.Y - size.Height / 2), 
            size);
        actual.Should().Be(expected);
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_DoNotIntersect()
    {
        for (var i = 0; i < 5; i++)
            layouter.PutNextRectangle(new Size(45, 15));
        
        var rectangles = layouter.PlacedRectangles.ToList();
        rectangles.Should().NotBeEmpty();
        
        for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_LayoutIsDense()
    {
        for (var i = 0; i < 50; i++)
            layouter.PutNextRectangle(new Size(45, 15));
        
        const double minDensityRatio = 0.3;
        
        var totalRectanglesArea = layouter.PlacedRectangles
            .Sum(rectangle => rectangle.Width * rectangle.Height);
        
        var boundingCircleRadius = CalculateBoundingCircleRadius();
        var boundingCircleArea = Math.PI * boundingCircleRadius * boundingCircleRadius;
        
        var density = totalRectanglesArea / boundingCircleArea;
        density.Should().BeGreaterThan(minDensityRatio);
    }
    
    [Test]
    public void PutNextRectangle_ManyRectangles_FormsCircularShape()
    {
        for (var i = 0; i < 150; i++)
            layouter.PutNextRectangle(new Size(45, 15));
        
        var rectangles = layouter.PlacedRectangles.ToList();
        
        var quadrants = new int[4];
    
        foreach (var rect in rectangles)
        {
            var rectCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            
            if (rectCenter.X >= center.X && rectCenter.Y <= center.Y) 
                quadrants[0]++;
            else if (rectCenter.X >= center.X && rectCenter.Y > center.Y) 
                quadrants[1]++;
            else if (rectCenter.X < center.X && rectCenter.Y > center.Y) 
                quadrants[2]++;
            else 
                quadrants[3]++;
        }
        
        quadrants.All(count => count > 0).Should().BeTrue();
        
        var maxCount = quadrants.Max();
        var minCount = quadrants.Min();
        (maxCount <= minCount * 2).Should().BeTrue();
    }

    private double CalculateBoundingCircleRadius()
    {
        return layouter.PlacedRectangles
            .SelectMany(rect => new[]
            {
                GetDistance(rect.Location, center),
                GetDistance(new Point(rect.Right, rect.Top), center),
                GetDistance(new Point(rect.Left, rect.Bottom), center),
                GetDistance(new Point(rect.Right, rect.Bottom), center)
            })
            .Max();
    }

    private double GetDistance(Point point1, Point point2)
    {
        return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
    }
}