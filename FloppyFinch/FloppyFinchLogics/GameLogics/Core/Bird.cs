using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FloppyFinchLogics.SoundLogics;
using FloppyFinchLogics.TextureManagement;
using FloppyFinchLogics.WindowLogics;

public class Bird
{
    private const double Gravity = 1.6;
    private const double JumpStrength = -16;
    private const double OscillationAmplitude = 5;
    private const double DeathThreshold = -100;
    private const double InitialXOffset = 35;
    private const double InitialYOffset = 40;

    private static readonly RotateTransform RotateTransformJump = new(-30);

    private readonly Image _bird;
    private readonly double _initialX;
    private readonly double _initialY;
    public readonly RotateTransform RotateTransformStatus = new(0);
    private int _frameCounter;
    private int _frameIndex;

    private BirdSkin _skin;
    private double _velocity;
    private double _waitTime;

    public Bird(Canvas gameCanvas, BirdSkin skin)
    {
        _skin = skin;
        _bird = new Image
        {
            Width = 50,
            Height = 40,
            Source = _skin.Frames[0],
            RenderTransformOrigin = new Point(0.5, 0.5)
        };
        gameCanvas.Children.Add(_bird);
        _initialY = WindowStateData.WindowHeight / 2 - InitialYOffset;
        _initialX = WindowStateData.WindowWidth / 2 - InitialXOffset;
        Canvas.SetLeft(_bird, _initialX);
        Canvas.SetTop(_bird, _initialY);
    }

    public double X => Canvas.GetLeft(_bird);

    public void Jump()
    {
        SoundApplier.PlaySound(SoundApplier.SoundType.Flap);
        _velocity = JumpStrength;
        _bird.RenderTransform = RotateTransformJump;
        RotateTransformStatus.Angle = RotateTransformJump.Angle;
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

        _frameCounter++;
        if (_frameCounter >= _skin.FrameDuration)
        {
            _frameCounter = 0;
            _frameIndex = (_frameIndex + 1) % _skin.Frames.Count;
            _bird.Source = _skin.Frames[_frameIndex];
        }
    }

    public void Wait()
    {
        _waitTime += 0.2;
        var oscillation = Math.Sin(_waitTime) * OscillationAmplitude;
        Canvas.SetTop(_bird, _initialY + oscillation);
        _frameCounter++;
        if (_frameCounter >= _skin.FrameDuration)
        {
            _frameCounter = 0;
            _frameIndex = (_frameIndex + 1) % _skin.Frames.Count;
            _bird.Source = _skin.Frames[_frameIndex];
        }
    }

    public void SetSkin(BirdSkin newSkin)
    {
        _skin = newSkin;
        _frameIndex = 0;
        _frameCounter = 0;
        _bird.Source = _skin.Frames[0];
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

    public double GetVelocity()
    {
        return _velocity;
    }
}