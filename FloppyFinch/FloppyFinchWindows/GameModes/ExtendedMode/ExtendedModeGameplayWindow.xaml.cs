using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.GameLogics.Core;
using FloppyFinchLogics.GameLogics.ExtendedMode;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchWindows.GameModes.ExtendedMode;

public partial class ExtendedModeModeGameplayWindow : Window
{
    private readonly ExtendedModeGame _modeGame;
    private bool _gameStarted;
    private bool _pauseState;

    public ExtendedModeModeGameplayWindow()
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

        // set the window to use hardware acceleration
        Loaded += (s, e) =>
        {
            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource == null) return;
            var hwndTarget = hwndSource.CompositionTarget;
            if (hwndTarget != null) hwndTarget.RenderMode = RenderMode.Default;
        };

        _modeGame = new ExtendedModeGame(GameCanvas, ScoreText, HeartsCountLabel, PowerUpSpaceGrid);
        _modeGame.OnGameOver += OpenModeGameOverWindow;
        RefreshLayout();
    }

    private void OpenModeGameOverWindow(int score)
    {
        Dispatcher.Invoke(() =>
        {
            var gameOverWindow = new ExtendedModeGameOverWindow(score, Game.CaptureGameCanvas());
            gameOverWindow.Show();
            Close();
        });
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape && _gameStarted)
        {
            TogglePauseState();
            return;
        }

        if (e.Key == Key.Space) HandleActionKey();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        HandleActionKey();
    }

    private void ButtonReturnMainMenu_OnClick(object sender, RoutedEventArgs e)
    {
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void ButtonGamePause_OnClick(object sender, RoutedEventArgs e)
    {
        _pauseState = !_pauseState;
        _modeGame.PauseGame(_pauseState);

        ButtonGamePause.Content = _pauseState ? "Resume" : "Pause";
        ButtonReturnMainMenu.Visibility = _pauseState ? Visibility.Visible : Visibility.Hidden;
        ButtonReturnMainMenu.Margin = _pauseState ? new Thickness(105, 15, 15, 15) : new Thickness(15);
    }

    private void SetItemsVisibility()
    {
        ScoreText.Visibility = Visibility.Visible;
        KeyWait.Visibility = Visibility.Hidden;
        ButtonReturnMainMenu.Visibility = Visibility.Hidden;
        ButtonGamePause.Visibility = Visibility.Visible;
    }

    private void HandleActionKey()
    {
        if (!_pauseState) _modeGame.Bird.Jump();
        if (!_gameStarted)
        {
            _modeGame.StartCheck(true);
            _gameStarted = true;
            SetItemsVisibility();
        }
    }

    private void TogglePauseState()
    {
        _pauseState = !_pauseState;
        _modeGame.PauseGame(_pauseState);
        UpdatePauseUi();
    }

    private void UpdatePauseUi()
    {
        ButtonGamePause.Content = _pauseState ? "Resume" : "Pause";
        ButtonReturnMainMenu.Visibility = _pauseState ? Visibility.Visible : Visibility.Hidden;
        ButtonReturnMainMenu.Margin = _pauseState ? new Thickness(105, 15, 15, 15) : new Thickness(15);
    }

    private void RefreshLayout()
    {
        PowerUpSpaceGrid.Children.Clear();
        /*
        var itemJetpack = CreateItem("Jetpack", false);
        var itemScoreMultiplayer = CreateItem("Score Multiplayer", false);
        var itemShield = CreateItem("Shield", false);

        if (itemJetpack.Visibility == Visibility.Visible) PowerUpSpaceGrid.Children.Add(itemJetpack);
        if (itemScoreMultiplayer.Visibility == Visibility.Visible) PowerUpSpaceGrid.Children.Add(itemScoreMultiplayer);
        if (itemShield.Visibility == Visibility.Visible) PowerUpSpaceGrid.Children.Add(itemShield);
?
        PowerUpSpaceGrid.Columns = PowerUpSpaceGrid.Children.Count > 0 ? PowerUpSpaceGrid.Children.Count : 1;
        */
    }
}