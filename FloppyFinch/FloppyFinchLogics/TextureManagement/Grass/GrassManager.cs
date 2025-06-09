using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FloppyFinchLogics.TextureManagement.Grass;

public class GrassManager
{
    private const string BaseImagePath = "pack://application:,,,/FloppyFinchWindows;component/Assets/Grass";

    public static BitmapImage LoadGrass()
    {
        var texture = LoadImage($"{BaseImagePath}/grass.png");
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