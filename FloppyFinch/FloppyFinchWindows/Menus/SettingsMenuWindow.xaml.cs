using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.Authentication;

namespace FloppyFinchWindows.Menus;

public partial class SettingsMenuWindow : Window
{
    private bool isInitializingResolutionComboBox;

    public SettingsMenuWindow()
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

        SetUpResolutionComboBox();
    }

    private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void ButtonSignOut_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        SessionManager.ClearSession();
        AccountManager.CurrentAccount = null;
        new SignInWindow().Show();
        Close();
    }

    private static void SaveWindowStateToAccount()
    {
        if (AccountManager.CurrentAccount == null) return;
        AccountManager.CurrentAccount!.MaximizedWindow = WindowStateData.Maximized;
        AccountManager.CurrentAccount.WindowWidth = (int)WindowStateData.WindowWidth;
        AccountManager.CurrentAccount.WindowHeight = (int)WindowStateData.WindowHeight;
        AccountManager.CurrentAccount.WindowPositionX = (int)WindowStateData.WindowPositionX;
        AccountManager.CurrentAccount.WindowPositionY = (int)WindowStateData.WindowPositionY;
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }

    private void ButtonClearAccountData_OnClick(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure?", "Clear account data.", MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
            if (AccountManager.CurrentAccount != null)
            {
                AccountManager.ClearAccountData(AccountManager.CurrentAccount);
                AccountManager.SaveAccount(AccountManager.CurrentAccount);
                MessageBox.Show("Account's data successfully cleared.", "Success.", MessageBoxButton.OK);
                Application.Current.MainWindow.Width = AccountManager.CurrentAccount.WindowWidth;
                Application.Current.MainWindow.Height = AccountManager.CurrentAccount.WindowHeight;
                Application.Current.MainWindow.Left = AccountManager.CurrentAccount.WindowPositionX;
                Application.Current.MainWindow.Top = AccountManager.CurrentAccount.WindowPositionY;
            }
        }
        else if (result == MessageBoxResult.Cancel)
        {
        }
    }

    private void ButtonDeleteAccount_OnClick(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure?", "Delete account.", MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
            if (AccountManager.CurrentAccount != null)
            {
                AccountManager.DeleteAccount(AccountManager.CurrentAccount);
                MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButton.OK);
                SessionManager.ClearSession();
                new SignInWindow().Show();
                Close();
            }
        }
        else if (result == MessageBoxResult.Cancel)
        {
        }
    }

    private void SettingsMenuWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
    }

    private void ButtonResetScreenPosition_OnClick(object sender, RoutedEventArgs e)
    {
        var screenWidth = SystemParameters.PrimaryScreenWidth;
        var screenHeight = SystemParameters.PrimaryScreenHeight;
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var defaultCenterX = (int)((screenWidth - WindowStateData.WindowWidth) / 2);
        var defaultCenterY = (int)((screenHeight - WindowStateData.WindowHeight) / 2);
        Application.Current.MainWindow.Left = defaultCenterX >= 0 ? defaultCenterX : 0;
        Application.Current.MainWindow.Top = defaultCenterY >= 0 ? defaultCenterY : 0;
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
    }

    private void SetUpResolutionComboBox()
    {
        isInitializingResolutionComboBox = true;

        var currentResolution = $"{(int)WindowStateData.WindowWidth} x {(int)WindowStateData.WindowHeight}";

        ComboBoxResolution.Items.Add(currentResolution);
        ComboBoxResolution.Items.Add("1920 x 1200");
        ComboBoxResolution.Items.Add("1920 x 1080");
        ComboBoxResolution.Items.Add("1600 x 900");
        ComboBoxResolution.Items.Add("1366 x 768");
        ComboBoxResolution.Items.Add("1280 x 720");
        ComboBoxResolution.Items.Add("1024 x 768");
        ComboBoxResolution.Items.Add("800 x 600");
        ComboBoxResolution.Items.Add("400 x 600");

        ComboBoxResolution.SelectedItem = currentResolution;

        isInitializingResolutionComboBox = false;
    }

    private void ComboBoxResolution_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isInitializingResolutionComboBox)
            return;

        if (ComboBoxResolution.SelectedItem is string selectedResolution)
        {
            var parts = selectedResolution.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0].Trim(), out var width) &&
                int.TryParse(parts[1].Trim(), out var height))
            {
                Application.Current.MainWindow.Width = width;
                Application.Current.MainWindow.Height = height;

                WindowStateData.SaveWindowState(Application.Current.MainWindow);

                ButtonResetScreenPosition_OnClick(sender, e);
            }
        }
    }
}