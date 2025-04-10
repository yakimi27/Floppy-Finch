using System.Windows;
using System.Windows.Media.Imaging;
using Test;

namespace FloppyFinchGameModes;

public partial class GameOverWindow : Window
{
    public GameOverWindow(int score, BitmapSource gameImage)
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        if (Class1.Maximized)
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            Application.Current.MainWindow.Width = Class1.WindowWidth;
            Application.Current.MainWindow.Height = Class1.WindowHeight;
        }

        LoseScreenScoreTextBlock.Text = $"You scored: {score}";
        GameImage.Source = gameImage;
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        var classicGameplayWindow = new ClassicGameplayMode();
        classicGameplayWindow.Show();
        Close();
    }


    private void MainMenuButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}