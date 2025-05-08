using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class PowerUp
{
    public enum PowerUpType
    {
        Heart,
        Jetpack,
        ScoreMultiplier,
        Shield
    }

    private readonly Canvas _gameCanvas;
    private readonly Rectangle _powerUp;

    public PowerUp(Canvas gameCanvas, double x, double y, PowerUpType type)
    {
        _gameCanvas = gameCanvas;
        Type = type;
        _powerUp = new Rectangle
        {
            Width = 40,
            Height = 40,
            Fill = GetPowerupColor(type)
        };
        Canvas.SetLeft(_powerUp, x);
        Canvas.SetTop(_powerUp, y);
        /*_gameCanvas.Children.Add(_powerUp);*/
    }

    public PowerUpType Type { get; }

    private static Brush GetPowerupColor(PowerUpType type)
    {
        return type switch
        {
            PowerUpType.Heart => Brushes.Red,
            PowerUpType.Jetpack => Brushes.DarkGray,
            PowerUpType.ScoreMultiplier => Brushes.Yellow,
            PowerUpType.Shield => Brushes.Blue,
            _ => Brushes.White
        };
    }

    public void Update(double speed)
    {
        var left = Canvas.GetLeft(_powerUp) - speed;
        Canvas.SetLeft(_powerUp, left);
    }

    public bool IsOutOfBounds()
    {
        return Canvas.GetLeft(_powerUp) + _powerUp.Width < 0;
    }

    public Rect GetBounds()
    {
        return new Rect(Canvas.GetLeft(_powerUp), Canvas.GetTop(_powerUp), _powerUp.Width, _powerUp.Height);
    }

    public Rect GetBoundsManual(double x, double y)
    {
        return new Rect(x, y, _powerUp.Width, _powerUp.Height);
    }

    public void Remove()
    {
        _gameCanvas.Children.Remove(_powerUp);
    }

    public Rectangle GetShape()
    {
        return _powerUp;
    }
}