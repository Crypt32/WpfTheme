using System;
using System.Collections.ObjectModel;
using SysadminsLV.WPF.Controls;

namespace Wpf.Demo.Views.UserControls;
/// <summary>
/// Interaction logic for NotificationPanelUC.xaml
/// </summary>
public partial class NotificationPanelUC {
    public NotificationPanelUC() {
        Notifications.Add(new NotificationVM { Kind = NotificationItemKind.Info, Text = "Dismiss me and I am removed from the collection." });
        Notifications.Add(new NotificationVM { Kind = NotificationItemKind.Success, Text = "Saved." });
        Notifications.Add(new NotificationVM { Kind = NotificationItemKind.Danger, Text = "Cannot be dismissed.", CanDismiss = false });
        InitializeComponent();
    }

    public ObservableCollection<NotificationVM> Notifications { get; } = [];
}
public class NotificationVM {
    public NotificationItemKind Kind { get; set; }
    public String Text { get; set; } = String.Empty;
    public Boolean CanDismiss { get; set; } = true;
}
