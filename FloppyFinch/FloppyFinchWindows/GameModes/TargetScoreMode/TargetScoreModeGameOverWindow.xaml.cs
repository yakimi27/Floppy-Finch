using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.GameModes.TargetScoreMode;

public partial class TargetScoreModeGameOverWindow : Window
{
    private readonly bool _isTargetAchieved;

    public TargetScoreModeGameOverWindow(int score, BitmapSource gameImage, int targetScoreValue)
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

        _isTargetAchieved = score == targetScoreValue;

        if (_isTargetAchieved)
            GameOverLabel.Content = "Congratulations!";
        else
            GameOverLabel.Content = "Game over";
        LoseScreenScoreTextBlock.Text = $"You scored: {score}";

        GameImage.Source = gameImage;
        UpdateAccountInfo(score, _isTargetAchieved);
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var targetScoreGameplayWindow = new TargetScoreModeGameplayWindow();
        targetScoreGameplayWindow.Show();
        Close();
    }


    private void MainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateAccountInfo(int score, bool isTargetAchieved)
    {
        if (isTargetAchieved) score += 25;
        AccountManager.CurrentAccount!.Coins += score;
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }
}