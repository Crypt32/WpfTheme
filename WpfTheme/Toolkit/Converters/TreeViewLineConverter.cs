using System;
using System.Windows.Controls;
using System.Windows.Data;

// Credits: https://stackoverflow.com/a/19560467/3997611

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Converters {
    class TreeViewLineConverter : IValueConverter {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture) {
            var item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture) {
            return false;
        }
    }
}
