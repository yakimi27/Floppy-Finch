using System.Windows.Media;

namespace FloppyFinchLogics.TextureManagement;

public class BirdSkin
{
    public BirdSkin(string name, List<ImageSource> frames, int frameDuration)
    {
        Name = name;
        Frames = frames;
        FrameDuration = frameDuration;
    }

    public string Name { get; }
    public List<ImageSource> Frames { get; }
    public int FrameDuration { get; }
}