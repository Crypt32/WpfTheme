using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Converters {
    public class BooleanToVisibilityReverse : IValueConverter {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            return value != null && (Boolean)value
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
