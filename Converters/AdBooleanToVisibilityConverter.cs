using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfCalava.Converters
{
    //@@trick

    /// <summary>
    /// An AvalonDock-adapted boolean to visibility converter which privileges Visibility.Hidden instead
    /// of Visibility.Collapsed, returned for false by the standard BooleanToVisibilityConverter. Using
    /// a parameter as suggested in http://stackoverflow.com/questions/23617707/binding-to-layoutanchorableitem-visibility-in-avalondock-2
    /// does not seem to do the trick, so I just implemented an ad-hoc converter.
    /// </summary>
    public sealed class AdBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (value is bool && (bool) value);
            return (b ? Visibility.Visible : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility) value) == Visibility.Visible));
        }
    }
}
