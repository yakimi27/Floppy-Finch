using System.Windows;

namespace FloppyFinchLogics.WindowLogics;

public static class WindowStateData
{
    public static bool Maximized { set; get; }

    public static double WindowHeight { set; get; }

    public static double WindowWidth { set; get; }

    public static double WindowPositionX { set; get; }
    public static double WindowPositionY { set; get; }

    public static void SaveWindowState(Window window)
    {
        Maximized = window.WindowState == WindowState.Maximized;
        WindowWidth = window.Width;
        WindowHeight = window.Height;
        WindowPositionX = window.Left;
        WindowPositionY = window.Top;
    }

    public static bool IsFirstStartup()
    {
        return WindowPositionX == 0 && WindowPositionY == 0;
    }
}