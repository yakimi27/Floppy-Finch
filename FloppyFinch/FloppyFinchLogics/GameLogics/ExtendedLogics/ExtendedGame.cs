using System.Windows.Controls;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class ExtendedGame : Game
{
    private const int PowerupHeight = 40;
    private const int PowerupSpawnChance = 5; // 5% chance per game loop
    private const double PowerupSpeed = 5;
    private readonly List<Powerup> _powerups = new();

    public ExtendedGame(Canvas canvas, TextBlock scoreTextBlock) : base(canvas,
        scoreTextBlock)
    {
    }

    internal static int Jetpack { private get; set; } /*jetpack duration is upgradeable and sets in config*/

    internal static int Hearts { get; set; } = 1;

    internal static bool ScoreMultiplier { get; set; } /*score multiplayer duration is upgradeable and sets in config*/

    internal static bool Shield { get; set; } /*shield duration is upgradeable and sets in config*/

    protected override async void GameLoop(object sender, EventArgs e)
    {
        if (Jetpack <= 0)
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
            if (Jetpack > 0)
                pipe.Update(50);
            else
                pipe.Update();


            if (!pipe.IsScored && Canvas.GetLeft(pipe.TopPipe) + pipe.TopPipe.Width < Bird.X)
            {
                pipe.IsScored = true;
                Score += ScoreMultiplier ? 2 : 1;
                UpdateScore();
                Jetpack--;
            }
        }

        var nextPipe = Pipes.FirstOrDefault(pipe => !pipe.IsScored);
        if (nextPipe != null && nextPipe.CheckCollision(Bird) && Jetpack <= 0)
        {
            if (Shield)
            {
                Pipes.Remove(nextPipe);
                GameCanvas.Children.Remove(nextPipe.TopPipe);
                GameCanvas.Children.Remove(nextPipe.BottomPipe);
                Shield = false;
            }
            else
            {
                Hearts--;
                if (Hearts == 0)
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

        if (Random.Next(1, 100) < PowerupSpawnChance && _powerups.Count == 0)
        {
            var x = GameCanvas.ActualWidth;
            var y = Random.NextDouble() * (GameCanvas.ActualHeight - PowerupHeight);
            _powerups.Add(new Powerup(GameCanvas, x, y, RanodmPowerupType(Random.Next(1, 4))));
        }

        for (var i = _powerups.Count - 1; i >= 0; i--)
        {
            var powerup = _powerups[i];
            if (Jetpack > 0)
                powerup.Update(50);
            else
                powerup.Update(PowerupSpeed);

            if (powerup.GetBounds().IntersectsWith(Bird.GetBounds()))
            {
                ApplyPowerup(powerup.Type);
                powerup.Remove();
                _powerups.RemoveAt(i);
                continue;
            }

            if (powerup.IsOutOfBounds())
            {
                powerup.Remove();
                _powerups.RemoveAt(i);
            }
        }

        if (Bird.IsOutOfBounds(GameCanvas.ActualHeight)) GameOver();
    }

    private void ApplyPowerup(Powerup.PowerupType powerupType)
    {
        switch (powerupType)
        {
            case Powerup.PowerupType.Heart:
                PowerupProperties.HeartPickup();
                break;
            case Powerup.PowerupType.Jetpack:
                PowerupProperties.JetpackDuration();
                break;
            case Powerup.PowerupType.ScoreMultiplier:
                PowerupProperties.ScoreMultiplierDuration();
                break;
            case Powerup.PowerupType.Shield:
                PowerupProperties.ShieldDuration();
                break;
        }
    }

    private async Task StartTimer(int seconds)
    {
        await Task.Delay(seconds * 1000);
    }

    private Powerup.PowerupType RanodmPowerupType(int powerupIndex)
    {
        switch (powerupIndex)
        {
            case 1: return Powerup.PowerupType.Heart;
            case 2: return Powerup.PowerupType.Jetpack;
            case 3: return Powerup.PowerupType.ScoreMultiplier;
            case 4: return Powerup.PowerupType.Shield;
        }

        return Powerup.PowerupType.Heart;
    }
}