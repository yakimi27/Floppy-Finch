using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FloppyFinchLogics.TextureManagement;

public class SkinManager
{
    public static BirdSkin LoadClassicSkin()
    {
        var frames = new List<ImageSource>
        {
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_0_classic_0.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_0_classic_1.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_0_classic_2.png")
        };
        return new BirdSkin("Classic", frames, 4);
    }

    public static BirdSkin LoadBlueSkin()
    {
        var frames = new List<ImageSource>
        {
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_1_blue_0.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_1_blue_1.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_1_blue_2.png")
        };
        return new BirdSkin("Blue", frames, 4);
    }

    public static BirdSkin LoadBlackSkin()
    {
        var frames = new List<ImageSource>
        {
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_2_black_0.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_2_black_1.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_2_black_2.png")
        };
        return new BirdSkin("Black", frames, 4);
    }

    public static BirdSkin LoadGreenSkin()
    {
        var frames = new List<ImageSource>
        {
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_3_green_0.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_3_green_1.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_3_green_2.png")
        };
        return new BirdSkin("Green", frames, 4);
    }

    public static BirdSkin LoadPinkSkin()
    {
        var frames = new List<ImageSource>
        {
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_4_pink_0.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_4_pink_1.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_4_pink_2.png")
        };
        return new BirdSkin("Pink", frames, 4);
    }

    public static BirdSkin LoadWhiteSkin()
    {
        var frames = new List<ImageSource>
        {
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_5_white_0.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_5_white_1.png"),
            LoadImage("pack://application:,,,/FloppyFinchWindows;component/Assets/Bird/skin_5_white_2.png")
        };
        return new BirdSkin("Pink", frames, 4);
    }


    public static BirdSkin LoadSkin(string skinName)
    {
        return skinName switch
        {
            "Classic" => LoadClassicSkin(),
            "Blue" => LoadBlueSkin(),
            "Black" => LoadBlackSkin(),
            "Green" => LoadGreenSkin(),
            "Pink" => LoadPinkSkin(),
            "White" => LoadWhiteSkin(),
            _ => LoadClassicSkin()
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