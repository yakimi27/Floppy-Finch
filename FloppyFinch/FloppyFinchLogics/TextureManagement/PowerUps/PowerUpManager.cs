using System.Windows.Media;
using System.Windows.Media.Imaging;
using FloppyFinchLogics.GameLogics.ExtendedMode;

namespace FloppyFinchLogics.TextureManagement.PowerUps;

public static class PowerUpManager
{
    private const string BaseImagePath = "pack://application:,,,/FloppyFinchWindows;component/Assets/PowerUp";

    private static BitmapImage LoadHeartPowerUp()
    {
        var texture = LoadImage($"{BaseImagePath}/heart_powerup.png");
        return texture;
    }

    private static BitmapImage LoadJetpackPowerUp()
    {
        var texture = LoadImage($"{BaseImagePath}/jetpack_powerup.png");
        return texture;
    }

    private static BitmapImage LoadScoreMultiplierPowerUp()
    {
        var texture = LoadImage($"{BaseImagePath}/score_multiplier_powerup.png");
        return texture;
    }

    private static BitmapImage LoadShieldPowerUp()
    {
        var texture = LoadImage($"{BaseImagePath}/shield_powerup.png");
        return texture;
    }

    public static BitmapImage LoadPowerUp(PowerUp.PowerUpType type)
    {
        return type switch
        {
            PowerUp.PowerUpType.Heart => LoadHeartPowerUp(),
            PowerUp.PowerUpType.Jetpack => LoadJetpackPowerUp(),
            PowerUp.PowerUpType.ScoreMultiplier => LoadScoreMultiplierPowerUp(),
            PowerUp.PowerUpType.Shield => LoadShieldPowerUp(),
            _ => new BitmapImage()
        };
    }

    private static BitmapImage LoadImage(string uri)
    {
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(uri, UriKind.Absolute);
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.CreateOptions = BitmapCreateOptions.None;
        RenderOptions.SetBitmapScalingMode(bitmap, BitmapScalingMode.NearestNeighbor);
        bitmap.EndInit();
        bitmap.Freeze();
        return bitmap;
    }
}