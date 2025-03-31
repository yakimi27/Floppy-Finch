using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FloppyFinchWPF;

public class Game
{
    private readonly Canvas _gameCanvas;
    private readonly DispatcherTimer _gameTimer;
    private readonly List<Pipe> _pipes = new();
    private readonly Random _rand = new();
    private readonly TextBlock _scoreText;

    /*public static double Coefficient { get; set; } = ((Panel)Application.Current.MainWindow.Content).ActualHeight /
                                           ((Panel)Application.Current.MainWindow.Content).ActualWidth;*/

    private int _score;


    public Game(Canvas canvas, TextBlock scoreTextBlock)
    {
        _gameCanvas = canvas;
        _scoreText = scoreTextBlock;
        Bird = new Bird(_gameCanvas);
        _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
        _gameTimer.Tick += GameLoop;
        _gameTimer.Start();

        UpdateScore();
    }

    public Bird Bird { get; }

    private void GameLoop(object sender, EventArgs e)
    {
        Bird.Update();

        // add new pipe according to spacing
        if (_pipes.Count == 0 || Canvas.GetLeft(_pipes.Last().TopPipe) < _gameCanvas.ActualWidth - Pipe.PipeSpacing)
            _pipes.Add(new Pipe(_gameCanvas));


        if (Bird.RotateTransformStatus.Angle < 80 && Bird.GetVelocity() > 10)
        {
            Bird.RotateTransformStatus.Angle += 10;
            Bird.SetBirdRotation(Bird.RotateTransformStatus.Angle);
        }

        for (var i = _pipes.Count - 1; i >= 0; i--)
        {
            _pipes[i].Update();

            if (!_pipes[i].IsScored && Canvas.GetLeft(_pipes[i].TopPipe) + _pipes[i].TopPipe.Width < Bird.X)
            {
                _pipes[i].IsScored = true; // mark pipe as scored
                _score++;
                UpdateScore(); // update score text
            }

            if (_pipes[i].CheckCollision(Bird)) GameOver();
            if (_pipes[i].IsOutOfBounds) _pipes.RemoveAt(i);
        }

        if (Bird.IsOutOfBounds(_gameCanvas.ActualHeight)) GameOver();
    }

    /*
    private void UpdateScore()
    {
        _scoreText.Text = $"{_score}";
    }*/
    private void UpdateScore()
    {
        _scoreText.Text = _score.ToString();
    }

    private void GameOver()
    {
        _gameTimer.Stop();
        var looseWindow = new LooseWindow(_score);
        looseWindow.Show();
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
    }
}