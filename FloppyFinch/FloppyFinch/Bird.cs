using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinch;

public class Bird
{
    private const double Gravity = 0.5;
    private const double JumpStrength = -10;
    private readonly Rectangle bird;
    private double velocity;

    public Bird(Canvas gameCanvas)
    {
        bird = new Rectangle { Width = 40, Height = 40, Fill = Brushes.Yellow };
        gameCanvas.Children.Add(bird);
        Canvas.SetLeft(bird, 100);
        Canvas.SetTop(bird, 200);
    }

    public void Jump()
    {
        velocity = JumpStrength;
    }

    public void Update()
    {
        velocity += Gravity;
        Canvas.SetTop(bird, Canvas.GetTop(bird) + velocity);
    }

    public bool IsOutOfBounds(double height)
    {
        var top = Canvas.GetTop(bird);
        return top < 0 || top > height;
    }

    public Rect GetBounds()
    {
        return new Rect(Canvas.GetLeft(bird), Canvas.GetTop(bird), bird.Width, bird.Height);
    }
}