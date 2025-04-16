using System.Windows;
using FloppyFinchGameModes.MenuWindows;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.AuthenticationWindows;

public partial class SignIn : Window
{
    public SignIn()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        if (WindowStateData.IsFirstStartup())
        {
            Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
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
    }

    private void ButtonSignIn_OnClick(object sender, RoutedEventArgs e)
    {
        SaveWindowProperties();
        var mainMenuWindow = new MainMenu();
        mainMenuWindow.Show();
        Close();
    }

    private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
    {
        SaveWindowProperties();
        var signUpWindow = new SignUp();
        signUpWindow.Show();
        Close();
    }

    private void SaveWindowProperties()
    {
        var mainWindow = Application.Current.MainWindow;
        WindowStateData.Maximized = mainWindow.WindowState == WindowState.Maximized;
        WindowStateData.WindowWidth = mainWindow.Width;
        WindowStateData.WindowHeight = mainWindow.Height;
        WindowStateData.WindowPositionX = mainWindow.Left;
        WindowStateData.WindowPositionY = mainWindow.Top;
    }
}