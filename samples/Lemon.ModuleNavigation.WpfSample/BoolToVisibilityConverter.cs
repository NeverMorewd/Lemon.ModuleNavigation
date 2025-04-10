using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Lemon.ModuleNavigation.WpfSample;

public class BoolToVisibilityConverter : IValueConverter
{
    public bool Inverse { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            if (Inverse)
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            else
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            if (Inverse)
                return visibility != Visibility.Visible;
            else
                return visibility == Visibility.Visible;
        }
        return false;
    }
}
