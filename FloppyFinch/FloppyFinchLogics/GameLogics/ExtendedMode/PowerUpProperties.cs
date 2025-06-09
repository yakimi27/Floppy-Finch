using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FloppyFinchLogics.AccountManagement;

namespace FloppyFinchLogics.GameLogics.ExtendedMode;

public class PowerUpProperties
{
    private static UniformGrid? _powerUpSpaceGrid;
    private static int _scoreMultiplayerDuration;
    private static readonly int MaxScoreMultiplierDuration = 5;
    private static int _shieldDuration;
    private static readonly int MaxShieldDuration = 5;
    private static int _jetpackDuration;
    private static readonly int MaxJetpackDuration = 5;

    private static bool _paused;

    private static CancellationTokenSource? _shieldCancellation;

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
        ExtendedModeGame.Jetpack = AccountManager.CurrentAccount == null
            ? MaxJetpackDuration
            : AccountManager.CurrentAccount.PowerUpLevels[0] * MaxJetpackDuration;
    }

    public static async void ScoreMultiplierDuration()
    {
        _scoreMultiplayerDuration = AccountManager.CurrentAccount == null
            ? MaxScoreMultiplierDuration
            : AccountManager.CurrentAccount.PowerUpLevels[1] * MaxScoreMultiplierDuration;

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

    public static async void ShieldDuration()
    {
        if (_shieldCancellation != null)
        {
            _shieldDuration = AccountManager.CurrentAccount == null
                ? MaxShieldDuration
                : AccountManager.CurrentAccount.PowerUpLevels[2] * MaxShieldDuration;
            return;
        }

        _shieldCancellation = new CancellationTokenSource();
        _shieldDuration = AccountManager.CurrentAccount == null
            ? MaxShieldDuration
            : AccountManager.CurrentAccount.PowerUpLevels[2] * MaxShieldDuration;
        ExtendedModeGame.Shield = true;

        try
        {
            while (_shieldDuration > 0)
            {
                await Task.Delay(1000, _shieldCancellation.Token);
                if (!_paused && ExtendedModeGame.Jetpack <= 0)
                    _shieldDuration--;
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            ExtendedModeGame.Shield = false;
            _shieldDuration = 0;
            _shieldCancellation = null;
        }
    }

    public static void UpdateShieldState()
    {
        if (_shieldCancellation != null)
        {
            _shieldCancellation.Cancel();
            _shieldCancellation.Dispose();
            _shieldCancellation = null;
        }

        _shieldDuration = 0;
    }

    public static async void StartProgressBarUpdate(Border progressBarItem, PowerUp.PowerUpType powerUpType)
    {
        var progressBar = (ProgressBar)((StackPanel)progressBarItem.Child).Children[1];
        var duration = powerUpType switch
        {
            PowerUp.PowerUpType.Jetpack => AccountManager.CurrentAccount == null
                ? MaxJetpackDuration
                : AccountManager.CurrentAccount.PowerUpLevels[0] * MaxJetpackDuration,
            PowerUp.PowerUpType.ScoreMultiplier => AccountManager.CurrentAccount == null
                ? MaxScoreMultiplierDuration
                : AccountManager.CurrentAccount.PowerUpLevels[1] * MaxScoreMultiplierDuration,
            PowerUp.PowerUpType.Shield => AccountManager.CurrentAccount == null
                ? MaxShieldDuration
                : AccountManager.CurrentAccount.PowerUpLevels[2] * MaxShieldDuration,
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