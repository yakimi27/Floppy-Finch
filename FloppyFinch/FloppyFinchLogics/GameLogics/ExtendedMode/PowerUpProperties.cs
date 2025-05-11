using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FloppyFinchLogics.GameLogics.ExtendedMode;

public class PowerUpProperties
{
    private static UniformGrid? _powerUpSpaceGrid;
    private static int _scoreMultiplayerDuration;
    private static int _shieldDuration;
    private static bool _paused;


    public static void Initialize(UniformGrid? powerUpSpaceGrid)
    {
        _powerUpSpaceGrid = powerUpSpaceGrid;
    }

    public static void SetPauseState(bool paused)
    {
        _paused = paused;
    }


    public static void HeartPickup()
    {
        ExtendedModeGame.Hearts++;
    }

    public static void JetpackDuration()
    {
        ExtendedModeGame.Jetpack = 15;
    }

    public static async void ScoreMultiplierDuration(int seconds = 5)
    {
        _scoreMultiplayerDuration = seconds;
        ExtendedModeGame.ScoreMultiplier = true;
        while (seconds != 0)
        {
            await Task.Delay(seconds * 1000);
            if (!_paused)
            {
                seconds--;
                _scoreMultiplayerDuration--;
            }
        }

        ExtendedModeGame.ScoreMultiplier = false;
    }

    public static async void ShieldDuration(int seconds = 15)
    {
        _shieldDuration = seconds;
        ExtendedModeGame.Shield = true;
        while (seconds != 0)
        {
            await Task.Delay(seconds * 1000);
            if (!_paused)
            {
                seconds--;
                _shieldDuration--;
            }
        }

        ExtendedModeGame.Shield = false;
    }

    public static async void StartProgressBarUpdate(Border progressBarItem, PowerUp.PowerUpType powerUpType)
    {
        var progressBar = (ProgressBar)((StackPanel)progressBarItem.Child).Children[1];
        var duration = powerUpType switch
        {
            PowerUp.PowerUpType.Jetpack => 15,
            PowerUp.PowerUpType.ScoreMultiplier => 5,
            PowerUp.PowerUpType.Shield => 15,
            _ => 0
        };

        progressBar.Maximum = duration;
        progressBar.Value = duration;

        while (duration > 0)
        {
            if (powerUpType == PowerUp.PowerUpType.Jetpack)
            {
                await Task.Delay(50);
                duration = ExtendedModeGame.Jetpack;
            }
            else if (powerUpType == PowerUp.PowerUpType.ScoreMultiplier)
            {
                await Task.Delay(50);
                duration = _scoreMultiplayerDuration;
            }
            else
            {
                await Task.Delay(50);
                duration = _shieldDuration;
            }

            progressBar.Value = duration;

            if (powerUpType == PowerUp.PowerUpType.Shield && !ExtendedModeGame.Shield)
                break;
        }

        _powerUpSpaceGrid.Children.Remove(progressBarItem);
    }
}