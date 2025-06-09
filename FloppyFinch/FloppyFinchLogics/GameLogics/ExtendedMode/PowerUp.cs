using System.Windows;
using System.Windows.Controls;
using FloppyFinchLogics.TextureManagement.PowerUps;

namespace FloppyFinchLogics.GameLogics.ExtendedMode;

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
    private readonly Image _powerUp;

    public PowerUp(Canvas gameCanvas, double x, double y, PowerUpType type)
    {
        _gameCanvas = gameCanvas;
        Type = type;
        _powerUp = new Image
        {
            Width = 40,
            Height = 40,
            Source = PowerUpManager.LoadPowerUp(type)
        };
        Canvas.SetLeft(_powerUp, x);
        Canvas.SetTop(_powerUp, y);
    }

    public PowerUpType Type { get; }

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

    public Image GetShape()
    {
        return _powerUp;
    }
}