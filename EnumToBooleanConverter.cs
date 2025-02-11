using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Expenses;

public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        return value.ToString().Equals(parameter.ToString());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked)
        {
            return Enum.Parse(targetType, parameter.ToString());
        }
        return null;
    }
}
