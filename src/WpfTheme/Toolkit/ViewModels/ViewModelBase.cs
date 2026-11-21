#nullable enable
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.ViewModels;
/// <summary>
/// Represents the base view model that implements <see cref="INotifyPropertyChanged"/> interface.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged {
    /// <summary>
    /// Raises property changed event.
    /// </summary>
    /// <param name="propertyName">Property name that changed its value.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null) {
        PropertyChangedEventHandler handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
}
