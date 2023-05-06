using System.Security.Cryptography;

namespace Wpf.Demo.Views.UserControls;

/// <summary>
/// Interaction logic for ListViewUC.xaml
/// </summary>
public partial class ListViewUC {
    public ListViewUC() {
        InitializeComponent();
        ListView.Items.Add(new Oid("sha1"));
        ListView.Items.Add(new Oid("sha256"));
        ListView2.Items.Add(new Oid("sha1"));
        ListView2.Items.Add(new Oid("sha256"));
        ListView.Items.Add(new Oid("sha1"));
        ListView.Items.Add(new Oid("sha256"));
        ListView2.Items.Add(new Oid("sha1"));
        ListView2.Items.Add(new Oid("sha256"));
        ListView.Items.Add(new Oid("sha1"));
        ListView.Items.Add(new Oid("sha256"));
        ListView2.Items.Add(new Oid("sha1"));
        ListView2.Items.Add(new Oid("sha256"));
    }
}