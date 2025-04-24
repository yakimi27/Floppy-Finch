using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinchLogics.GameLogics;

public class Pipe
{
    private const int PipeWidth = 100;
    private const int GapSize = 155;
    private const double DefaultSpeed = 5;
    private const double VerticalMarginFactor = 0.1;
    private const int MaxStepSize = 200;

    internal static readonly int PipeSpacing = 250;
    private static readonly Random Random = new();
    private static int _lastGapY = -1;

    private readonly Rectangle _bottomPipe;
    private readonly Canvas _gameCanvas;
    private readonly bool _isFinal;

    internal readonly Rectangle TopPipe;

    public Pipe(Canvas canvas, bool isFinal)
    {
        _gameCanvas = canvas;
        _isFinal = isFinal;

        var pipeColor = _isFinal ? Brushes.Gold : Brushes.Green;
        var height = CalculateGapHeight(canvas);

        (TopPipe, _bottomPipe) = CreatePipes(canvas, height, pipeColor);
        InitializePipePositions(canvas, height);

        _gameCanvas.Children.Add(TopPipe);
        _gameCanvas.Children.Add(_bottomPipe);
    }

    public bool IsScored { get; set; }
    public bool IsOutOfBounds => Canvas.GetLeft(TopPipe) < -PipeWidth;

    private int CalculateGapHeight(Canvas canvas)
    {
        var verticalMargin = (int)(canvas.ActualHeight * VerticalMarginFactor);
        var minY = verticalMargin;
        var maxY = (int)(canvas.ActualHeight - GapSize - verticalMargin);

        int height;
        if (_lastGapY == -1)
        {
            height = Random.Next(minY, maxY);
        }
        else
        {
            var stepMin = Math.Max(minY, _lastGapY - MaxStepSize);
            var stepMax = Math.Min(maxY, _lastGapY + MaxStepSize);
            height = Random.Next(stepMin, stepMax);
        }

        _lastGapY = height;
        return height;
    }

    private static (Rectangle top, Rectangle bottom) CreatePipes(Canvas canvas, int height, Brush color)
    {
        var top = new Rectangle { Width = PipeWidth, Height = height, Fill = color };
        var bottom = new Rectangle
        {
            Width = PipeWidth,
            Height = canvas.ActualHeight - height - GapSize,
            Fill = color
        };
        return (top, bottom);
    }

    private void InitializePipePositions(Canvas canvas, int height)
    {
        Canvas.SetLeft(TopPipe, canvas.ActualWidth);
        Canvas.SetTop(TopPipe, 0);
        Canvas.SetLeft(_bottomPipe, canvas.ActualWidth);
        Canvas.SetTop(_bottomPipe, height + GapSize);
    }

    public void Update(double speed = DefaultSpeed)
    {
        var newX = Canvas.GetLeft(TopPipe) - speed;
        Canvas.SetLeft(TopPipe, newX);
        Canvas.SetLeft(_bottomPipe, newX);
    }

    public bool CheckCollision(Bird bird)
    {
        var birdBounds = bird.GetBounds();
        return birdBounds.IntersectsWith(new Rect(Canvas.GetLeft(TopPipe), Canvas.GetTop(TopPipe),
                   TopPipe.Width, TopPipe.Height)) ||
               birdBounds.IntersectsWith(new Rect(Canvas.GetLeft(_bottomPipe), Canvas.GetTop(_bottomPipe),
                   _bottomPipe.Width, _bottomPipe.Height));
    }
}