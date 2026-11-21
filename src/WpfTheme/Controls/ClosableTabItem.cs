using System;
using System.Windows;
using System.Windows.Controls;

// Credits: http://www.blogs.intuidev.com/post/2010/post/2010/01/25/TabControlStyling_PartOne.aspx

namespace SysadminsLV.WPF.OfficeTheme.Controls {
    public class ClosableTabItem : TabItem {
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(nameof(IsClosable), typeof(Boolean), typeof(ClosableTabItem), new FrameworkPropertyMetadata(false));
        public Boolean IsClosable {
            get => (Boolean)GetValue(IsClosableProperty);
            set => SetValue(IsClosableProperty, value);
        }
    }
}
