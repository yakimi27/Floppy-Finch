using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.Menus;

namespace FloppyFinchWindows.Authentication;

public partial class SignUpWindow : Window
{
    private readonly Dictionary<Ellipse, (double OriginalWidth, double OriginalHeight)> _originalCloudSizes = new();
    private readonly Regex _passwordCheckout = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d!@#$%^&*()_+]+$");
    private readonly Regex _usernameCheckout = new(@"^[A-Za-z0-9][A-Za-z0-9-_]+$");
    private bool _isPasswordVisible;
    private Match _match;

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
            ShowError("Username too short.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        if (username.Length > 16)
        {
            ShowError("Username too long.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        _match = _usernameCheckout.Match(username);

        if (!_match.Success)
        {
            ShowError("Invalid characters in username.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        if (password.Length < 4)
        {
            ShowError("Password too short.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        if (password.Length > 24)
        {
            ShowError("Password too long.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        _match = _passwordCheckout.Match(password);

        if (!_match.Success)
        {
            ShowError("Password doesn't meet requirements.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        if (password != confirm)
        {
            ShowError("Passwords mismatch.");
            ButtonSignUp.IsEnabled = false;
            return;
        }

        var success = AccountManager.Register(username, password);
        if (success)
        {
            if (CheckBoxRememberMe.IsChecked == true)
                SessionManager.SaveSession(new SessionData
                {
                    LastUsername = username,
                    RememberMe = true
                });

            SaveWindowProperties();
            WindowStateData.SaveWindowState(Application.Current.MainWindow);
            AccountManager.SaveAccount(AccountManager.CurrentAccount);
            MessageBox.Show("Account successfully created!", "Success!", MessageBoxButton.OK);
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

    private void ButtonPlayAsGuest_OnClick(object sender, RoutedEventArgs e)
    {
        AccountManager.LoginAsGuest();
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
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

    private void SaveWindowProperties()
    {
        var mainWindow = Application.Current.MainWindow;
        WindowStateData.Maximized = mainWindow.WindowState == WindowState.Maximized;
        WindowStateData.WindowWidth = mainWindow.Width;
        WindowStateData.WindowHeight = mainWindow.Height;
        WindowStateData.WindowPositionX = mainWindow.Left;
        WindowStateData.WindowPositionY = mainWindow.Top;
    }

    private void CloudCanvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (CloudCanvas == null) return;

        var width = CloudCanvas.ActualWidth;
        var height = CloudCanvas.ActualHeight;

        BackCloud1Transform.X = width * 0.1;
        BackCloud1Transform.Y = height * 0.1;

        BackCloud2Transform.X = width * 0.7;
        BackCloud2Transform.Y = height * 0.15;

        MiddleCloud1Transform.X = width * 0.2;
        MiddleCloud1Transform.Y = height * 0.3;
        MiddleCloud2Transform.X = width * 0.6;
        MiddleCloud2Transform.Y = height * 0.45;

        FrontCloud1Transform.X = width * 0.15;
        FrontCloud1Transform.Y = height * 0.7;

        FrontCloud2Transform.X = width * 0.55;
        FrontCloud2Transform.Y = height * 0.75;

        var scale = Math.Min(width / 400, height / 200);
        scale = Math.Max(0.5, Math.Min(scale, 1.5));

        foreach (var child in CloudCanvas.Children)
            if (child is Ellipse cloud && _originalCloudSizes.ContainsKey(cloud))
            {
                var (originalWidth, originalHeight) = _originalCloudSizes[cloud];
                cloud.Width = originalWidth * scale;
                cloud.Height = originalHeight * scale;
            }
    }
}