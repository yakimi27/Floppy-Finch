using System.IO;
using System.Windows.Media;

namespace FloppyFinchLogics.SoundLogics;

public class SoundApplier
{
    public static async void Button_MouseEnter()
    {
        var mouseHover = new MediaPlayer();
        mouseHover.Open(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Sounds/hover.wav")));
        mouseHover.Volume = 0.2;
        mouseHover.Play();
    }

    public static async void Button_MouseClick()
    {
        var mouseHover = new MediaPlayer();
        mouseHover.Open(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Sounds/click.wav")));
        mouseHover.Volume = 1.0;
        mouseHover.Play();
    }
}