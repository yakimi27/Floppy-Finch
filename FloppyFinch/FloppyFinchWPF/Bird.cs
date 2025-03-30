using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinchWPF;

public class Bird
{
    private const double Gravity = 1.6;
    private const double JumpStrength = -16;
    private readonly Rectangle _bird;
    private double _velocity;

    public Bird(Canvas gameCanvas)
    {
        _bird = new Rectangle { Width = 40, Height = 40, Fill = Brushes.Yellow };
        gameCanvas.Children.Add(_bird);
        Canvas.SetLeft(_bird, 100);
        Canvas.SetTop(_bird, 200);
    }

    public double X => Canvas.GetLeft(_bird);

    public void Jump()
    {
        _velocity = JumpStrength;
    }

    public void Update()
    {
        _velocity += Gravity;
        Canvas.SetTop(_bird, Canvas.GetTop(_bird) + _velocity);
    }

    public bool IsOutOfBounds(double height)
    {
        var top = Canvas.GetTop(_bird);
        return top < -100 || top > height;
    }

    public Rect GetBounds()
    {
        return new Rect(Canvas.GetLeft(_bird), Canvas.GetTop(_bird), _bird.Width, _bird.Height);
    }
}