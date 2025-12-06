using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Minesweeper_WPF
{
    public class WidthToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                // Példa: margó legyen az ablak szélességének 5%-a
                double margin = width * 0.10;
                return new Thickness(margin, margin, margin, 10);
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
