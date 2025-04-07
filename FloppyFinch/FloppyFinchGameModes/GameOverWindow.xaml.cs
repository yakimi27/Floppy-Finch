using System.Windows;
using System.Windows.Media.Imaging;

namespace FloppyFinchGameModes;

public partial class GameOverWindow : Window
{
    public GameOverWindow(int score, BitmapSource gameImage)
    {
        InitializeComponent();
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