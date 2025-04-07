using System.Windows;

namespace FloppyFinchMenus;

public partial class GameModesMenuWindow : Window
{
    public GameModesMenuWindow()
    {
        InitializeComponent();
    }

    private void ButtonClassicMode_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonRaceMode_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonExtendedMode_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonMainMenu_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}