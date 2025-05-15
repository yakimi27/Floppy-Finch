using System.Windows.Controls;
using FloppyFinchLogics.GameLogics.Core;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.SpeedRaceMode;

public class SpeedRaceModeGame : Game
{
    private readonly int _gameSpeed;

    public SpeedRaceModeGame(Canvas canvas, TextBlock scoreTextBlock, int gameSpeed) : base(canvas,
        scoreTextBlock)
    {
        _gameSpeed = gameSpeed;
    }

    protected override void GameLoop(object sender, EventArgs e)
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
            pipe.Update(_gameSpeed);
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
}