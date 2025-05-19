using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.TextureManagement;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.Menus;

public partial class ShopMenuWindow : Window
{
    public ShopMenuWindow()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        if (WindowStateData.Maximized)
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            Application.Current.MainWindow.Width = WindowStateData.WindowWidth;
            Application.Current.MainWindow.Height = WindowStateData.WindowHeight;
            Application.Current.MainWindow.Left = WindowStateData.WindowPositionX;
            Application.Current.MainWindow.Top = WindowStateData.WindowPositionY;
        }

        /*CoinIcon.Source = new BitmapImage(new Uri("pack://application:,,,/FloppyFinchWindows;component/Assets/UI/coin.png"));*/
        CoinsText.Text = AccountManager.CurrentAccount!.Coins.ToString();

        LoadSkins();
    }

    private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }


    private static void SaveWindowStateToAccount()
    {
        if (AccountManager.CurrentAccount == null) return;
        AccountManager.CurrentAccount!.MaximizedWindow = WindowStateData.Maximized;
        AccountManager.CurrentAccount.WindowWidth = (int)WindowStateData.WindowWidth;
        AccountManager.CurrentAccount.WindowHeight = (int)WindowStateData.WindowHeight;
        AccountManager.CurrentAccount.WindowPositionX = (int)WindowStateData.WindowPositionX;
        AccountManager.CurrentAccount.WindowPositionY = (int)WindowStateData.WindowPositionY;
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }

    private void ShopMenuWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
    }

    private void LoadSkins()
    {
        var currentSkin = AccountManager.CurrentAccount!.SelectedSkin;
        var skins = new List<SkinItem>
        {
            new()
            {
                Name = "Classic",
                Price = 0,
                IsOwned = true,
                Image = SkinManager.LoadClassicSkin().Frames[0],
                IsSelected = currentSkin != "Classic"
            },
            new()
            {
                Name = "Blue",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Blue"),
                Image = SkinManager.LoadBlueSkin().Frames[0],
                IsSelected = currentSkin != "Blue"
            },
            new()
            {
                Name = "Black",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Black"),
                Image = SkinManager.LoadBlackSkin().Frames[0],
                IsSelected = currentSkin != "Black"
            },
            new()
            {
                Name = "Green",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Green"),
                Image = SkinManager.LoadGreenSkin().Frames[0],
                IsSelected = currentSkin != "Green"
            },
            new()
            {
                Name = "Pink",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Pink"),
                Image = SkinManager.LoadPinkSkin().Frames[0],
                IsSelected = currentSkin != "Pink"
            },
            new()
            {
                Name = "White",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("White"),
                Image = SkinManager.LoadWhiteSkin().Frames[0],
                IsSelected = currentSkin != "White"
            }
        };

        SkinsItemsControl.ItemsSource = skins;
    }


    private void BuySkin(SkinItem skin)
    {
        if (AccountManager.CurrentAccount!.Coins >= skin.Price && !skin.IsOwned)
        {
            AccountManager.CurrentAccount.Coins -= skin.Price;
            AccountManager.CurrentAccount.UnlockedSkins.Add(skin.Name);
            AccountManager.SaveAccount(AccountManager.CurrentAccount);
            LoadSkins();
        }
    }

    private void SelectSkin(SkinItem skin)
    {
        if (skin.IsOwned)
        {
            AccountManager.CurrentAccount!.SelectedSkin = skin.Name;
            AccountManager.SaveAccount(AccountManager.CurrentAccount);
        }
    }

    private void SkinButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var skinItem = (SkinItem)button.DataContext;

        if (skinItem.IsOwned)
            SelectSkin(skinItem);
        else
            BuySkin(skinItem);

        CoinsText.Text = AccountManager.CurrentAccount!.Coins.ToString();
        LoadSkins();
    }
}