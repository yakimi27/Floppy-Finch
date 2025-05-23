using System.Windows;
using System.Windows.Shapes;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.Menus;

public partial class MainMenuWindow : Window
{
    private readonly Dictionary<Ellipse, (double OriginalWidth, double OriginalHeight)> _originalCloudSizes = new();

    public MainMenuWindow()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        foreach (var child in CloudCanvas.Children)
            if (child is Ellipse cloud)
                _originalCloudSizes[cloud] = (cloud.Width, cloud.Height);

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
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var leaderboardMenuWindow = new LeaderboardMenuWindow();
        leaderboardMenuWindow.Show();
        Close();
    }

    private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
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

    private void MainMenuWindow_OnClosing(object? sender, EventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
    }

    private void CloudCanvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (CloudCanvas == null) return;

        var width = CloudCanvas.ActualWidth;
        var height = CloudCanvas.ActualHeight;

        BackCloud1Transform.X = width * 0.1;
        BackCloud1Transform.Y = height * 0.1;

        BackCloud2Transform.X = width * 0.7;
        BackCloud2Transform.Y = height * 0.15;

        MiddleCloud1Transform.X = width * 0.2;
        MiddleCloud1Transform.Y = height * 0.3;
        MiddleCloud2Transform.X = width * 0.6;
        MiddleCloud2Transform.Y = height * 0.45;

        FrontCloud1Transform.X = width * 0.15;
        FrontCloud1Transform.Y = height * 0.7;

        FrontCloud2Transform.X = width * 0.55;
        FrontCloud2Transform.Y = height * 0.75;

        var scale = Math.Min(width / 400, height / 200);
        scale = Math.Max(0.5, Math.Min(scale, 1.5));

        foreach (var child in CloudCanvas.Children)
            if (child is Ellipse cloud && _originalCloudSizes.ContainsKey(cloud))
            {
                var (originalWidth, originalHeight) = _originalCloudSizes[cloud];
                cloud.Width = originalWidth * scale;
                cloud.Height = originalHeight * scale;
            }
    }
}