﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.Authentication;
using FloppyFinchWindows.Resources;

namespace FloppyFinchWindows.Menus;

public partial class SettingsMenuWindow : Window
{
    private readonly Dictionary<Ellipse, (double OriginalWidth, double OriginalHeight)> _originalCloudSizes = new();
    private bool isInitializingResolutionComboBox;
    private bool isInitializingSubComboBoxes;

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
        SetUpSubComboBoxes();
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
        var result = MessageBox.Show($"{Strings.AccountActionConfirmationMessageBox}",
            $"{Strings.ClearAccountDataTitleMessageBox}", MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
            if (AccountManager.CurrentAccount != null)
            {
                AccountManager.ClearAccountData(AccountManager.CurrentAccount);
                AccountManager.SaveAccount(AccountManager.CurrentAccount);
                MessageBox.Show($"{Strings.AccountDataClearedMessageBox}", $"{Strings.SuccessTitleMessageBox}",
                    MessageBoxButton.OK);
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
        var result = MessageBox.Show($"{Strings.AccountActionConfirmationMessageBox}",
            $"{Strings.DeleteAccountTitleMessageBox}", MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
            if (AccountManager.CurrentAccount != null)
            {
                AccountManager.DeleteAccount(AccountManager.CurrentAccount);
                MessageBox.Show($"{Strings.AccountDeletedSuccessMessageBox}", $"{Strings.SuccessTitleMessageBox}",
                    MessageBoxButton.OK);
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

    private void ComboBoxLanguage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isInitializingSubComboBoxes) return;
        if (AccountManager.CurrentAccount == null)
            return;


        if (ComboBoxLanguage.SelectedItem.Equals(ComboBoxItemLanguageUkrainian))
        {
            App.ChangeLanguage("uk");

            AccountManager.CurrentAccount.SelectedLanguage = "uk";
            WindowStateData.SaveWindowState(Application.Current.MainWindow);
            AccountManager.SaveAccount(AccountManager.CurrentAccount);

            var updateUkrainianWindow = new SettingsMenuWindow();
            updateUkrainianWindow.Show();
            Close();

            return;
        }

        App.ChangeLanguage("en");

        AccountManager.CurrentAccount.SelectedLanguage = "en";
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        AccountManager.SaveAccount(AccountManager.CurrentAccount);

        var updateEnglishWindow = new SettingsMenuWindow();
        updateEnglishWindow.Show();
        Close();
    }

    private void ComboBoxBackground_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (isInitializingSubComboBoxes) return;
        if (AccountManager.CurrentAccount == null)
            return;

        AccountManager.CurrentAccount.SelectedBackground =
            ComboBoxBackground.SelectedItem.Equals(ComboBoxItemBackgroundOff) ? "null" :
            ComboBoxBackground.SelectedItem.Equals(ComboBoxItemBackgroundLight) ? "Light" : "Dark";
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
    }

    private void SetUpSubComboBoxes()
    {
        isInitializingSubComboBoxes = true;
        if (AccountManager.CurrentAccount != null)
            ComboBoxLanguage.SelectedItem = AccountManager.CurrentAccount.SelectedLanguage == "uk"
                ? ComboBoxItemLanguageUkrainian
                : ComboBoxItemLanguageEnglish;
        if (AccountManager.CurrentAccount != null)
            ComboBoxBackground.SelectedItem = AccountManager.CurrentAccount.SelectedBackground == "null"
                ? ComboBoxItemBackgroundOff
                : AccountManager.CurrentAccount.SelectedBackground == "Light"
                    ? ComboBoxItemBackgroundLight
                    : ComboBoxItemBackgroundDark;
        isInitializingSubComboBoxes = false;
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