using System.IO;
using System.Windows.Media;
using FloppyFinchLogics.AccountManagement;

namespace FloppyFinchLogics.SoundLogics;

public static class SoundApplier
{
    public enum SoundType
    {
        MouseHover,
        MouseClick,
        Flap,
        Score,
        Hit,
        Swoosh
    }

    private static readonly Dictionary<SoundType, (string Path, double Volume)> SoundConfigurations = new()
    {
        { SoundType.MouseHover, ("Assets/Sounds/hover.wav", 0.2) },
        { SoundType.MouseClick, ("Assets/Sounds/click.wav", 1.2) },
        { SoundType.Flap, ("Assets/Sounds/flap.wav", 1.0) },
        { SoundType.Score, ("Assets/Sounds/score.wav", 1.0) },
        { SoundType.Hit, ("Assets/Sounds/hit.wav", 1.0) },
        { SoundType.Swoosh, ("Assets/Sounds/swoosh.wav", 1.0) }
    };

    public static void PlaySound(SoundType soundType)
    {
        if (!AccountManager.CurrentAccount.Sounds) return;

        var (path, volume) = SoundConfigurations[soundType];
        var soundPlayer = new MediaPlayer
        {
            Volume = volume
        };

        soundPlayer.Open(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)));
        soundPlayer.Play();
    }
}