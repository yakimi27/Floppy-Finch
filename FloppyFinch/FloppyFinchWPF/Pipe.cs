using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinchWPF;

public class Pipe
{
    private const double Speed = 5;
    private const int PipeWidth = 80;
    private const int GapSize = 150;
    private readonly Rectangle bottomPipe;
    private readonly Canvas gameCanvas;
    internal readonly Rectangle topPipe;

    public Pipe(Canvas canvas)
    {
        gameCanvas = canvas;
        Random rand = new();

        var height = rand.Next(50, (int)(canvas.ActualHeight - GapSize - 50));

        topPipe = new Rectangle { Width = PipeWidth, Height = height, Fill = Brushes.Green };
        bottomPipe = new Rectangle
            { Width = PipeWidth, Height = canvas.ActualHeight - height - GapSize, Fill = Brushes.Green };

        Canvas.SetLeft(topPipe, canvas.ActualWidth);
        Canvas.SetTop(topPipe, 0);
        Canvas.SetLeft(bottomPipe, canvas.ActualWidth);
        Canvas.SetTop(bottomPipe, height + GapSize);

        gameCanvas.Children.Add(topPipe);
        gameCanvas.Children.Add(bottomPipe);
    }

    public bool IsOutOfBounds => Canvas.GetLeft(topPipe) < -PipeWidth;

    public void Update()
    {
        var newX = Canvas.GetLeft(topPipe) - Speed;
        Canvas.SetLeft(topPipe, newX);
        Canvas.SetLeft(bottomPipe, newX);
    }

    public bool CheckCollision(Bird bird)
    {
        var birdBounds = bird.GetBounds();
        var topBounds = new Rect(Canvas.GetLeft(topPipe), Canvas.GetTop(topPipe), topPipe.Width, topPipe.Height);
        var bottomBounds = new Rect(Canvas.GetLeft(bottomPipe), Canvas.GetTop(bottomPipe), bottomPipe.Width,
            bottomPipe.Height);

        return birdBounds.IntersectsWith(topBounds) || birdBounds.IntersectsWith(bottomBounds);
    }
}