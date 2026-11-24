using System.ComponentModel;
using System.Windows.Data;

namespace Wpf.Demo.Views.UserControls;

/// <summary>Sample row for the grouping demos: <see cref="Group"/> is a plain string, <see cref="Kind"/> an object whose DisplayName is the header text.</summary>
public sealed class GroupedSampleItem {
    GroupedSampleItem(string group, GroupedSampleKind kind, string name) {
        Group = group;
        Kind = kind;
        Name = name;
    }

    public string Group { get; }
    public GroupedSampleKind Kind { get; }
    public string Name { get; }

    public static ICollectionView CreateView(string groupPath) {
        GroupedSampleKind hash = new("Hash algorithms");
        GroupedSampleKind sign = new("Signature algorithms");
        GroupedSampleItem[] items = {
            new("Hash", hash, "SHA-1"), new("Hash", hash, "SHA-256"), new("Hash", hash, "SHA-512"),
            new("Signature", sign, "RSA"), new("Signature", sign, "ECDSA"), new("Signature", sign, "Ed25519")
        };
        ListCollectionView view = new(items);
        view.GroupDescriptions.Add(new PropertyGroupDescription(groupPath));
        return view;
    }
}

public sealed class GroupedSampleKind {
    public GroupedSampleKind(string displayName) {
        DisplayName = displayName;
    }

    public string DisplayName { get; }
}

/// <summary>
/// Interaction logic for ComboBoxUC.xaml
/// </summary>
public partial class ComboBoxUC {
    public ComboBoxUC() {
        InitializeComponent();
        GroupedByName.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Group));
        GroupedByKind.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Kind));
    }
}
