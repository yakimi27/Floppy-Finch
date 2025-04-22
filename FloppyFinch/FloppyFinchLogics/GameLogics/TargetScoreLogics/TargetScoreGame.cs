using System.Windows.Controls;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.TargetScoreLogics;

public class TargetScoreGame : Game
{
    private readonly int _targetScore;
    private readonly ProgressBar _targetScoreProgressBar;
    private bool _isFinalPipeSpawned;
    private int _pipesGenerated;


    public TargetScoreGame(Canvas canvas, TextBlock scoreTextBlock, ref int targetScoreValue,
        ProgressBar targetScoreProgressBar) : base(canvas,
        scoreTextBlock)
    {
        var rand = new Random();
        _targetScore = rand.Next(10, 50);
        targetScoreValue = _targetScore;
        _isFinalPipeSpawned = false;
        _pipesGenerated = 0;
        _targetScoreProgressBar = targetScoreProgressBar;
    }

    protected override void GameLoop(object sender, EventArgs e)
    {
        Bird.Update();

        if (Pipes.Count == 0 ||
            Canvas.GetLeft(Pipes.Last().TopPipe) <
            GameCanvas.ActualWidth - Pipe.PipeSpacing * WindowStateData.WidthScaleFactor)
        {
            if (_pipesGenerated != _targetScore - 1 && !_isFinalPipeSpawned)
            {
                Pipes.Add(new Pipe(GameCanvas, false));
                _pipesGenerated++;
            }
            else if (_pipesGenerated == _targetScore - 1)
            {
                Pipes.Add(new Pipe(GameCanvas, true));
                _isFinalPipeSpawned = true;
                _pipesGenerated++;
            }
        }

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
                UpdateProgressBar();
                UpdateScore();

                if (Score >= _targetScore)
                {
                    GameOver();
                    return;
                }
            }
        }

        var nextPipe = Pipes.FirstOrDefault(pipe => !pipe.IsScored);
        if (nextPipe != null && nextPipe.CheckCollision(Bird))
        {
            GameOver();
            return;
        }

        Pipes.RemoveAll(pipe => pipe.IsOutOfBounds);

        if (Bird.IsOutOfBounds(GameCanvas.ActualHeight)) GameOver();
    }

    private void UpdateProgressBar()
    {
        _targetScoreProgressBar.Value = Score;
    }
}