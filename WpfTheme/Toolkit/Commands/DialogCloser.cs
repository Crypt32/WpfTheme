using System;
using System.Windows;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    public static class DialogCloser {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(Boolean?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));
        static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e) {
            if (d is Window window) {
                window.DialogResult = e.NewValue as Boolean?;
            }
        }
        public static void SetDialogResult(Window target, Boolean? value) {
            target.SetValue(DialogResultProperty, value);
        }
    }
}
