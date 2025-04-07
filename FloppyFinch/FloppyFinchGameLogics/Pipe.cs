using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinchGameLogics;

public class Pipe
{
    private const int PipeWidth = 100;
    private const int GapSize = 190;
    internal const int PipeSpacing = 250;
    private const double Speed = 5;
    private readonly Rectangle _bottomPipe;
    private readonly Canvas _gameCanvas;
    internal readonly Rectangle TopPipe;

    public Pipe(Canvas canvas)
    {
        _gameCanvas = canvas;
        Random rand = new();

        var height = rand.Next(50, (int)(canvas.ActualHeight - GapSize - 100));

        TopPipe = new Rectangle { Width = PipeWidth, Height = height, Fill = Brushes.Green };
        _bottomPipe = new Rectangle
            { Width = PipeWidth, Height = canvas.ActualHeight - height - GapSize, Fill = Brushes.Green };

        Canvas.SetLeft(TopPipe, canvas.ActualWidth);
        Canvas.SetTop(TopPipe, 0);
        Canvas.SetLeft(_bottomPipe, canvas.ActualWidth);
        Canvas.SetTop(_bottomPipe, height + GapSize);

        _gameCanvas.Children.Add(TopPipe);
        _gameCanvas.Children.Add(_bottomPipe);
    }

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