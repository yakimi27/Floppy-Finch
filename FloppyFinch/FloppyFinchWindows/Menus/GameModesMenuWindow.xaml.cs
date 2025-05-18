using System.Windows;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.GameModes.ExtendedMode;
using FloppyFinchWindows.GameModes.SpeedRaceMode;
using FloppyFinchWindows.GameModes.TargetScoreMode;
using ClassicModeGameplayWindow = FloppyFinchWindows.GameModes.ClassicMode.ClassicModeGameplayWindow;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.Menus;

public partial class GameModesMenuWindow : Window
{
    public GameModesMenuWindow()
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
    }

    private void ButtonClassicMode_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var classicGameplayWindow = new ClassicModeGameplayWindow();
        classicGameplayWindow.Show();
        Close();
    }

    private void ButtonTargetScoreMode_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var targetScoreGameplayWindow = new TargetScoreModeGameplayWindow();
        targetScoreGameplayWindow.Show();
        Close();
    }

    private void ButtonExtendedMode_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var extendedGameplayWindow = new ExtendedModeModeGameplayWindow();
        extendedGameplayWindow.Show();
        Close();
    }

    private void ButtonSpeedRaceMode_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        var speedRaceDifficultyAdjusterWindow = new SpeedRaceModeDifficultyWindow();
        speedRaceDifficultyAdjusterWindow.Show();
    }

    private void ButtonBackToMainMenu_OnClick(object sender, RoutedEventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        AccountManager.SaveAccount(AccountManager.CurrentAccount);
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void SaveWindowStateToAccount()
    {
        AccountManager.CurrentAccount.MaximizedWindow = WindowStateData.Maximized;
        AccountManager.CurrentAccount.WindowWidth = (int)WindowStateData.WindowWidth;
        AccountManager.CurrentAccount.WindowHeight = (int)WindowStateData.WindowHeight;
        AccountManager.CurrentAccount.WindowPositionX = (int)WindowStateData.WindowPositionX;
        AccountManager.CurrentAccount.WindowPositionY = (int)WindowStateData.WindowPositionY;
    }
}