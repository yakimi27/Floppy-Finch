using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.Core;

public class Grass
{
    private const int GrassZIndex = 1000;
    private readonly Canvas _gameCanvas;

    private readonly Image _grass1;
    private readonly Image _grass2;
    private readonly double _scrollSpeed;

    private double _grass1Position;
    private double _grass2Position;


    public Grass(Canvas gameCanvas, BitmapImage grassSprite, double scrollSpeed = 5)
    {
        _gameCanvas = gameCanvas;
        _scrollSpeed = scrollSpeed;
        var allowedHeight = (int)WindowStateData.WindowPaddingBottom;

        _grass1 = new Image
        {
            Source = grassSprite,
            Stretch = Stretch.Fill,
            Width = allowedHeight * 20,
            Height = allowedHeight
        };

        _grass2 = new Image
        {
            Source = grassSprite,
            Stretch = Stretch.Fill,
            Width = allowedHeight * 20,
            Height = allowedHeight
        };
        Canvas.SetZIndex(_grass1, GrassZIndex);
        Canvas.SetZIndex(_grass2, GrassZIndex);


        _gameCanvas.Children.Add(_grass1);
        _gameCanvas.Children.Add(_grass2);

        _grass1Position = 0;
        _grass2Position = _grass1.Width;
        UpdateGrassPositions();

        Canvas.SetBottom(_grass1, 0);
        Canvas.SetBottom(_grass2, 0);
    }

    public void Update()
    {
        _grass1Position -= _scrollSpeed;
        _grass2Position -= _scrollSpeed;

        if (_grass1Position <= -_grass1.Width) _grass1Position = _grass2Position + _grass2.Width;
        if (_grass2Position <= -_grass2.Width) _grass2Position = _grass1Position + _grass1.Width;

        UpdateGrassPositions();
    }

    private void UpdateGrassPositions()
    {
        Canvas.SetLeft(_grass1, _grass1Position);
        Canvas.SetLeft(_grass2, _grass2Position);
    }

    public void Cleanup()
    {
        if (_grass1 != null)
            _gameCanvas.Children.Remove(_grass1);
        if (_grass2 != null)
            _gameCanvas.Children.Remove(_grass2);
    }
}