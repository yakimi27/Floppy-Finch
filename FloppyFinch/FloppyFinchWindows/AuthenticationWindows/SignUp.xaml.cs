using System.Windows;
using FloppyFinchGameModes.MenuWindows;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchGameModes.AuthenticationWindows;

public partial class SignUp : Window
{
    public SignUp()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        Application.Current.MainWindow.Left = WindowStateData.WindowPositionX;
        Application.Current.MainWindow.Top = WindowStateData.WindowPositionY;
    }

    private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var mainMenuWindow = new MainMenu();
        mainMenuWindow.Show();
        Close();
    }

    private void ButtonSgnIn_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var signInWindow = new SignIn();
        signInWindow.Show();
        Close();
    }
}