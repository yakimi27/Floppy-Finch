using System.Windows;
using System.Windows.Controls;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchGameModes.AuthenticationWindows;

public partial class SignUpWindow : Window
{
    private bool _isPasswordVisible;

    public SignUpWindow()
    {
        InitializeComponent();
        Application.Current.MainWindow = this;
        Application.Current.MainWindow.Left = WindowStateData.WindowPositionX;
        Application.Current.MainWindow.Top = WindowStateData.WindowPositionY;
    }

    private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
    {
        var username = TextBoxUsername.Text.Trim();
        var password = _isPasswordVisible ? TextBoxPassword.Text : PasswordBoxPassword.Password;
        var confirm = _isPasswordVisible ? TextBoxConfirmPassword.Text : PasswordBoxConfirmPassword.Password;

        if (username.Length < 3)
        {
            ShowError("Username too short!");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        if (password.Length < 4)
        {
            ShowError("Password too short!");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        if (password != confirm)
        {
            ShowError("Passwords mismatch!");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        var success = AccountManager.Register(username, password);
        if (success)
        {
            MessageBox.Show("Account successfully created!", "Success!", MessageBoxButton.OK);
            WindowStateData.SaveWindowState(Application.Current.MainWindow);
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.Show();
            Close();
        }
        else
        {
            ShowError("Username already taken!");
        }
    }

    private void ButtonSignIn_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var signInWindow = new SignInWindow();
        signInWindow.Show();
        Close();
    }

    private void TextBoxPassword_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void TextBoxUsername_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void TextBoxConfirmPassword_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void PasswordBoxPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void PasswordBoxConfirmPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        OnTextChanged(sender, e);
    }

    private void OnTextChanged(object sender, RoutedEventArgs e)
    {
        LabelSignUpHelper.Visibility = Visibility.Collapsed;
        ButtonSignUp.IsEnabled = (!string.IsNullOrWhiteSpace(TextBoxUsername.Text)
                                  && !string.IsNullOrWhiteSpace(TextBoxPassword.Text)
                                  && !string.IsNullOrWhiteSpace(TextBoxConfirmPassword.Text)) ||
                                 (!string.IsNullOrWhiteSpace(TextBoxUsername.Text)
                                  && !string.IsNullOrWhiteSpace(PasswordBoxPassword.Password)
                                  && !string.IsNullOrWhiteSpace(PasswordBoxConfirmPassword.Password));
    }

    private void ShowError(string message)
    {
        LabelSignUpHelper.Visibility = Visibility.Visible;
        LabelSignUpHelper.Content = message;
    }

    private void CheckBoxShowPassword_OnChecked(object sender, RoutedEventArgs e)
    {
        _isPasswordVisible = true;
        TextBoxPassword.Text = PasswordBoxPassword.Password;
        TextBoxConfirmPassword.Text = PasswordBoxConfirmPassword.Password;
        TextBoxPassword.Visibility = Visibility.Visible;
        TextBoxConfirmPassword.Visibility = Visibility.Visible;
        PasswordBoxPassword.Visibility = Visibility.Collapsed;
        PasswordBoxConfirmPassword.Visibility = Visibility.Collapsed;
    }

    private void CheckBoxShowPassword_OnUnchecked(object sender, RoutedEventArgs e)
    {
        _isPasswordVisible = false;
        PasswordBoxPassword.Password = TextBoxPassword.Text;
        PasswordBoxConfirmPassword.Password = TextBoxConfirmPassword.Text;


        TextBoxPassword.Visibility = Visibility.Collapsed;
        TextBoxConfirmPassword.Visibility = Visibility.Collapsed;
        PasswordBoxPassword.Visibility = Visibility.Visible;
        PasswordBoxConfirmPassword.Visibility = Visibility.Visible;
    }
}