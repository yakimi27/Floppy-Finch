using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.GameModes.ClassicMode;

public partial class ClassicModeGameOverWindow : Window
{
    public ClassicModeGameOverWindow(int score, BitmapSource gameImage)
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

        GameImage.Source = gameImage;
        if (score > AccountManager.CurrentAccount!.HighScore)
        {
            LabelGameOver.Content = "Congratulations!";
            TextBlockScoredPoints.Text = $"New high score: {score}";
        }
        else
        {
            LabelGameOver.Content = "Game over";
            TextBlockScoredPoints.Text = $"You scored: {score}";
        }

        UpdateAccountInfo(score);
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var classicGameplayWindow = new ClassicModeGameplayWindow();
        classicGameplayWindow.Show();
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
        if (score > AccountManager.CurrentAccount.HighScore) AccountManager.CurrentAccount.HighScore = score;
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }
}