﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchLogics.GameLogics.Core;
using FloppyFinchLogics.TextureManagement.Backgrounds;
using FloppyFinchLogics.WindowLogics;
using FloppyFinchWindows.Menus;
using WindowState = System.Windows.WindowState;
using Strings = FloppyFinchWindows.Resources.Strings;

namespace FloppyFinchWindows.GameModes.ClassicMode;

public partial class ClassicModeGameplayWindow : Window
{
    private readonly Game _game;
    private bool _gameStarted;
    private bool _pauseState;

    public ClassicModeGameplayWindow()
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

        _game = new Game(GameCanvas, ScoreText);
        _game.OnGameOver += OpenGameOverWindow;
    }

    private void OpenGameOverWindow(int score)
    {
        Dispatcher.Invoke(() =>
        {
            WindowStateData.SaveWindowState(Application.Current.MainWindow);
            SaveWindowStateToAccount();
            var gameOverWindow = new ClassicModeGameOverWindow(score, Game.CaptureGameCanvas());
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
        _pauseState = !_pauseState;
        _game.PauseGame(_pauseState);

        UpdatePauseUi();
    }

    private void SetItemsVisibility()
    {
        ScoreText.Visibility = Visibility.Visible;
        KeyWait.Visibility = Visibility.Hidden;
        ButtonReturnMainMenu.Visibility = Visibility.Collapsed;
        ButtonGamePause.Visibility = Visibility.Visible;
    }

    private void HandleActionKey()
    {
        if (!_pauseState) _game.Bird.Jump();
        if (!_gameStarted)
        {
            _game.StartCheck(true);
            _gameStarted = true;
            SetItemsVisibility();
        }
    }

    private void TogglePauseState()
    {
        _pauseState = !_pauseState;
        _game.PauseGame(_pauseState);
        UpdatePauseUi();
    }

    private void UpdatePauseUi()
    {
        ButtonGamePause.Content = _pauseState ? $"{Strings.ResumeButtonText}" : $"{Strings.PauseButtonText}";
        ButtonReturnMainMenu.Visibility = _pauseState ? Visibility.Visible : Visibility.Collapsed;
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

    private void ClassicModeGameplayWindow_OnClosing(object? sender, EventArgs e)
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