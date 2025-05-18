using System.Windows;
using System.Windows.Controls;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.Authentication;

public partial class SignInWindow : Window
{
    private bool _isPasswordVisible;

    public SignInWindow()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        if (WindowStateData.IsFirstStartup())
        {
            Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            Application.Current.MainWindow.Left = WindowStateData.WindowPositionX;
            Application.Current.MainWindow.Top = WindowStateData.WindowPositionY;
        }
    }

    private void ButtonSignIn_OnClick(object sender, RoutedEventArgs e)
    {
        var username = TextBoxUsername.Text.Trim();
        var password = _isPasswordVisible ? TextBoxPassword.Text : PasswordBoxPassword.Password;

        var account = AccountManager.Login(username, password);
        if (account != null)
        {
            if (CheckBoxRememberMe.IsChecked == true)
                SessionManager.SaveSession(new SessionData
                {
                    LastUsername = account.Username,
                    RememberMe = true
                });
            SaveWindowProperties();
            AccountManager.Login(account);
            AccountManager.SaveAccount(account);
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
            Close();
        }
        else
        {
            ButtonSignIn.IsEnabled = false;
            SessionManager.ClearSession();
            ShowError("Wrong username or password.");
        }
    }


    private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
    {
        SaveWindowProperties();
        var signUpWindow = new SignUpWindow();
        signUpWindow.Show();
        Close();
    }

    private void ButtonPlayAsGuest_OnClick(object sender, RoutedEventArgs e)
    {
        AccountManager.LoginAsGuest();
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void TextBoxUsername_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void TextBoxPassword_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void PasswordBoxPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        OnTextChanged(sender, e);
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

    private void ShowError(string message)
    {
        LabelSignInHelper.Visibility = Visibility.Visible;
        LabelSignInHelper.Content = message;
    }

    private void OnTextChanged(object sender, RoutedEventArgs e)
    {
        LabelSignInHelper.Visibility = Visibility.Collapsed;
        ButtonSignIn.IsEnabled = (!string.IsNullOrWhiteSpace(TextBoxUsername.Text)
                                  && !string.IsNullOrWhiteSpace(TextBoxPassword.Text)) ||
                                 (!string.IsNullOrWhiteSpace(TextBoxUsername.Text) &&
                                  !string.IsNullOrWhiteSpace(PasswordBoxPassword.Password));
    }


    private void CheckBoxShowPassword_OnChecked(object sender, RoutedEventArgs e)
    {
        _isPasswordVisible = true;
        TextBoxPassword.Text = PasswordBoxPassword.Password;
        TextBoxPassword.Visibility = Visibility.Visible;
        PasswordBoxPassword.Visibility = Visibility.Collapsed;
    }

    private void CheckBoxShowPassword_OnUnchecked(object sender, RoutedEventArgs e)
    {
        _isPasswordVisible = false;
        PasswordBoxPassword.Password = TextBoxPassword.Text;
        TextBoxPassword.Visibility = Visibility.Collapsed;
        PasswordBoxPassword.Visibility = Visibility.Visible;
    }

    private void SignInWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        var session = SessionManager.LoadSession();
        if (session.RememberMe)
        {
            TextBoxUsername.Text = session.LastUsername;
            CheckBoxRememberMe.IsChecked = true;
        }
    }
}