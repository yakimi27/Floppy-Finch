using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics;

public class Pipe
{
    private const int PipeWidth = 100;
    private const int GapSize = 165;
    private const double Speed = 5;
    internal static readonly int PipeSpacing = 250;
    private readonly Rectangle _bottomPipe;
    private readonly Canvas _gameCanvas;
    internal readonly Rectangle TopPipe;

    public Pipe(Canvas canvas, bool isFinal)
    {
        _gameCanvas = canvas;
        Random rand = new();
        IsFinal = isFinal;

        Brush pipeColor = IsFinal ? Brushes.Gold : Brushes.Green;

        var height = rand.Next((int)(50 * WindowStateData.HeightScaleFactor),
            (int)(canvas.ActualHeight - GapSize - 100 * WindowStateData.HeightScaleFactor));

        TopPipe = new Rectangle { Width = PipeWidth, Height = height, Fill = pipeColor };
        _bottomPipe = new Rectangle
            { Width = PipeWidth, Height = canvas.ActualHeight - height - GapSize, Fill = pipeColor };

        Canvas.SetLeft(TopPipe, canvas.ActualWidth);
        Canvas.SetTop(TopPipe, 0);
        Canvas.SetLeft(_bottomPipe, canvas.ActualWidth);
        Canvas.SetTop(_bottomPipe, height + GapSize);

        _gameCanvas.Children.Add(TopPipe);
        _gameCanvas.Children.Add(_bottomPipe);
    }

    public bool IsFinal { get; }

    public bool IsScored { get; set; } = false;

    public bool IsOutOfBounds => Canvas.GetLeft(TopPipe) < -PipeWidth;

    public void Update()
    {
        var newX = Canvas.GetLeft(TopPipe) - Speed;
        Canvas.SetLeft(TopPipe, newX);
        Canvas.SetLeft(_bottomPipe, newX);
    }

    /*
    public bool CheckCollision(Bird bird)
    {
        var birdBounds = bird.GetBounds();
        var topBounds = new Rect(Canvas.GetLeft(TopPipe), Canvas.GetTop(TopPipe), TopPipe.Width, TopPipe.Height);
        var bottomBounds = new Rect(Canvas.GetLeft(_bottomPipe), Canvas.GetTop(_bottomPipe), _bottomPipe.Width,
            _bottomPipe.Height);
        return birdBounds.IntersectsWith(topBounds) || birdBounds.IntersectsWith(bottomBounds);
    }*/
    public bool CheckCollision(Bird bird)
    {
        var birdBounds = bird.GetBounds();
        return birdBounds.IntersectsWith(new Rect(Canvas.GetLeft(TopPipe), Canvas.GetTop(TopPipe), TopPipe.Width,
                   TopPipe.Height)) ||
               birdBounds.IntersectsWith(new Rect(Canvas.GetLeft(_bottomPipe), Canvas.GetTop(_bottomPipe),
                   _bottomPipe.Width, _bottomPipe.Height));
    }
}