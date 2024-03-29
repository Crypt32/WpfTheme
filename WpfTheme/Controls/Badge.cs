﻿using System.Windows;
using System.Windows.Controls;

namespace SysadminsLV.WPF.OfficeTheme.Controls {
    /// <summary>
    /// Represents badge control. This class inherits from <see cref="Label"/> WPF control.
    /// </summary>
    public class Badge : Label {
        public static readonly DependencyProperty ThemeColorProperty = DependencyProperty.Register(
            nameof(ThemeColor),
            typeof(ThemedColor),
            typeof(Badge),
            new FrameworkPropertyMetadata(ThemedColor.Default));
        /// <summary>
        /// Gets or sets badge color theme.
        /// </summary>
        public ThemedColor ThemeColor {
            get => (ThemedColor)GetValue(ThemeColorProperty);
            set => SetValue(ThemeColorProperty, value);
        }

        public static readonly DependencyProperty BadgeStyleProperty = DependencyProperty.Register(
            nameof(BadgeStyle),
            typeof(BadgeStyle),
            typeof(Badge),
            new FrameworkPropertyMetadata(BadgeStyle.Outline));
        /// <summary>
        /// Gets or sets the badge look.
        /// </summary>
        public BadgeStyle BadgeStyle {
            get => (BadgeStyle)GetValue(ThemeColorProperty);
            set => SetValue(ThemeColorProperty, value);
        }
    }
}