using System;
using System.Windows;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit {
    public class MsgBox {
        public static MessageBoxResult Show(String header, String message, MessageBoxImage image = MessageBoxImage.Error, MessageBoxButton button = MessageBoxButton.OK) {
            WindowCollection windows = Application.Current.Windows;
            Window hwnd = null;
            if (windows.Count > 0) {
                hwnd = windows[windows.Count - 1];
            }
            return hwnd == null
                ? MessageBox.Show(message, header, button, image)
                : MessageBox.Show(hwnd, message, header, button, image);
        }
    }
}
