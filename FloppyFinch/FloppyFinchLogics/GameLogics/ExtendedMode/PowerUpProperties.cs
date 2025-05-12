using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FloppyFinchLogics.GameLogics.ExtendedMode;

public class PowerUpProperties
{
    private static UniformGrid? _powerUpSpaceGrid;
    private static int _scoreMultiplayerDuration;
    private static readonly int MaxScoreMultiplayerDuration = 15;
    private static int _shieldDuration;
    private static readonly int MaxShieldDuration = 15;
    private static int _jetpackDuration;
    private static readonly int MaxJetpackDuration = 15;

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
        ExtendedModeGame.Jetpack = MaxJetpackDuration;
    }

    public static async void ScoreMultiplierDuration(int seconds = 10)
    {
        _scoreMultiplayerDuration = MaxScoreMultiplayerDuration;

        if (!ExtendedModeGame.ScoreMultiplier)
        {
            ExtendedModeGame.ScoreMultiplier = true;
            while (_scoreMultiplayerDuration != 0)
            {
                await Task.Delay(1000);
                if (!_paused) _scoreMultiplayerDuration--;
            }

            ExtendedModeGame.ScoreMultiplier = false;
        }
    }

    public static async void ShieldDuration(int seconds = 15)
    {
        _shieldDuration = MaxShieldDuration;

        if (!ExtendedModeGame.Shield)
        {
            ExtendedModeGame.Shield = true;
            while (_shieldDuration != 0)
            {
                await Task.Delay(1000);
                if (!_paused && ExtendedModeGame.Jetpack <= 0) _shieldDuration--;
            }

            ExtendedModeGame.Shield = false;
        }
    }

    public static void UpdateShieldState()
    {
        _shieldDuration = 0;
    }

    public static async void StartProgressBarUpdate(Border progressBarItem, PowerUp.PowerUpType powerUpType)
    {
        var progressBar = (ProgressBar)((StackPanel)progressBarItem.Child).Children[1];
        var duration = powerUpType switch
        {
            PowerUp.PowerUpType.Jetpack => MaxJetpackDuration,
            PowerUp.PowerUpType.ScoreMultiplier => MaxScoreMultiplayerDuration,
            PowerUp.PowerUpType.Shield => MaxShieldDuration,
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

            if (powerUpType == PowerUp.PowerUpType.ScoreMultiplier)
            {
                await Task.Delay(50);
                duration = _scoreMultiplayerDuration;
            }

            if (powerUpType == PowerUp.PowerUpType.Shield)
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