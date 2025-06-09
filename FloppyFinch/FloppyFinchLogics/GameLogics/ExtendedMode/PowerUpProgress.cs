using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FloppyFinchLogics.GameLogics.ExtendedMode;

public class PowerUpProgress
{
    internal static Border CreateItem(string content, bool isVisible)
    {
        var progressBar = new ProgressBar
        {
            Minimum = 0,
            Maximum = 100,
            Value = 0,
            Height = 10,
            Margin = new Thickness(4, 2, 4, 0)
        };

        return new Border
        {
            Background = new SolidColorBrush(Color.FromRgb(224, 97, 25)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(0x52, 0x31, 0x03)),
            BorderThickness = new Thickness(3, 3, 3, 10),
            Margin = new Thickness(4),
            Padding = new Thickness(5),
            Child = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    new TextBlock
                    {
                        Text = $"{content}",
                        FontSize = 14,
                        Foreground = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    progressBar
                }
            },
            Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed
        };
    }
}