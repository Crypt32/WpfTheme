using System;
using SysadminsLV.WPF.OfficeTheme.Toolkit.ViewModels;

namespace Wpf.Demo.ViewModels;
class FormViewModel : ViewModelBase {
    Int32 number;

    public Int32 Number {
        get => number;
        set {
            number = value;
            OnPropertyChanged(nameof(Number));
        }
    }
}
