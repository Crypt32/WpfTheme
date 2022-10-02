using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Demo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            ListView.Items.Add(new Oid("sha1"));
            ListView.Items.Add(new Oid("sha256"));
        }
    }
}
