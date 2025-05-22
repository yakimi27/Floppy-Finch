using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.GameLogics.Core;
using FloppyFinchLogics.GameLogics.TargetScoreMode;
using FloppyFinchLogics.TextureManagement.Background;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.Menus;
using FloppyFinchWindows.Resources;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchWindows.GameModes.TargetScoreMode;

public partial class TargetScoreModeGameplayWindow : Window
{
    private readonly TargetScoreModeGame _modeGame;
    private readonly int _targetScoreValue;
    private bool _gameStarted;
    private bool _pauseState;

    public TargetScoreModeGameplayWindow()
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

        SetUpBackground(GameCanvas);

        _modeGame = new TargetScoreModeGame(GameCanvas, ScoreText, ref _targetScoreValue, TargetScoreProgressBar);
        TargetScoreText.Text = $"{Strings.TargetScoreTextBlockText}" + _targetScoreValue;
        TargetScoreProgressBar.Maximum = _targetScoreValue;
        _modeGame.OnGameOver += OpenModeGameOverWindow;
    }

    private void OpenModeGameOverWindow(int score)
    {
        Dispatcher.Invoke(() =>
        {
            WindowStateData.SaveWindowState(Application.Current.MainWindow);
            SaveWindowStateToAccount();
            var gameOverWindow = new TargetScoreModeGameOverWindow(score, Game.CaptureGameCanvas(), _targetScoreValue);
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
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
        var mainMenuWindow = new MainMenuWindow();
        mainMenuWindow.Show();
        Close();
    }

    private void ButtonGamePause_OnClick(object sender, RoutedEventArgs e)
    {
        TogglePauseState();
    }

    private void SetItemsVisibility()
    {
        ScoreText.Visibility = Visibility.Visible;
        TargetScoreText.Visibility = Visibility.Hidden;
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
        ButtonGamePause.Content = _pauseState ? $"{Strings.ResumeButtonText}" : $"{Strings.PauseButtonText}";
        ButtonReturnMainMenu.Visibility = _pauseState ? Visibility.Visible : Visibility.Hidden;
        ButtonReturnMainMenu.Margin = _pauseState ? new Thickness(105, 15, 15, 15) : new Thickness(15);
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

    private void TargetScoreModeGameplayWindow_OnClosing(object? sender, EventArgs e)
    {
        WindowStateData.SaveWindowState(Application.Current.MainWindow);
        SaveWindowStateToAccount();
    }

    private static void SetUpBackground(Canvas mainCanvas)
    {
        if (AccountManager.CurrentAccount == null || AccountManager.CurrentAccount.SelectedBackground == "null")
        {
            mainCanvas.Background = new SolidColorBrush(Color.FromRgb(173, 216, 230));
            return;
        }

        var imageBrush = new ImageBrush
        {
            ImageSource = BackgroundManager.LoadBackground(AccountManager.CurrentAccount.SelectedBackground),
            ViewportUnits = BrushMappingMode.Absolute,
            Stretch = Stretch.UniformToFill,
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top
        };

        var canvasHeight = WindowStateData.WindowHeight;
        var canvasWidth = WindowStateData.WindowWidth;
        imageBrush.Viewport = new Rect(0, 0, canvasWidth, canvasHeight);

        mainCanvas.Background = imageBrush;
    }
}