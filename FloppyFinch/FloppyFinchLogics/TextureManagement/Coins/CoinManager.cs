using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FloppyFinchLogics.TextureManagement.Coins;

public class CoinManager
{
    private const string BaseImagePath = "pack://application:,,,/FloppyFinchWindows;component/Assets/Coin";

    public static BitmapImage LoadCoin()
    {
        var texture = LoadImage($"{BaseImagePath}/coin.png");
        return texture;
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