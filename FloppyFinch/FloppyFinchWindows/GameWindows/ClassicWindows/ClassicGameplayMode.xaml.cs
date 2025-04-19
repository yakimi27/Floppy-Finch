using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using FloppyFinchLogics.GameLogics;
using FloppyFinchLogics.WindowLogics;
using WindowState = System.Windows.WindowState;

namespace FloppyFinchGameModes.GameWindows.ClassicWindows;

public partial class ClassicGameplayMode : Window
{
    private readonly Game _game;
    private bool _gameStarted;

    public ClassicGameplayMode()
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

        _game = new Game(GameCanvas, ScoreText);
        _game.OnGameOver += OpenGameOverWindow;
    }

    private void OpenGameOverWindow(int score)
    {
        Dispatcher.Invoke(() =>
        {
            var gameOverWindow = new ClassicGameOverWindow(score, Game.CaptureGameCanvas());
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
    }
}