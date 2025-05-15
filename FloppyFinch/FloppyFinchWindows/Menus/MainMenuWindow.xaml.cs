using System.Windows;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.Menus;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.Menus;

public partial class MainMenuWindow : Window
{
    public MainMenuWindow()
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
    }

    private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var gameModesMenuWindow = new GameModesMenuWindow();
        gameModesMenuWindow.Show();
        Close();
    }

    private void ButtonShop_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var shopMenuWindow = new ShopMenuWindow();
        shopMenuWindow.Show();
        Close();
    }

    private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var settingsMenuWindow = new SettingsMenuWindow();
        settingsMenuWindow.Show();
        Close();
    }

    private void ButtonLeaderboards_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
        Close();
    }

    private void MainMenuWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!AccountManager.IsGuest)
        {
            if (AccountManager.CurrentAccount.MaximizedWindow)
            {
                Left = AccountManager.CurrentAccount.WindowPositionX;
                Top = AccountManager.CurrentAccount.WindowPositionY;
                WindowState = WindowState.Maximized;
                return;
            }

            Width = AccountManager.CurrentAccount.WindowWidth;
            Height = AccountManager.CurrentAccount.WindowHeight;
            Left = AccountManager.CurrentAccount.WindowPositionX;
            Top = AccountManager.CurrentAccount.WindowPositionY;
        }
        else return;
    }

    private void SaveWindowStateToAccount()
    {
        AccountManager.CurrentAccount.MaximizedWindow = WindowStateData.Maximized;
        AccountManager.CurrentAccount.WindowWidth = (int)WindowStateData.WindowWidth;
        AccountManager.CurrentAccount.WindowHeight = (int)WindowStateData.WindowHeight;
        AccountManager.CurrentAccount.WindowPositionX = (int)WindowStateData.WindowPositionX;
        AccountManager.CurrentAccount.WindowPositionY = (int)WindowStateData.WindowPositionY;
    }
}