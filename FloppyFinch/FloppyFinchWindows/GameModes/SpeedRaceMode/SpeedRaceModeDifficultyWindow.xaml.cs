﻿using System.Windows;
using System.Windows.Controls;
using FloppyFinchLogics.WindowLogics;

namespace FloppyFinchWindows.GameModes.SpeedRaceMode;

public partial class SpeedRaceModeDifficultyWindow : Window
{
    private const int SlowSpeed = 3;
    private const int FasterSpeed = 7;
    private const int FastSpeed = 10;
    private const int MinSpeed = 1;
    private const int MaxSpeed = 13;
    private static int _ownDeclaredSpeed;

    public SpeedRaceModeDifficultyWindow()
    {
        InitializeComponent();
        Left = WindowStateData.WindowPositionX + (WindowStateData.WindowWidth - Width) / 2;
        Top = WindowStateData.WindowPositionY + (WindowStateData.WindowHeight - Height) / 2;
    }

    private void SlowSpeedButton_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.Close();
        var speedRaceGameplayWindow = new SpeedRaceModeGameplayWindow(SlowSpeed);
        speedRaceGameplayWindow.Show();
        Close();
    }

    private void FasterSpeedButton_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.Close();
        var speedRaceGameplayWindow = new SpeedRaceModeGameplayWindow(FasterSpeed);
        speedRaceGameplayWindow.Show();
        Close();
    }

    private void FastSpeedButton_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.Close();
        var speedRaceGameplayWindow = new SpeedRaceModeGameplayWindow(FastSpeed);
        speedRaceGameplayWindow.Show();
        Close();
    }

    private void OwnSpeedTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(OwnSpeedTextBox.Text, out var speed) && speed >= MinSpeed && speed <= MaxSpeed)
            OwnSpeedStartButton.IsEnabled = true;
        else
            OwnSpeedStartButton.IsEnabled = false;
    }


    private void OwnSpeedStartButton_OnClick(object sender, RoutedEventArgs e)
    {
        _ownDeclaredSpeed = int.Parse(OwnSpeedTextBox.Text);
        Application.Current.MainWindow!.Close();
        var speedRaceGameplayWindow = new SpeedRaceModeGameplayWindow(_ownDeclaredSpeed);
        speedRaceGameplayWindow.Show();
        Close();
    }
}