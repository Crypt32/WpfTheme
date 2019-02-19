using System;
using System.ComponentModel;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        protected virtual void OnPropertyChanged(String PropertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
