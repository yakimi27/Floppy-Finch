using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FloppyFinchLogics.TextureManagement.Background;

public class BackgroundManager
{
    private const string BaseImagePath = "pack://application:,,,/FloppyFinchWindows;component/Assets/Background";

    public static BitmapImage LoadBackground(string selectedBackgoround)
    {
        return selectedBackgoround == "Dark" ? LoadDarkBackground() : LoadLightBackground();
    }

    private static BitmapImage LoadLightBackground()
    {
        var texture = LoadImage($"{BaseImagePath}/background_light.png");
        return texture;
    }

    private static BitmapImage LoadDarkBackground()
    {
        var texture = LoadImage($"{BaseImagePath}/background_dark.png");
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