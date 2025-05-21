using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.GameLogics.ExtendedMode;
using FloppyFinchLogics.TextureManagement;
using FloppyFinchLogics.TextureManagement.Coins;
using FloppyFinchLogics.TextureManagement.PowerUps;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;
using FloppyFinchWindows.Resources;

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

        CoinIcon.Source = CoinManager.LoadCoin();
        CoinsText.Text = AccountManager.CurrentAccount!.Coins.ToString();

        LoadSkins();
        LoadPowerUps();
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
                DisplayName = $"{Strings.ClassicSkinLabelText}",
                Name = "Classic",
                Price = 0,
                IsOwned = true,
                Image = SkinManager.LoadClassicSkin().Frames[2],
                IsSelected = currentSkin != "Classic"
            },
            new()
            {
                DisplayName = $"{Strings.BlueSkinLabelText}",
                Name = "Blue",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Blue"),
                Image = SkinManager.LoadBlueSkin().Frames[2],
                IsSelected = currentSkin != "Blue"
            },
            new()
            {
                DisplayName = $"{Strings.BlackSkinLabelText}",
                Name = "Black",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Black"),
                Image = SkinManager.LoadBlackSkin().Frames[2],
                IsSelected = currentSkin != "Black"
            },
            new()
            {
                DisplayName = $"{Strings.GreenSkinLabelText}",
                Name = "Green",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Green"),
                Image = SkinManager.LoadGreenSkin().Frames[2],
                IsSelected = currentSkin != "Green"
            },
            new()
            {
                DisplayName = $"{Strings.PinkSkinLabelText}",
                Name = "Pink",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("Pink"),
                Image = SkinManager.LoadPinkSkin().Frames[2],
                IsSelected = currentSkin != "Pink"
            },
            new()
            {
                DisplayName = $"{Strings.WhiteSkinLabelText}",
                Name = "White",
                Price = 200,
                IsOwned = AccountManager.CurrentAccount.UnlockedSkins.Contains("White"),
                Image = SkinManager.LoadWhiteSkin().Frames[2],
                IsSelected = currentSkin != "White"
            }
        };

        SkinsItemsControl.ItemsSource = skins;
    }

    private void SkinButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var skinItem = (SkinItem)button.DataContext;

        if (skinItem.IsOwned)
            SelectSkin(skinItem);
        else
            BuySkin(skinItem);

        UpdateCoinsDisplay();
        LoadSkins();
        LoadPowerUps();
    }

    private void BuySkin(SkinItem skin)
    {
        if (AccountManager.CurrentAccount!.Coins >= skin.Price && !skin.IsOwned)
        {
            AccountManager.CurrentAccount.Coins -= skin.Price;
            AccountManager.CurrentAccount.UnlockedSkins.Add(skin.Name);
            AccountManager.SaveAccount(AccountManager.CurrentAccount);
            LoadSkins();
            LoadPowerUps();
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

    private void LoadPowerUps()
    {
        var powerUps = new List<PowerUpItem>
        {
            new()
            {
                Id = 0,
                DisplayName = $"{Strings.JetpackPowerUpLabelText}",
                Name = "Jetpack",
                Price = CalculatePowerUpPrice(175, AccountManager.CurrentAccount.PowerUpLevels[0]),
                Level = AccountManager.CurrentAccount.PowerUpLevels[0],
                Image = PowerUpManager.LoadPowerUp(PowerUp.PowerUpType.Jetpack)
            },
            new()
            {
                Id = 1,
                DisplayName = $"{Strings.ScoreMultiplierPowerUpLabelText}",
                Name = "Score Multiplier",
                Price = CalculatePowerUpPrice(150, AccountManager.CurrentAccount.PowerUpLevels[1]),
                Level = AccountManager.CurrentAccount.PowerUpLevels[1],
                Image = PowerUpManager.LoadPowerUp(PowerUp.PowerUpType.ScoreMultiplier)
            },
            new()
            {
                Id = 2,
                DisplayName = $"{Strings.ShieldPowerUpLabelText}",
                Name = "Shield",
                Price = CalculatePowerUpPrice(100, AccountManager.CurrentAccount.PowerUpLevels[2]),
                Level = AccountManager.CurrentAccount.PowerUpLevels[2],
                Image = PowerUpManager.LoadPowerUp(PowerUp.PowerUpType.Shield)
            }
        };

        PowerupsItemsControl.ItemsSource = powerUps;
    }

    private int CalculatePowerUpPrice(int basePrice, int currentLevel)
    {
        return basePrice * (1 + currentLevel / 2);
    }

    private void PowerUpButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var powerUpItem = button?.DataContext as PowerUpItem;

        if (powerUpItem == null || powerUpItem.IsMaxed ||
            AccountManager.CurrentAccount!.Coins < powerUpItem.Price)
            return;

        AccountManager.CurrentAccount.Coins -= powerUpItem.Price;

        AccountManager.CurrentAccount.PowerUpLevels[powerUpItem.Id]++;

        UpdateCoinsDisplay();
        LoadSkins();
        LoadPowerUps();
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }

    private void UpdateCoinsDisplay()
    {
        CoinsText.Text = AccountManager.CurrentAccount!.Coins.ToString();
    }
}