using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FloppyFinchLogics.TextureManagement.Pipes;

namespace FloppyFinchLogics.GameLogics.Core;

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
    private readonly Canvas _gameCanvas;
    private readonly bool _isFinal;


    internal readonly Image BottomPipe;
    internal readonly Image TopPipe;
    private Rect _bottomPipeCollisionBounds;
    private Rect _topPipeCollisionBounds;

    public Pipe(Canvas canvas, bool isFinal, double pipeOffset = 0)
    {
        _gameCanvas = canvas;
        _isFinal = isFinal;

        var pipeType = _isFinal ? "Gold" : "Default";
        var height = CalculateGapHeight(canvas);

        (TopPipe, BottomPipe) = CreatePipes(canvas, height, pipeType);
        InitializePipePositions(canvas, height, pipeOffset);

        _gameCanvas.Children.Add(TopPipe);
        _gameCanvas.Children.Add(BottomPipe);
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

    private static (Image top, Image bottom) CreatePipes(Canvas canvas, int height, string type)
    {
        var top = new Image
        {
            Width = PipeWidth,
            Source = PipeManager.LoadTopPipe(type),
            Stretch = Stretch.Fill
        };
        var bottom = new Image
        {
            Width = PipeWidth,
            Source = PipeManager.LoadBottomPipe(type),
            Stretch = Stretch.Fill
        };
        return (top, bottom);
    }

    private void InitializePipePositions(Canvas canvas, int height, double pipeOffset)
    {
        Canvas.SetLeft(TopPipe, canvas.ActualWidth + pipeOffset);
        Canvas.SetBottom(TopPipe, canvas.ActualHeight - height);
        Canvas.SetLeft(BottomPipe, canvas.ActualWidth);
        Canvas.SetTop(BottomPipe, height + GapSize);

        _topPipeCollisionBounds = new Rect(canvas.ActualWidth + pipeOffset, 0, PipeWidth, height);
        _bottomPipeCollisionBounds = new Rect(canvas.ActualWidth, height + GapSize, PipeWidth,
            canvas.ActualHeight - (height + GapSize));
    }

    public void Update(double speed = DefaultSpeed)
    {
        var newX = Canvas.GetLeft(TopPipe) - speed;
        Canvas.SetLeft(TopPipe, newX);
        Canvas.SetLeft(BottomPipe, newX);

        _topPipeCollisionBounds.X = newX;
        _bottomPipeCollisionBounds.X = newX;
    }

    public bool CheckCollision(Bird bird)
    {
        var birdBounds = bird.GetBounds();
        return birdBounds.IntersectsWith(_topPipeCollisionBounds) ||
               birdBounds.IntersectsWith(_bottomPipeCollisionBounds);
    }

    public bool CheckPowerUpCollision(Rect powerUpBounds)
    {
        return powerUpBounds.IntersectsWith(_topPipeCollisionBounds) ||
               powerUpBounds.IntersectsWith(_bottomPipeCollisionBounds);
    }
}