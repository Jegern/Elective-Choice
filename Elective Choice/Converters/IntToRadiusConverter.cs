using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Elective_Choice.Converters;

public class IntToRadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new CornerRadius((double) value / 2.0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}