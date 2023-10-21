using System;
using System.Collections.ObjectModel;

namespace Wpf.Demo.Views.UserControls;
/// <summary>
/// Interaction logic for ClosableTabControl.xaml
/// </summary>
public partial class ClosableTabControlUC {
    public ClosableTabControlUC() {
        InitializeComponent();
        Tabs.Add(new TabVM { Header = "Hello", Content = "Hello2" });
    }

    public ObservableCollection<TabVM> Tabs { get; } = new();
}
public class TabVM {
    public String Header { get; set; }
    public String Content { get; set; }
}
