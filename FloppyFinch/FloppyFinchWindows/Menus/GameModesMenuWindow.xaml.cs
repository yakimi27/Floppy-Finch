using System.Windows;
using FloppyFinchGameModes.GameWindows.ExtendedWindows;
using FloppyFinchGameModes.GameWindows.SpeedRaceWindows;
using FloppyFinchGameModes.GameWindows.TargetScoreWindows;
using FloppyFinchLogics.WindowLogics;
using ClassicModeGameplayWindow = FloppyFinchGameModes.GameModes.ClassicMode.ClassicModeGameplayWindow;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.Menus;

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
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }
}