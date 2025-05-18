using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.GameModes.SpeedRaceMode;

namespace FloppyFinchGameModes.GameModes.SpeedRaceMode;

public partial class SpeedRaceModeGameOverWindow : Window
{
    private readonly int _gameSpeed;

    public SpeedRaceModeGameOverWindow(int score, BitmapSource gameImage, int gameSpeed)
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
        UpdateAccountInfo(score, gameSpeed);
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var speedRaceGameplayWindow = new SpeedRaceModeGameplayWindow(_gameSpeed);
        speedRaceGameplayWindow.Show();
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

    private void UpdateAccountInfo(int score, int gameSpeed)
    {
        var multiplier = gameSpeed switch
        {
            13 => 3.0,
            var speedValue when speedValue > 9 => 2.0,
            var speedValue when speedValue > 6 => 1.25,
            var speedValue when speedValue > 3 => 0.75,
            var speedValue when speedValue <= 3 => 0.5,
            _ => 1.0
        };

        score = (int)(score * multiplier);
        AccountManager.CurrentAccount!.Coins += score;
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }
}