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
    private readonly TextBlock _heartsTextBlock;
    private readonly List<PowerUp> _powerups = new();

    public ExtendedModeGame(Canvas canvas, TextBlock scoreTextBlock, TextBlock HeartsTextBlock) : base(canvas,
        scoreTextBlock)
    {
        Hearts = 1;
        Jetpack = 0;
        Shield = false;
        ScoreMultiplier = false;
        _heartsTextBlock = HeartsTextBlock;
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

        if (Random.Next(1, 1000) < PowerupSpawnChance && _powerups.Count == 0 && Jetpack <= 0)
        {
            var x = GameCanvas.ActualWidth;
            var y = Random.NextDouble() * (GameCanvas.ActualHeight - PowerupHeight);
            var newPowerup = new PowerUp(GameCanvas, x, y, DeterminePowerupTypeByIndex(Random.Next(1, 5)));

            if (!Pipes.Any(pipe => pipe.CheckPowerupCollision(newPowerup))) _powerups.Add(newPowerup);
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

    private void ApplyPowerup(PowerUp.PowerupType powerupType)
    {
        switch (powerupType)
        {
            case PowerUp.PowerupType.Heart:
                PowerUpProperties.HeartPickup();
                UpdateHearts();
                break;
            case PowerUp.PowerupType.Jetpack:
                PowerUpProperties.JetpackDuration();
                break;
            case PowerUp.PowerupType.ScoreMultiplier:
                PowerUpProperties.ScoreMultiplierDuration();
                break;
            case PowerUp.PowerupType.Shield:
                PowerUpProperties.ShieldDuration();
                break;
        }
    }

    private async Task StartTimer(int seconds)
    {
        await Task.Delay(seconds * 1000);
    }

    private PowerUp.PowerupType DeterminePowerupTypeByIndex(int powerupIndex)
    {
        return powerupIndex switch
        {
            HeartPowerupIndex when CanReceiveHeart() => PowerUp.PowerupType.Heart,
            JetpackPowerupIndex => PowerUp.PowerupType.Jetpack,
            ScoreMultiplierPowerupIndex => PowerUp.PowerupType.ScoreMultiplier,
            _ => PowerUp.PowerupType.Shield
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