using System.Windows;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchGameModes.Menus;

public partial class LeaderboardMenuWindow : Window
{
    public LeaderboardMenuWindow()
    {
        InitializeComponent();
        AccountManager.LoadAllAccounts();
        var leaderboardData = new LeaderboardManager();
        DataContext = leaderboardData;
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

    private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
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