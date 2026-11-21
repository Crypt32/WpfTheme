using System;
using System.Windows;
using System.Windows.Controls;

namespace SysadminsLV.WPF.OfficeTheme.Controls {
    public class LoadingSpinner : Control {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(String),
            typeof(LoadingSpinner),
            new FrameworkPropertyMetadata(null));
        /// <summary>
        /// indicates whether the control should show or not.
        /// </summary>
        public String Text {
            get => (String)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(
            nameof(IsShown),
            typeof(Boolean),
            typeof(LoadingSpinner),
            new FrameworkPropertyMetadata(false));
        /// <summary>
        /// indicates whether the control should show or not.
        /// </summary>
        public Boolean IsShown {
            get => (Boolean)GetValue(IsShownProperty);
            set => SetValue(IsShownProperty, value);
        }
    }
}