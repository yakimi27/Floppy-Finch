using System.Windows;
using FloppyFinchGameModes.GameWindows;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.MenuWindows;

public partial class GameModesMenu : Window
{
    public GameModesMenu()
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
    }

    private void ButtonClassicMode_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var classicGameplayWindow = new ClassicGameplayMode();
        classicGameplayWindow.Show();
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

    private void ButtonBackToMainMenu_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var mainMenuWindow = new MainMenu();
        mainMenuWindow.Show();
        Close();
    }
}