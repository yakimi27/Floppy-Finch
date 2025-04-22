using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics;

public class Game
{
    private static Canvas _gameOverCanvas;
    private readonly DispatcherTimer _gameTimer;
    private readonly Random _rand = new();
    private readonly TextBlock _scoreText;
    protected readonly Canvas GameCanvas;
    protected readonly List<Pipe> Pipes = new();
    protected int Score;


    public Game(Canvas canvas, TextBlock scoreTextBlock)
    {
        GameCanvas = canvas;
        _gameOverCanvas = canvas;
        _scoreText = scoreTextBlock;
        Bird = new Bird(GameCanvas);
        _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
        _gameTimer.Tick += WaitLoop;
        _gameTimer.Start();
        UpdateScore();
    }

    public Bird Bird { get; }

    public void StartCheck(bool started)
    {
        if (started)
        {
            _gameTimer.Tick -= WaitLoop;
            _gameTimer.Tick -= GameLoop;
            _gameTimer.Tick += GameLoop;
        }
    }

    private void WaitLoop(object sender, EventArgs e)
    {
        Bird.Wait();
    }

    protected virtual void GameLoop(object sender, EventArgs e)
    {
        Bird.Update();

        if (Pipes.Count == 0 ||
            Canvas.GetLeft(Pipes.Last().TopPipe) <
            GameCanvas.ActualWidth - Pipe.PipeSpacing * WindowStateData.WidthScaleFactor)
            Pipes.Add(new Pipe(GameCanvas, false));


        if (Bird.RotateTransformStatus.Angle < 80 && Bird.GetVelocity() > 10)
        {
            Bird.RotateTransformStatus.Angle += 10;
            Bird.SetBirdRotation(Bird.RotateTransformStatus.Angle);
        }

        foreach (var pipe in Pipes)
        {
            pipe.Update();
            if (!pipe.IsScored && Canvas.GetLeft(pipe.TopPipe) + pipe.TopPipe.Width < Bird.X)
            {
                pipe.IsScored = true;
                Score++;
                UpdateScore();
            }
        }

        var nextPipe = Pipes.FirstOrDefault(pipe => !pipe.IsScored);
        if (nextPipe != null && nextPipe.CheckCollision(Bird)) GameOver();

        Pipes.RemoveAll(pipe => pipe.IsOutOfBounds);

        if (Bird.IsOutOfBounds(GameCanvas.ActualHeight)) GameOver();
    }

    protected void UpdateScore()
    {
        _scoreText.Text = Score.ToString();
    }

    public event Action<int> OnGameOver;

    public static BitmapSource CaptureGameCanvas()
    {
        var renderTargetBitmap = new RenderTargetBitmap((int)_gameOverCanvas.ActualWidth,
            (int)_gameOverCanvas.ActualHeight, 96,
            96, PixelFormats.Pbgra32);
        renderTargetBitmap.Render(_gameOverCanvas);
        return renderTargetBitmap;
    }

    protected void GameOver()
    {
        _gameTimer.Stop();
        OnGameOver?.Invoke(Score);
    }

    public void PauseGame(bool paused)
    {
        if (paused) _gameTimer.Stop();
        else _gameTimer.Start();
    }
}