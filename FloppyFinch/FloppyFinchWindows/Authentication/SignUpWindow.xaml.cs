using System.Windows;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchGameModes.AuthenticationWindows;

public partial class SignUpWindow : Window
{
    public SignUpWindow()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        Application.Current.MainWindow.Left = WindowStateData.WindowPositionX;
        Application.Current.MainWindow.Top = WindowStateData.WindowPositionY;
    }

    private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void ButtonSgnIn_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var signInWindow = new SignInWindow();
        signInWindow.Show();
        Close();
    }
}