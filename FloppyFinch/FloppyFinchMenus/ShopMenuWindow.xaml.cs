using System.Windows;
using Test;

namespace FloppyFinchMenus;

public partial class ShopMenuWindow : Window
{
    public ShopMenuWindow()
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

    private void ButtonShopBack_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow = this;
        Class1.Maximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        Class1.WindowWidth = Application.Current.MainWindow.Width;
        Class1.WindowHeight = Application.Current.MainWindow.Height;
        var testMainMenu = new MainMenuWindow();
        testMainMenu.Show();
        Close();
    }
}