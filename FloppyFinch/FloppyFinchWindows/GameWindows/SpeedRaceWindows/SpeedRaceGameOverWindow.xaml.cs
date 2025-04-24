using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.MenuWindows;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchGameModes.GameWindows.SpeedRaceWindows;

public partial class SpeedRaceGameOverWindow : Window
{
    private readonly int _gameSpeed;

    public SpeedRaceGameOverWindow(int score, BitmapSource gameImage, int gameSpeed)
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
        _gameSpeed = gameSpeed;
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var speedRaceGameplayWindow = new SpeedRaceGameplayMode(_gameSpeed);
        speedRaceGameplayWindow.Show();
        Close();
    }


    private void MainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow!);
        var mainMenuWindow = new MainMenu();
        mainMenuWindow.Show();
        Close();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}