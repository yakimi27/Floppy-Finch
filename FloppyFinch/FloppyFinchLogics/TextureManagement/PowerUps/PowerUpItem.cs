using System.Windows;
using System.Windows.Media;
using FloppyFinchLogics.AccountManagement;

namespace FloppyFinchLogics.TextureManagement.PowerUps;

public class PowerUpItem
{
    private const int MaxPowerUpLevel = 12;
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public int Price { get; set; }
    public int Level { get; set; }
    public ImageSource Image { get; set; }

    public bool IsMaxed => MaxPowerUpLevel <= AccountManager.CurrentAccount.PowerUpLevels[Id];

    public string ButtonLabel => IsMaxed ? "Max level" : $"Upgrade ({Price} coins)";

    public Visibility NotEnoughCoins
    {
        get
        {
            if (IsMaxed) return Visibility.Collapsed;
            return AccountManager.CurrentAccount.Coins >= Price ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public bool CanUpgrade
    {
        get
        {
            if (IsMaxed) return false;
            return AccountManager.CurrentAccount.Coins >= Price;
        }
    }
}