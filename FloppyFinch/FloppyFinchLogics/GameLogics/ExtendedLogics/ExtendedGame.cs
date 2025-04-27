using System.Windows.Controls;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class ExtendedGame : Game
{
    private int _heartsQuantity = 3;
    private bool _scoreMultiplyer;
    private bool _shield = true;

    public ExtendedGame(Canvas canvas, TextBlock scoreTextBlock) : base(canvas,
        scoreTextBlock)
    {
    }

    protected override async void GameLoop(object sender, EventArgs e)
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
        if (nextPipe != null && nextPipe.CheckCollision(Bird))
        {
            if (_shield)
            {
                Pipes.Remove(nextPipe);
                GameTimerStop();
                await StartTimer(1);
                GameCanvas.Children.Remove(nextPipe.TopPipe);
                GameCanvas.Children.Remove(nextPipe.BottomPipe);
                await StartTimer(1);
                GameTimerResume();
                _shield = false;
            }
            else
            {
                _heartsQuantity--;
                if (_heartsQuantity == 0)
                {
                    GameOver();
                }
                else
                {
                    Pipes.Remove(nextPipe);
                    GameTimerStop();
                    await StartTimer(1);
                    GameCanvas.Children.Remove(nextPipe.TopPipe);
                    GameCanvas.Children.Remove(nextPipe.BottomPipe);
                    await StartTimer(1);
                    GameTimerResume();
                }
            }
        }

        Pipes.RemoveAll(pipe =>
        {
            if (pipe.IsOutOfBounds)
            {
                GameCanvas.Children.Remove(pipe.TopPipe);
                GameCanvas.Children.Remove(pipe.BottomPipe);
                return true;
            }

            return false;
        });

        if (Bird.IsOutOfBounds(GameCanvas.ActualHeight)) GameOver();
    }

    private async Task StartTimer(int seconds)
    {
        await Task.Delay(seconds * 1000);
    }
}