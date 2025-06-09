using System.Windows;

namespace FloppyFinchLogics.WindowLogics;

public static class WindowStateData
{
    public static bool Maximized { set; get; }

    public static double WindowHeight { set; get; }

    public static double WindowWidth { set; get; }

    public static double WindowPositionX { set; get; }
    public static double WindowPositionY { set; get; }

    public static double WindowPaddingBottom
    {
        get
        {
            var padding = WindowHeight * 0.1;
            if (padding > 88) padding = 88;
            return padding;
        }
    }

    public static double WidthScaleFactor
    {
        get
        {
            if (WindowWidth == 0 || WindowHeight == 0) return 1;
            if (WindowWidth > WindowHeight) return WindowWidth / WindowHeight;
            return 1;
        }
    }

    public static double HeightScaleFactor
    {
        get
        {
            if (WindowWidth == 0 || WindowHeight == 0) return 1;
            if (WindowWidth < WindowHeight) return WindowHeight / WindowWidth;
            return 1;
        }
    }

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