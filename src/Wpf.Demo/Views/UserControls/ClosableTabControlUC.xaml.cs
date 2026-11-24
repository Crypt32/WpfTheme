using System;
using System.Collections.ObjectModel;

namespace Wpf.Demo.Views.UserControls;
/// <summary>
/// Interaction logic for ClosableTabControl.xaml
/// </summary>
public partial class ClosableTabControlUC {
    public ClosableTabControlUC() {
        InitializeComponent();
        Tabs.Add(new TabVM { Title = "Hello1", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello2", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello3", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello4", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello5", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello6", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello7", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello8", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello9", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello10", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello11", Content = "Hello2" });
        Tabs.Add(new TabVM { Title = "Hello12", Content = "Hello2" });
    }

    public ObservableCollection<TabVM> Tabs { get; } = new();
}
public class TabVM {
    public String Title { get; set; }
    public String Content { get; set; }
}
