namespace SysadminsLV.WPF.Controls;

/// <summary>
/// Specifies the kind of a notification, which determines its colors.
/// </summary>
public enum NotificationItemKind {
    /// <summary>
    /// Informational notification.
    /// </summary>
    Info = 0,
    /// <summary>
    /// Notification about a successful operation.
    /// </summary>
    Success = 1,
    /// <summary>
    /// Warning notification.
    /// </summary>
    Warning = 2,
    /// <summary>
    /// Notification about an error or a dangerous situation.
    /// </summary>
    Danger = 3
}
