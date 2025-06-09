using System.Windows;
using System.Windows.Media;
using FloppyFinchLogics.AccountManagement;

namespace FloppyFinchLogics.TextureManagement;

public class SkinItem
{
    public ImageSource Image { get; set; }
    public string Name { get; set; }
    public string DisplayName { set; get; }
    public int Price { get; set; }
    public bool IsOwned { get; set; }
    public bool IsSelected { get; set; }

    public string ButtonLabel
    {
        get
        {
            if (IsSelected && IsOwned)
                return "Select";
            if (!IsSelected && IsOwned)
                return "Selected";
            return $"Buy ({Price} coins)";
        }
    }

    public Visibility NotEnoughCoins
    {
        get
        {
            if (IsOwned || (!IsOwned && Price <= AccountManager.CurrentAccount.Coins))
                return Visibility.Collapsed;
            return Visibility.Visible;
        }
    }

    public bool ButtonEnabled => NotEnoughCoins == Visibility.Collapsed && IsSelected;
}