using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class Powerup
{
    public enum PowerupType
    {
        Heart,
        Jetpack,
        ScoreMultiplier,
        Shield
    }

    private readonly Canvas _gameCanvas;
    private readonly Rectangle _powerup;

    public Powerup(Canvas gameCanvas, double x, double y, PowerupType type)
    {
        _gameCanvas = gameCanvas;
        Type = type;
        _powerup = new Rectangle
        {
            Width = 40,
            Height = 40,
            Fill = GetPowerupColor(type)
        };
        Canvas.SetLeft(_powerup, x);
        Canvas.SetTop(_powerup, y);
        _gameCanvas.Children.Add(_powerup);
    }

    public PowerupType Type { get; }

    private static Brush GetPowerupColor(PowerupType type)
    {
        return type switch
        {
            PowerupType.Heart => Brushes.Red,
            PowerupType.Jetpack => Brushes.DarkGray,
            PowerupType.ScoreMultiplier => Brushes.Yellow,
            PowerupType.Shield => Brushes.Blue,
            _ => Brushes.White
        };
    }

    public void Update(double speed)
    {
        var left = Canvas.GetLeft(_powerup) - speed;
        Canvas.SetLeft(_powerup, left);
    }

    public bool IsOutOfBounds()
    {
        return Canvas.GetLeft(_powerup) + _powerup.Width < 0;
    }

    public Rect GetBounds()
    {
        return new Rect(Canvas.GetLeft(_powerup), Canvas.GetTop(_powerup), _powerup.Width, _powerup.Height);
    }

    public void Remove()
    {
        _gameCanvas.Children.Remove(_powerup);
    }
}