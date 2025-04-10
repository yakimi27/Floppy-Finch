using System.Windows;
using FloppyFinchGameModes;
using Test;

namespace FloppyFinchMenus;

public partial class GameModesMenuWindow : Window
{
    public GameModesMenuWindow()
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
    }

    private void ButtonClassicMode_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow = this;
        Class1.Maximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        Class1.WindowWidth = Application.Current.MainWindow.ActualWidth;
        Class1.WindowHeight = Application.Current.MainWindow.ActualHeight;

        var testPlayClassic = new ClassicGameplayMode();
        testPlayClassic.Show();
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

    private void ButtonBackMainMenu_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow = this;
        Class1.Maximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        Class1.WindowWidth = Application.Current.MainWindow.Width;
        Class1.WindowHeight = Application.Current.MainWindow.Height;

        var testReturnBack = new MainMenuWindow();
        testReturnBack.Show();
        Close();
    }
}