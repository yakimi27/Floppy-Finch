using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics;

public class Bird
{
    private const double Gravity = 1.6; /*originally 1.6*/
    private const double JumpStrength = -16; /*originally -16*/
    private const double OscillationAmplitude = 5;
    private const double DeathThreshold = -100;
    private const double InitialXOffset = 35;
    private const double InitialYOffset = 40;

    private static readonly RotateTransform RotateTransformJump = new(-30);
    private readonly Rectangle _bird;
    private readonly double _initialX;
    private readonly double _initialY;
    private double _velocity;
    private double _waitTime;
    public RotateTransform RotateTransformStatus = new(0);

    public Bird(Canvas gameCanvas)
    {
        _bird = new Rectangle
            { Width = 50, Height = 40, Fill = Brushes.Yellow, RenderTransformOrigin = new Point(0.5, 0.5) };
        gameCanvas.Children.Add(_bird);
        _initialY = WindowStateData.WindowHeight / 2 - InitialYOffset;
        _initialX = WindowStateData.WindowWidth / 2 - InitialXOffset;
        Canvas.SetLeft(_bird, _initialX);
        Canvas.SetTop(_bird, _initialY);
    }

    public double X => Canvas.GetLeft(_bird);

    public void Jump()
    {
        _velocity = JumpStrength;
        _bird.RenderTransform = RotateTransformJump;
        RotateTransformStatus.Angle = RotateTransformJump.Angle;
    }

    public double GetVelocity()
    {
        return _velocity;
    }

    public void SetBirdRotation(double angle)
    {
        RotateTransformStatus.Angle = angle;
        _bird.RenderTransform = RotateTransformStatus;
    }

    public void Update()
    {
        _velocity += Gravity;
        Canvas.SetTop(_bird, Canvas.GetTop(_bird) + _velocity);
    }

    public void Wait()
    {
        _waitTime += 0.2;
        var oscillation = Math.Sin(_waitTime) * OscillationAmplitude;
        Canvas.SetTop(_bird, _initialY + oscillation);
    }

    public bool IsOutOfBounds(double height)
    {
        var top = Canvas.GetTop(_bird);
        return top < DeathThreshold || top > height;
    }

    public Rect GetBounds()
    {
        return new Rect(Canvas.GetLeft(_bird), Canvas.GetTop(_bird), _bird.Width, _bird.Height);
    }
}