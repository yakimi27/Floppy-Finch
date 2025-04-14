using System.Windows;
using Test;

namespace FloppyFinchMenus;

public partial class MainMenuWindow : Window
{
    public MainMenuWindow()
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

    private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow = this;
        Class1.Maximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        Class1.WindowWidth = Application.Current.MainWindow.Width;
        Class1.WindowHeight = Application.Current.MainWindow.Height;
        var testPlay = new GameModesMenuWindow();
        testPlay.Show();
        Close();
    }

    private void ButtonShop_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow = this;
        Class1.Maximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        Class1.WindowWidth = Application.Current.MainWindow.Width;
        Class1.WindowHeight = Application.Current.MainWindow.Height;
        var testShop = new ShopMenuWindow();
        testShop.Show();
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