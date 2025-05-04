using System.Windows;
using System.Windows.Media.Imaging;
using FloppyFinchGameModes.MenuWindows;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchGameModes.GameWindows.ExtendedWindows;

public partial class ExtendedGameOverWindow : Window
{
    public ExtendedGameOverWindow(int score, BitmapSource gameImage)
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
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var extendedGameplayWindow = new ExtendedGameplayMode();
        extendedGameplayWindow.Show();
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