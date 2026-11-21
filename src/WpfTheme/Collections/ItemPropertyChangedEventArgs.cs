using System;
using System.ComponentModel;

namespace SysadminsLV.WPF.Collections;

/// <summary>
/// Provides data for an event that is raised when a property of a collection element changes.
/// </summary>
/// <typeparam name="T">The type of the collection element.</typeparam>
public class ItemPropertyChangedEventArgs<T> : PropertyChangedEventArgs {
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemPropertyChangedEventArgs{T}"/> class.
    /// </summary>
    /// <param name="item">The collection element whose property has changed.</param>
    /// <param name="propertyName">
    /// The name of the property that has changed. <strong>Null</strong> or an empty string means that
    /// all properties of the element have changed.
    /// </param>
    public ItemPropertyChangedEventArgs(T item, String? propertyName) : base(propertyName) {
        Item = item;
    }

    /// <summary>
    /// Gets the collection element whose property has changed.
    /// </summary>
    public T Item { get; }
}
