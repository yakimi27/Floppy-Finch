using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.GameWindows.TargetScoreWindows;

public partial class TargetScoreModeGameOverWindow : Window
{
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

        if (score == targetScoreValue)
            GameOverLabel.Content = "Congratulations!";
        else
            GameOverLabel.Content = "Game over";
        LoseScreenScoreTextBlock.Text = $"You scored: {score}";
        GameImage.Source = gameImage;
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
}