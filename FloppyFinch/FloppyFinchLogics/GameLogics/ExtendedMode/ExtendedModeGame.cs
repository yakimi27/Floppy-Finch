using System.Windows.Controls;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.ExtendedLogics;

public class ExtendedModeGame : Game
{
    private const int PowerupHeight = 40;
    private const int PowerupSpawnChance = 5; // 5% chance per game loop
    private const double PowerupSpeed = 5;
    private const int MaxHearts = 3;
    private const int HeartPowerupIndex = 1;
    private const int JetpackPowerupIndex = 2;
    private const int ScoreMultiplierPowerupIndex = 3;
    private const int pipeOffset = 100;
    private readonly TextBlock _heartsTextBlock;
    private readonly List<PowerUp> _powerUps = new();

    public ExtendedModeGame(Canvas canvas, TextBlock scoreTextBlock, TextBlock heartsTextBlock) : base(canvas,
        scoreTextBlock)
    {
        Hearts = 1;
        Jetpack = 0;
        Shield = false;
        ScoreMultiplier = false;
        _heartsTextBlock = heartsTextBlock;
    }

    internal static int Jetpack { private get; set; } /*jetpack duration is upgradeable and sets in config*/

    internal static int Hearts { get; set; }

    internal static bool ScoreMultiplier { get; set; } /*score multiplayer duration is upgradeable and sets in config*/

    internal static bool Shield { get; set; } /*shield duration is upgradeable and sets in config*/

    protected override async void GameLoop(object sender, EventArgs e)
    {
        if (Jetpack <= 0)
            Bird.Update();

        if (Pipes.Count == 0 ||
            Canvas.GetLeft(Pipes.Last().TopPipe) <
            GameCanvas.ActualWidth - Pipe.PipeSpacing * WindowStateData.WidthScaleFactor)
            Pipes.Add(new Pipe(GameCanvas, false, pipeOffset));

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
                UpdateHearts();
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

        if (Random.Next(1, 500) < PowerupSpawnChance && _powerUps.Count == 0 && Jetpack <= 0)
        {
            var x = GameCanvas.ActualWidth + pipeOffset - 40;
            var y = Random.NextDouble() * (GameCanvas.ActualHeight - PowerupHeight);
            var tempPowerUp = new PowerUp(GameCanvas, x, y, DeterminePowerUpTypeByIndex(Random.Next(1, 5)));
            var tempBounds = tempPowerUp.GetBoundsManual(x, y);

            if (!Pipes.Any(pipe => pipe.CheckPowerUpCollision(tempBounds)))
            {
                GameCanvas.Children.Add(tempPowerUp.GetShape());
                _powerUps.Add(tempPowerUp);
            }
        }

        for (var i = _powerUps.Count - 1; i >= 0; i--)
        {
            var powerUp = _powerUps[i];
            if (Jetpack > 0)
                powerUp.Update(50);
            else
                powerUp.Update(PowerupSpeed);

            if (powerUp.GetBounds().IntersectsWith(Bird.GetBounds()))
            {
                ApplyPowerUp(powerUp.Type);
                powerUp.Remove();
                _powerUps.RemoveAt(i);
                continue;
            }

            if (powerUp.IsOutOfBounds())
            {
                powerUp.Remove();
                _powerUps.RemoveAt(i);
            }
        }

        if (Bird.IsOutOfBounds(GameCanvas.ActualHeight)) GameOver();
    }

    private void ApplyPowerUp(PowerUp.PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUp.PowerUpType.Heart:
                PowerUpProperties.HeartPickup();
                UpdateHearts();
                break;
            case PowerUp.PowerUpType.Jetpack:
                PowerUpProperties.JetpackDuration();
                break;
            case PowerUp.PowerUpType.ScoreMultiplier:
                PowerUpProperties.ScoreMultiplierDuration();
                break;
            case PowerUp.PowerUpType.Shield:
                PowerUpProperties.ShieldDuration();
                break;
        }
    }

    private async Task StartTimer(int seconds)
    {
        await Task.Delay(seconds * 1000);
    }

    private PowerUp.PowerUpType DeterminePowerUpTypeByIndex(int powerUpIndex)
    {
        return powerUpIndex switch
        {
            HeartPowerupIndex when CanReceiveHeart() => PowerUp.PowerUpType.Heart,
            JetpackPowerupIndex => PowerUp.PowerUpType.Jetpack,
            ScoreMultiplierPowerupIndex => PowerUp.PowerUpType.ScoreMultiplier,
            _ => PowerUp.PowerUpType.Shield
        };
    }

    private bool CanReceiveHeart()
    {
        return Hearts < MaxHearts;
    }

    private void UpdateHearts()
    {
        _heartsTextBlock.Text = Hearts.ToString();
    }
}