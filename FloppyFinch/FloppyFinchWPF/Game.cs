using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FloppyFinchWPF;

public class Game
{
    private const int PipeSpacing = 250;
    private readonly Canvas gameCanvas;
    private readonly DispatcherTimer gameTimer;
    private readonly List<Pipe> pipes = new();
    private readonly Random rand = new();

    public Game(Canvas canvas)
    {
        gameCanvas = canvas;
        Bird = new Bird(gameCanvas);
        gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
        gameTimer.Tick += GameLoop;
        gameTimer.Start();
    }

    public Bird Bird { get; }

    private void GameLoop(object sender, EventArgs e)
    {
        Bird.Update();

        // add new pipe according to spacing
        if (pipes.Count == 0 || Canvas.GetLeft(pipes.Last().topPipe) < gameCanvas.ActualWidth - PipeSpacing)
            pipes.Add(new Pipe(gameCanvas));

        for (var i = pipes.Count - 1; i >= 0; i--)
        {
            pipes[i].Update();
            if (pipes[i].IsOutOfBounds) pipes.RemoveAt(i);

            if (pipes[i].CheckCollision(Bird)) GameOver();
        }

        if (Bird.IsOutOfBounds(gameCanvas.ActualHeight)) GameOver();
    }

    private void GameOver()
    {
        gameTimer.Stop();
        MessageBox.Show("Game Over!");
    }
}