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
            Background = Brushes.LightGray,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Margin = new Thickness(4),
            Child = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    new TextBlock
                    {
                        Text = $"{content}",
                        FontSize = 14,
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