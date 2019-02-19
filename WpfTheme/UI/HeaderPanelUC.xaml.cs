using System;
using System.ComponentModel;
using System.Windows;

namespace SysadminsLV.WPF.OfficeTheme {
    /// <summary>
    /// Interaction logic for HeaderPanelUC.xaml
    /// </summary>
    public partial class HeaderPanelUC : INotifyPropertyChanged {
		FrameworkElement content;
		public HeaderPanelUC() {
			InitializeComponent();
		}
		public FrameworkElement Header {
			get => content;
		    set {
				content = value;
				OnPropertyChanged("Header");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged(String name = null) {
			var handler = PropertyChanged;
			if (handler != null) {
				handler(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
