using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FloppyFinchLogics.GameLogics;

public class Game
{
    private static Canvas _gameOverCanvas;
    private readonly Canvas _gameCanvas;
    private readonly DispatcherTimer _gameTimer;
    private readonly List<Pipe> _pipes = new();
    private readonly Random _rand = new();
    private readonly TextBlock _scoreText;
    private int _score;


    public Game(Canvas canvas, TextBlock scoreTextBlock)
    {
        _gameCanvas = canvas;
        _gameOverCanvas = canvas;
        _scoreText = scoreTextBlock;
        Bird = new Bird(_gameCanvas);
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
            _gameTimer.Tick += GameLoop;
        }
    }

    public void WaitLoop(object sender, EventArgs e)
    {
        Bird.Wait();
    }

    private void GameLoop(object sender, EventArgs e)
    {
        Bird.Update();


        // add new pipe according to spacing
        if (_pipes.Count == 0 ||
            Canvas.GetLeft(_pipes.Last().TopPipe) <
            _gameCanvas.ActualWidth - Pipe.PipeSpacing - 200 /*add new variable dependent on screen width*/
           ) _pipes.Add(new Pipe(_gameCanvas));


        if (Bird.RotateTransformStatus.Angle < 80 && Bird.GetVelocity() > 10)
        {
            Bird.RotateTransformStatus.Angle += 10;
            Bird.SetBirdRotation(Bird.RotateTransformStatus.Angle);
        }

        /*for (var i = _pipes.Count - 1; i >= 0; i--)
        {
            _pipes[i].Update();

            if (!_pipes[i].IsScored && Canvas.GetLeft(_pipes[i].TopPipe) + _pipes[i].TopPipe.Width < Bird.X /** 1.5#1#)
            {
                _pipes[i].IsScored = true; // mark pipe as scored
                _score++;
                UpdateScore(); // update score text
            }

            if (!_pipes[i].IsScored)
            {
                if (_score == 0)
                {
                    if (_pipes[i].CheckCollision(Bird)) GameOver();
                    if (_pipes[i].IsOutOfBounds) _pipes.RemoveAt(i);
                }
                else if (_pipes[i - 1].IsScored)
                {
                    if (_pipes[i].CheckCollision(Bird)) GameOver();
                    if (_pipes[i].IsOutOfBounds) _pipes.RemoveAt(i);
                }
            }
        }*/

        foreach (var pipe in _pipes)
        {
            pipe.Update();
            if (!pipe.IsScored && Canvas.GetLeft(pipe.TopPipe) + pipe.TopPipe.Width < Bird.X)
            {
                pipe.IsScored = true;
                _score++;
                UpdateScore();
            }
        }

        var nextPipe = _pipes.FirstOrDefault(pipe => !pipe.IsScored);
        if (nextPipe != null && nextPipe.CheckCollision(Bird)) GameOver();

        _pipes.RemoveAll(pipe => pipe.IsOutOfBounds);

        if (Bird.IsOutOfBounds(_gameCanvas.ActualHeight)) GameOver();
    }

    private void UpdateScore()
    {
        _scoreText.Text = _score.ToString();
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

    private void GameOver()
    {
        _gameTimer.Stop();
        OnGameOver?.Invoke(_score);
    }
}