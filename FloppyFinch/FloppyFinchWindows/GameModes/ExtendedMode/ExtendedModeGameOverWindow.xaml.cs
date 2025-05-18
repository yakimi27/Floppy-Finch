using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.GameModes.ExtendedMode;

namespace FloppyFinchGameModes.GameModes.ExtendedMode;

public partial class ExtendedModeGameOverWindow : Window
{
    public ExtendedModeGameOverWindow(int score, BitmapSource gameImage)
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

        LoseScreenScoreTextBlock.Text = $"You scored: {score}";
        GameImage.Source = gameImage;
        UpdateAccountInfo(score);
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var extendedGameplayWindow = new ExtendedModeModeGameplayWindow();
        extendedGameplayWindow.Show();
        Close();
    }


    private void MainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow!);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateAccountInfo(int score)
    {
        AccountManager.CurrentAccount!.Coins += score;
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }
}