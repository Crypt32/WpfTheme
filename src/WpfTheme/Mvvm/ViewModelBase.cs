using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SysadminsLV.WPF.Mvvm;
/// <summary>
/// Represents the base view model that implements <see cref="INotifyPropertyChanged"/> interface.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged {
    /// <summary>
    /// Stores a value in a field and raises the change of the property, unless the field has the value already.
    /// </summary>
    /// <returns>
    /// <c>true</c> when the value changed.
    /// </returns>
    protected Boolean SetField<T>(ref T field, T value, [CallerMemberName] String? property = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) {
            return false;
        }

        field = value;
        OnPropertyChanged(property);

        return true;
    }


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
