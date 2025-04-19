using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using FloppyFinchLogics.GameLogics;
using FloppyFinchLogics.GameLogics.TargetScoreLogics;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.GameWindows.TargetScoreWindows;

public partial class TargetScoreGameplayMode : Window
{
    private readonly TargetScoreGame _game;
    private readonly int _targetScoreValue;
    private bool _gameStarted;

    public TargetScoreGameplayMode()
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

        _game = new TargetScoreGame(GameCanvas, ScoreText, ref _targetScoreValue);
        TargetScoreText.Text = "Target score: " + _targetScoreValue;
        _game.OnGameOver += OpenGameOverWindow;
    }

    private void OpenGameOverWindow(int score)
    {
        Dispatcher.Invoke(() =>
        {
            var gameOverWindow = new TargetScoreGameOverWindow(score, Game.CaptureGameCanvas(), _targetScoreValue);
            gameOverWindow.Show();
            Close();
        });
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Space) return;
        _game.Bird.Jump();
        if (!_gameStarted)
        {
            _game.StartCheck(true);
            _gameStarted = true;
        }

        ScoreText.Visibility = Visibility.Visible;
        KeyWait.Visibility = Visibility.Hidden;
        TargetScoreText.Visibility = Visibility.Hidden;
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _game.Bird.Jump();
        if (!_gameStarted)
        {
            _game.StartCheck(true);
            _gameStarted = true;
        }

        ScoreText.Visibility = Visibility.Visible;
        KeyWait.Visibility = Visibility.Hidden;
        TargetScoreText.Visibility = Visibility.Hidden;
    }
}