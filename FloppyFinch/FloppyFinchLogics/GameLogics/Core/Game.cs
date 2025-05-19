using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.TextureManagement;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.Core;

public class Game
{
    protected const int BirdFallRotation = 10;
    protected const int BirdVelocityToRotate = 10;

    private static Canvas _gameOverCanvas;
    private readonly DispatcherTimer _gameTimer;
    private readonly TextBlock _scoreText;
    protected readonly Canvas GameCanvas;
    protected readonly List<Pipe> Pipes = new();
    protected readonly Random Random = new();
    protected int Score;

    public Game(Canvas canvas, TextBlock scoreTextBlock)
    {
        GameCanvas = canvas;
        _gameOverCanvas = canvas;
        _scoreText = scoreTextBlock;
        var currentSkin = SkinManager.LoadSkin(AccountManager.CurrentAccount!.SelectedSkin);
        Bird = new Bird(GameCanvas, currentSkin);
        _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
        _gameTimer.Tick += WaitLoop!;
        _gameTimer.Start();
        UpdateScore();
    }

    protected static int BirdMaxRotation { get; } = 80;

    public Bird Bird { get; }

    public void StartCheck(bool started)
    {
        if (started)
        {
            _gameTimer.Tick -= WaitLoop!;
            _gameTimer.Tick -= GameLoop!;
            _gameTimer.Tick += GameLoop!;
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


        if (Bird.RotateTransformStatus.Angle < BirdMaxRotation && Bird.GetVelocity() > BirdVelocityToRotate)
        {
            Bird.RotateTransformStatus.Angle += BirdFallRotation;
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
        OnGameOver(Score);
    }

    public virtual void PauseGame(bool paused)
    {
        if (paused) _gameTimer.Stop();
        else _gameTimer.Start();
    }

    protected void GameTimerStop()
    {
        _gameTimer.Stop();
    }

    protected void GameTimerResume()
    {
        _gameTimer.Start();
    }
}