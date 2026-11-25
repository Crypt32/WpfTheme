using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Wpf.Demo.Views.UserControls;
/// <summary>
/// Interaction logic for BasicControls.xaml
/// </summary>
public partial class BasicControls {
    public BasicControls() {
        InitializeComponent();
        ComboBoxByName.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Group));
        ComboBoxByKind.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Kind));
        ComboBoxEditable.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Group));

        ListBoxC.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Group));
        ListViewC.ItemsSource = GroupedSampleItem.CreateView(nameof(GroupedSampleItem.Group));
    }
}
public sealed class GroupedSampleItem {
    GroupedSampleItem(String group, GroupedSampleKind kind, String name) {
        Group = group;
        Kind = kind;
        Name = name;
    }

    public String Group { get; }
    public GroupedSampleKind Kind { get; }
    public String Name { get; }

    public static ICollectionView CreateView(String groupPath) {
        GroupedSampleKind hash = new("Hash algorithms");
        GroupedSampleKind sign = new("Signature algorithms");
        GroupedSampleItem[] items = [
            new("Hash", hash, "SHA-1"),
            new("Hash", hash, "SHA-256"),
            new("Hash", hash, "SHA-384"),
            new("Hash", hash, "SHA-512"),
            new("Signature", sign, "RSA"),
            new("Signature", sign, "ECDSA"),
            new("Signature", sign, "Ed25519"),
            new("Signature", sign, "ML-DSA")
        ];
        ListCollectionView view = new(items);
        view.GroupDescriptions!.Add(new PropertyGroupDescription(groupPath));
        return view;
    }
}

public sealed record GroupedSampleKind(String DisplayName);