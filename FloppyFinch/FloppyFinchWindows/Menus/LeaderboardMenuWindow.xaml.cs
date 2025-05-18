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
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }
}