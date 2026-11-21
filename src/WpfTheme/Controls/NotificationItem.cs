using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SysadminsLV.WPF.Controls;

/// <summary>
/// Represents a single notification: an optional icon, arbitrary content and an optional dismiss button.
/// </summary>
/// <remarks>
/// The look of the item depends on <see cref="Kind"/>. Dismissing the item hides it and raises
/// <see cref="DismissRequested"/>; a <see cref="NotificationPanel"/> that hosts the item can also remove it.
/// </remarks>
public class NotificationItem : ContentControl {
    static NotificationItem() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationItem), new FrameworkPropertyMetadata(typeof(NotificationItem)));
        CommandManager.RegisterClassCommandBinding(
            typeof(NotificationItem),
            new CommandBinding(DismissCommand, OnDismissExecuted, OnDismissCanExecute));
    }

    /// <summary>
    /// Command that dismisses the notification. The dismiss button of the item uses it; the command is handled by the
    /// nearest <see cref="NotificationItem"/> up the visual tree from the element that invokes it.
    /// </summary>
    public static readonly RoutedUICommand DismissCommand = new(
        nameof(DismissCommand),
        nameof(DismissCommand),
        typeof(NotificationItem));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(ImageSource),
        typeof(NotificationItem),
        new FrameworkPropertyMetadata(null));
    /// <summary>
    /// Gets or sets the icon displayed before the content. No space is reserved when the icon is not set.
    /// </summary>
    public ImageSource? Icon {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
        nameof(Kind),
        typeof(NotificationItemKind),
        typeof(NotificationItem),
        new FrameworkPropertyMetadata(NotificationItemKind.Info));
    /// <summary>
    /// Gets or sets the kind of the notification. The default value is <see cref="NotificationItemKind.Info"/>.
    /// </summary>
    public NotificationItemKind Kind {
        get => (NotificationItemKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public static readonly DependencyProperty IsDismissableProperty = DependencyProperty.Register(
        nameof(IsDismissable),
        typeof(Boolean),
        typeof(NotificationItem),
        new FrameworkPropertyMetadata(true));
    /// <summary>
    /// Gets or sets a value that indicates whether the user can dismiss the notification. When <c>false</c>, the dismiss
    /// button is hidden and <see cref="Dismiss"/> does nothing. The default value is <c>true</c>.
    /// </summary>
    public Boolean IsDismissable {
        get => (Boolean)GetValue(IsDismissableProperty);
        set => SetValue(IsDismissableProperty, value);
    }

    public static readonly DependencyProperty IsDismissedProperty = DependencyProperty.Register(
        nameof(IsDismissed),
        typeof(Boolean),
        typeof(NotificationItem),
        new FrameworkPropertyMetadata(false));
    /// <summary>
    /// Gets or sets a value that indicates whether the notification is dismissed. A dismissed notification is collapsed.
    /// </summary>
    public Boolean IsDismissed {
        get => (Boolean)GetValue(IsDismissedProperty);
        set => SetValue(IsDismissedProperty, value);
    }

    public static readonly RoutedEvent DismissRequestedEvent = EventManager.RegisterRoutedEvent(
        nameof(DismissRequested),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(NotificationItem));
    /// <summary>
    /// Occurs after the notification is dismissed.
    /// </summary>
    public event RoutedEventHandler DismissRequested {
        add => AddHandler(DismissRequestedEvent, value);
        remove => RemoveHandler(DismissRequestedEvent, value);
    }

    /// <summary>
    /// Dismisses the notification: marks it as dismissed and raises <see cref="DismissRequested"/>. Does nothing when
    /// <see cref="IsDismissable"/> is <c>false</c>.
    /// </summary>
    public void Dismiss() {
        if (!IsDismissable) {
            return;
        }

        IsDismissed = true;
        RaiseEvent(new RoutedEventArgs(DismissRequestedEvent, this));
    }

    static void OnDismissExecuted(Object sender, ExecutedRoutedEventArgs e) {
        ((NotificationItem)sender).Dismiss();
        e.Handled = true;
    }
    static void OnDismissCanExecute(Object sender, CanExecuteRoutedEventArgs e) {
        e.CanExecute = ((NotificationItem)sender).IsDismissable;
        e.Handled = true;
    }
}
