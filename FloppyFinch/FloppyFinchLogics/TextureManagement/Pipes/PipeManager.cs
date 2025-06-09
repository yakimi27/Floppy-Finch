using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FloppyFinchLogics.TextureManagement.Pipes;

public class PipeManager
{
    private const string BaseImagePath = "pack://application:,,,/FloppyFinchWindows;component/Assets/Pipe";

    public static BitmapImage LoadTopPipe(string type)
    {
        var texture = type == "Default" ? LoadDefaultTopPipe() : LoadGoldTopPipe();
        return texture;
    }

    public static BitmapImage LoadBottomPipe(string type)
    {
        var texture = type == "Default" ? LoadDefaultBottomPipe() : LoadGoldBottomPipe();
        return texture;
    }


    private static BitmapImage LoadDefaultTopPipe()
    {
        var texture = LoadImage($"{BaseImagePath}/pipe_default_top.png");
        return texture;
    }

    private static BitmapImage LoadDefaultBottomPipe()
    {
        var texture = LoadImage($"{BaseImagePath}/pipe_default_bottom.png");
        return texture;
    }

    private static BitmapImage LoadGoldTopPipe()
    {
        var texture = LoadImage($"{BaseImagePath}/pipe_gold_top.png");
        return texture;
    }

    private static BitmapImage LoadGoldBottomPipe()
    {
        var texture = LoadImage($"{BaseImagePath}/pipe_gold_bottom.png");
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