using System.Windows;

namespace FloppyFinchMenus;

public partial class MainMenuWindow : Window
{
    public MainMenuWindow()
    {
        InitializeComponent();
    }

    private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
    {
        var test = new GameModesMenuWindow();
        test.Show();
        Close();
    }

    private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonLeaderboards_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}