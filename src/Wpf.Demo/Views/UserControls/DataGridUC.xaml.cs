using System.Collections.Generic;
using System.Security.Cryptography;

namespace Wpf.Demo.Views.UserControls;
/// <summary>
/// Interaction logic for DataGridUC.xaml
/// </summary>
public partial class DataGridUC {
    public DataGridUC() {
        InitializeComponent();
        var list = new List<Oid> {
                                     new Oid("sha1"),
                                     new Oid("sha256"),
                                     new Oid("sha384"),
                                     new Oid("sha512")
                                 };
        Grid1.ItemsSource = list;
        Grid2.ItemsSource = list;
    }
}
