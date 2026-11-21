using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SysadminsLV.WPF.Controls;

/// <summary>
/// Represents a vertical list of <see cref="NotificationItem"/> controls.
/// </summary>
/// <remarks>
/// Notifications can be declared as children in XAML or generated from <see cref="ItemsControl.ItemsSource"/>. In the
/// second case the items are wrapped in <see cref="NotificationItem"/> containers, and properties of the container
/// (<see cref="NotificationItem.Kind"/>, <see cref="NotificationItem.Icon"/>, <see cref="NotificationItem.IsDismissable"/>)
/// are bound to the data through <see cref="ItemsControl.ItemContainerStyle"/>.
/// </remarks>
public class NotificationPanel : ItemsControl {
    static NotificationPanel() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationPanel), new FrameworkPropertyMetadata(typeof(NotificationPanel)));
        EventManager.RegisterClassHandler(
            typeof(NotificationPanel),
            NotificationItem.DismissRequestedEvent,
            new RoutedEventHandler(OnItemDismissRequested));
    }

    public static readonly DependencyProperty RemoveItemOnCloseProperty = DependencyProperty.Register(
        nameof(RemoveItemOnClose),
        typeof(Boolean),
        typeof(NotificationPanel),
        new FrameworkPropertyMetadata(false));
    /// <summary>
    /// Gets or sets a value that indicates whether a notification is removed from the panel when it is dismissed.
    /// The default value is <c>false</c>, which only hides the dismissed notification.
    /// </summary>
    /// <remarks>
    /// When the panel is bound to <see cref="ItemsControl.ItemsSource"/>, the data item is removed from the source, which
    /// must implement <see cref="IList"/>. A source that does not implement it is left untouched.
    /// </remarks>
    public Boolean RemoveItemOnClose {
        get => (Boolean)GetValue(RemoveItemOnCloseProperty);
        set => SetValue(RemoveItemOnCloseProperty, value);
    }

    /// <inheritdoc />
    protected override Boolean IsItemItsOwnContainerOverride(Object item) {
        return item is NotificationItem;
    }
    /// <inheritdoc />
    protected override DependencyObject GetContainerForItemOverride() {
        return new NotificationItem();
    }

    static void OnItemDismissRequested(Object sender, RoutedEventArgs e) {
        var panel = (NotificationPanel)sender;
        if (!panel.RemoveItemOnClose || e.OriginalSource is not NotificationItem container) {
            return;
        }

        if (panel.ItemsSource != null) {
            // Items is read-only when ItemsSource is set, so only a writable source can be changed
            if (panel.ItemsSource is IList { IsReadOnly: false, IsFixedSize: false } list) {
                Object dataItem = panel.ItemContainerGenerator.ItemFromContainer(container);
                if (dataItem != DependencyProperty.UnsetValue && list.Contains(dataItem)) {
                    list.Remove(dataItem);
                }
            }
        } else if (panel.Items.Contains(container)) {
            panel.Items.Remove(container);
        }
    }
}
