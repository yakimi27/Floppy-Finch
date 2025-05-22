using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FloppyFinchLogics.GameLogics.Core;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchLogics.GameLogics.ExtendedMode;

public class ExtendedModeGame : Game
{
    private const int PowerUpHeight = 40;
    private const int PowerUpSpawnChance = 5; // 5% chance per game loop
    private const double PowerUpSpeed = 5;
    private const int MaxHearts = 3;
    private const int HeartPowerUpIndex = 1;
    private const int JetpackPowerUpIndex = 2;
    private const int ScoreMultiplierPowerUpIndex = 3;
    private const int PipeOffset = 100;
    private readonly TextBlock _heartsTextBlock;
    private readonly Border _itemJetpack;
    private readonly Border _itemScoreMultiplayer;
    private readonly Border _itemShield;
    private readonly List<PowerUp> _powerUps = new();
    private readonly UniformGrid? _powerUpSpaceGrid;

    private bool _gamePaused;


    public ExtendedModeGame(Canvas canvas, TextBlock scoreTextBlock, TextBlock heartsTextBlock,
        UniformGrid? powerUpSpaceGrid) : base(canvas,
        scoreTextBlock)
    {
        Hearts = 1;
        Jetpack = 0;
        Shield = false;
        ScoreMultiplier = false;
        _heartsTextBlock = heartsTextBlock;
        _powerUpSpaceGrid = powerUpSpaceGrid;
        PowerUpProperties.Initialize(powerUpSpaceGrid);
        _itemJetpack = PowerUpProgress.CreateItem("Jetpack", true);
        _itemScoreMultiplayer = PowerUpProgress.CreateItem("Score Multiplayer", true);
        _itemShield = PowerUpProgress.CreateItem("Shield", true);
    }

    internal static int Jetpack { get; set; } /*jetpack duration is upgradeable and sets in config*/

    internal static int Hearts { get; set; }

    internal static bool ScoreMultiplier { get; set; } /*score multiplayer duration is upgradeable and sets in config*/

    internal static bool Shield { get; set; } /*shield duration is upgradeable and sets in config*/

    protected override async void GameLoop(object sender, EventArgs e)
    {
        if (Jetpack <= 0)
            Bird.Update();
        _grassScroller.Update();

        if (Pipes.Count == 0 ||
            Canvas.GetLeft(Pipes.Last().TopPipe) <
            GameCanvas.ActualWidth - Pipe.PipeSpacing * WindowStateData.WidthScaleFactor)
            Pipes.Add(new Pipe(GameCanvas, false, PipeOffset));

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
                PowerUpProperties.UpdateShieldState();
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
                    if (!_gamePaused) GameTimerResume();
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

        if (Random.Next(1, 250) < PowerUpSpawnChance && _powerUps.Count == 0 && Jetpack <= 0)
        {
            var x = GameCanvas.ActualWidth + PipeOffset - PowerUpHeight;
            var y = Random.NextDouble() *
                    (GameCanvas.ActualHeight - PowerUpHeight - (int)WindowStateData.WindowPaddingBottom);
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
                powerUp.Update(PowerUpSpeed);

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

        if (Bird.IsOutOfBounds(GameCanvas.ActualHeight - (int)WindowStateData.WindowPaddingBottom)) GameOver();
    }

    private void ApplyPowerUp(PowerUp.PowerUpType powerUpType)
    {
        Border progressBarItem = null;

        switch (powerUpType)
        {
            case PowerUp.PowerUpType.Heart:
                PowerUpProperties.HeartPickup();
                UpdateHearts();
                return;
            case PowerUp.PowerUpType.Jetpack:
                PowerUpProperties.JetpackDuration();
                progressBarItem = _itemJetpack;
                break;
            case PowerUp.PowerUpType.ScoreMultiplier:
                if (!ScoreMultiplier)
                    progressBarItem = _itemScoreMultiplayer;
                PowerUpProperties.ScoreMultiplierDuration();
                break;
            case PowerUp.PowerUpType.Shield:
                if (!Shield)
                    progressBarItem = _itemShield;
                PowerUpProperties.ShieldDuration();
                break;
        }

        if (progressBarItem != null)
        {
            if (!_powerUpSpaceGrid.Children.Contains(progressBarItem)) _powerUpSpaceGrid.Children.Add(progressBarItem);
            PowerUpProperties.StartProgressBarUpdate(progressBarItem, powerUpType);
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
            HeartPowerUpIndex when CanReceiveHeart() => PowerUp.PowerUpType.Heart,
            JetpackPowerUpIndex => PowerUp.PowerUpType.Jetpack,
            ScoreMultiplierPowerUpIndex => PowerUp.PowerUpType.ScoreMultiplier,
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

    public override void PauseGame(bool paused)
    {
        PowerUpProperties.SetPauseState(paused);
        _gamePaused = !_gamePaused;
        base.PauseGame(paused);
    }
}