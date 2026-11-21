using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SysadminsLV.WPF.Collections;

/// <summary>
/// Represents a dynamic list that provides notifications when items get added, removed, replaced or when
/// the whole list is refreshed, supports range operations and can report property changes of its elements.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
/// <remarks>
/// <para>
/// Range operations (<see cref="AddRange"/>, <see cref="InsertRange"/>, <see cref="RemoveRange"/>,
/// <see cref="RemoveAll"/>, <see cref="Sort()"/>, <see cref="Reverse"/>) raise a single
/// <see cref="CollectionChanged"/> event. When the operation affects exactly one element, the event describes
/// that element and its index. When it affects more than one, the event is
/// <see cref="NotifyCollectionChangedAction.Reset"/>, because WPF collection views do not support events
/// that describe several elements at once.
/// </para>
/// <para>
/// Methods with the <c>Silent</c> suffix do not raise any change notification, but they still maintain
/// the element property tracking described by <see cref="IsNotifyingNested"/>.
/// </para>
/// <para>
/// If the same object is stored in the list more than once, <see cref="ItemPropertyChanged"/> is raised once for each
/// occurrence.
/// </para>
/// </remarks>
public class ObservableList<T> : IList<T>, IReadOnlyList<T>, IList, INotifyCollectionChanged, INotifyPropertyChanged {
    const String CountName = nameof(Count);
    const String IndexerName = "Item[]";

    readonly List<T> _items = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableList{T}"/> class and specifies whether it provides
    /// observation features.
    /// </summary>
    /// <param name="observe">
    /// <strong>True</strong> if event subscribers are notified about collection changes,
    /// otherwise <strong>False</strong>.
    /// </param>
    /// <param name="observeChild">
    /// <strong>True</strong> if the list tracks <see cref="INotifyPropertyChanged.PropertyChanged"/> events of its elements and
    /// reports them via <see cref="ItemPropertyChanged"/>. Elements that do not implement
    /// <see cref="INotifyPropertyChanged"/> are not tracked.
    /// </param>
    public ObservableList(Boolean observe = true, Boolean observeChild = true) {
        IsNotifying = observe;
        IsNotifyingNested = observeChild;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableList{T}"/> class that contains elements copied
    /// from the specified collection. No notifications are raised for the initial elements.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <param name="observe">
    /// <strong>True</strong> if event subscribers are notified about collection changes,
    /// otherwise <strong>False</strong>.
    /// </param>
    /// <param name="observeChild">
    /// <strong>True</strong> if the list tracks <see cref="INotifyPropertyChanged.PropertyChanged"/> events of its elements and
    /// reports them via <see cref="ItemPropertyChanged"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <strong>null</strong>.</exception>
    public ObservableList(IEnumerable<T> collection, Boolean observe = true, Boolean observeChild = true) : this(observe, observeChild) {
        AddRangeSilent(collection);
    }

    /// <summary>
    /// Gets a value indicating whether event subscribers are notified about collection changes.
    /// </summary>
    public Boolean IsNotifying { get; }
    /// <summary>
    /// Gets a value indicating whether the list tracks property changes of its elements and reports them
    /// via <see cref="ItemPropertyChanged"/>.
    /// </summary>
    public Boolean IsNotifyingNested { get; }
    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public Int32 Count => _items.Count;

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    public T this[Int32 index] {
        get => _items[index];
        set {
            T oldValue = _items[index];
            _items[index] = value;
            detach(oldValue);
            attach(value);
            notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue, index), false);
        }
    }

    /// <summary>
    /// Occurs when an item is added, removed, replaced, or the entire list is refreshed.
    /// </summary>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    /// <summary>
    /// Occurs when a property of the list changes (<see cref="Count"/> and the indexer).
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// Occurs when a property of a list element changes. The event is raised only if
    /// <see cref="IsNotifyingNested"/> is <strong>True</strong> and the element implements
    /// <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public event EventHandler<ItemPropertyChangedEventArgs<T>>? ItemPropertyChanged;

    /// <summary>
    /// Adds an object to the end of the list.
    /// </summary>
    /// <param name="item">The object to add. The value can be null for reference types.</param>
    public void Add(T item) {
        _items.Add(item);
        attach(item);
        notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, _items.Count - 1), true);
    }
    /// <summary>
    /// Adds the elements of the specified collection to the end of the list. Event subscribers are notified once,
    /// when all items are added.
    /// </summary>
    /// <param name="collection">The collection whose elements are added.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <strong>null</strong>.</exception>
    public void AddRange(IEnumerable<T> collection) {
        InsertRange(_items.Count, collection);
    }
    /// <summary>
    /// Adds the elements of the specified collection to the end of the list without notifying event subscribers.
    /// </summary>
    /// <param name="collection">The collection whose elements are added.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <strong>null</strong>.</exception>
    public void AddRangeSilent(IEnumerable<T> collection) {
        InsertRangeSilent(_items.Count, collection);
    }
    /// <summary>
    /// Removes all elements from the list.
    /// </summary>
    public void Clear() {
        if (_items.Count == 0) {
            return;
        }
        List<T> removed = [.. _items];
        _items.Clear();
        detachRange(removed);
        notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), true);
    }
    /// <summary>
    /// Determines whether the list contains the specified element.
    /// </summary>
    /// <param name="item">The object to locate.</param>
    /// <returns><strong>True</strong> if the element is found, otherwise <strong>False</strong>.</returns>
    public Boolean Contains(T item) {
        return _items.Contains(item);
    }
    /// <summary>
    /// Copies the entire list to a compatible one-dimensional array, starting at the specified array index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    public void CopyTo(T[] array, Int32 arrayIndex) {
        _items.CopyTo(array, arrayIndex);
    }
    /// <summary>
    /// Determines whether the list contains elements that match the conditions defined by the predicate.
    /// </summary>
    /// <param name="match">The predicate that defines the conditions of the elements to search for.</param>
    /// <returns><strong>True</strong> if at least one matching element is found, otherwise <strong>False</strong>.</returns>
    public Boolean Exists(Predicate<T> match) {
        return _items.Exists(match);
    }
    /// <summary>
    /// Searches for an element that matches the conditions defined by the predicate and returns the first one.
    /// </summary>
    /// <param name="match">The predicate that defines the conditions of the element to search for.</param>
    /// <returns>The first matching element, or the default value of <typeparamref name="T"/> if none is found.</returns>
    public T? Find(Predicate<T> match) {
        return _items.Find(match);
    }
    /// <summary>
    /// Retrieves all the elements that match the conditions defined by the predicate.
    /// </summary>
    /// <param name="match">The predicate that defines the conditions of the elements to search for.</param>
    /// <returns>A new list that contains the matching elements.</returns>
    public List<T> FindAll(Predicate<T> match) {
        return _items.FindAll(match);
    }
    /// <summary>
    /// Searches for an element that matches the conditions defined by the predicate and returns the zero-based index
    /// of the first occurrence.
    /// </summary>
    /// <param name="match">The predicate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first matching element, or -1 if none is found.</returns>
    public Int32 FindIndex(Predicate<T> match) {
        return _items.FindIndex(match);
    }
    /// <summary>
    /// Performs the specified action on each element of the list.
    /// </summary>
    /// <param name="action">The action to perform on each element.</param>
    public void ForEach(Action<T> action) {
        _items.ForEach(action);
    }
    /// <summary>
    /// Raises <see cref="CollectionChanged"/> with the <see cref="NotifyCollectionChangedAction.Reset"/> action
    /// to force subscribers to refresh.
    /// </summary>
    public void ForceUpdate() {
        notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), false);
    }
    /// <summary>
    /// Returns an enumerator that iterates through the list.
    /// </summary>
    /// <returns>An enumerator for the list.</returns>
    public IEnumerator<T> GetEnumerator() {
        return _items.GetEnumerator();
    }
    /// <summary>
    /// Creates a shallow copy of a range of elements in the list.
    /// </summary>
    /// <param name="index">The zero-based index at which the range starts.</param>
    /// <param name="count">The number of elements in the range.</param>
    /// <returns>A new list that contains the elements from the specified range.</returns>
    public List<T> GetRange(Int32 index, Int32 count) {
        return _items.GetRange(index, count);
    }
    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <param name="item">The object to locate.</param>
    /// <returns>The zero-based index of the first occurrence, or -1 if the object is not found.</returns>
    public Int32 IndexOf(T item) {
        return _items.IndexOf(item);
    }
    /// <summary>
    /// Inserts an element into the list at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The object to insert. The value can be null for reference types.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    public void Insert(Int32 index, T item) {
        _items.Insert(index, item);
        attach(item);
        notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index), true);
    }
    /// <summary>
    /// Inserts the elements of a collection into the list at the specified index. Event subscribers are notified once,
    /// when all items are inserted.
    /// </summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="collection">The collection whose elements are inserted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <strong>null</strong>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    public void InsertRange(Int32 index, IEnumerable<T> collection) {
        List<T> inserted = insertRange(index, collection);
        if (inserted.Count == 1) {
            notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, inserted[0], index), true);
        } else if (inserted.Count > 1) {
            notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), true);
        }
    }
    /// <summary>
    /// Inserts the elements of a collection into the list at the specified index without notifying event subscribers.
    /// </summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="collection">The collection whose elements are inserted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <strong>null</strong>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    public void InsertRangeSilent(Int32 index, IEnumerable<T> collection) {
        insertRange(index, collection);
    }
    /// <summary>
    /// Removes the first occurrence of the specified object from the list.
    /// </summary>
    /// <param name="item">The object to remove.</param>
    /// <returns><strong>True</strong> if the object is removed, <strong>False</strong> if it is not found.</returns>
    public Boolean Remove(T item) {
        Int32 index = _items.IndexOf(item);
        if (index < 0) {
            return false;
        }
        RemoveAt(index);

        return true;
    }
    /// <summary>
    /// Removes all the elements that match the conditions defined by the predicate. Event subscribers are notified
    /// once, only if at least one element is removed.
    /// </summary>
    /// <param name="match">The predicate that defines the conditions of the elements to remove.</param>
    /// <returns>The number of removed elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="match"/> is <strong>null</strong>.</exception>
    public Int32 RemoveAll(Predicate<T> match) {
        if (match == null) {
            throw new ArgumentNullException(nameof(match));
        }
        List<T> removed = [];
        _items.RemoveAll(x => {
            if (!match(x)) {
                return false;
            }
            removed.Add(x);

            return true;
        });
        detachRange(removed);
        if (removed.Count > 0) {
            notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), true);
        }

        return removed.Count;
    }
    /// <summary>
    /// Removes the element at the specified index of the list.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    public void RemoveAt(Int32 index) {
        T removed = _items[index];
        _items.RemoveAt(index);
        detach(removed);
        notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed, index), true);
    }
    /// <summary>
    /// Removes a range of elements from the list. Event subscribers are notified once, when all items are removed.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> or <paramref name="count"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the list.
    /// </exception>
    public void RemoveRange(Int32 index, Int32 count) {
        List<T> removed = removeRange(index, count);
        if (removed.Count == 1) {
            notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed[0], index), true);
        } else if (removed.Count > 1) {
            notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), true);
        }
    }
    /// <summary>
    /// Removes a range of elements from the list without notifying event subscribers.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> or <paramref name="count"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the list.
    /// </exception>
    public void RemoveRangeSilent(Int32 index, Int32 count) {
        removeRange(index, count);
    }
    /// <summary>
    /// Reverses the order of the elements in the list. Event subscribers are notified once.
    /// </summary>
    public void Reverse() {
        _items.Reverse();
        notifyReorder();
    }
    /// <summary>
    /// Sorts the elements in the list using the default comparer. Event subscribers are notified once.
    /// </summary>
    public void Sort() {
        _items.Sort();
        notifyReorder();
    }
    /// <summary>
    /// Sorts the elements in the list using the specified comparer. Event subscribers are notified once.
    /// </summary>
    /// <param name="comparer">The comparer to use when comparing elements.</param>
    public void Sort(IComparer<T>? comparer) {
        _items.Sort(comparer);
        notifyReorder();
    }
    /// <summary>
    /// Sorts the elements in the list using the specified comparison. Event subscribers are notified once.
    /// </summary>
    /// <param name="comparison">The comparison to use when comparing elements.</param>
    public void Sort(Comparison<T> comparison) {
        _items.Sort(comparison);
        notifyReorder();
    }
    /// <summary>
    /// Copies the elements of the list to a new array.
    /// </summary>
    /// <returns>An array that contains copies of the elements of the list.</returns>
    public T[] ToArray() {
        return _items.ToArray();
    }

    /// <summary>
    /// Raises the <see cref="CollectionChanged"/> event. The event is not raised if <see cref="IsNotifying"/> is
    /// <strong>False</strong>.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
        if (IsNotifying) {
            CollectionChanged?.Invoke(this, e);
        }
    }
    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event. The event is not raised if <see cref="IsNotifying"/> is
    /// <strong>False</strong>.
    /// </summary>
    /// <param name="propertyName">The name of the property that has changed.</param>
    protected virtual void OnPropertyChanged(String propertyName) {
        if (IsNotifying) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Raises the <see cref="ItemPropertyChanged"/> event.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected virtual void OnItemPropertyChanged(ItemPropertyChangedEventArgs<T> e) {
        ItemPropertyChanged?.Invoke(this, e);
    }

    List<T> insertRange(Int32 index, IEnumerable<T> collection) {
        if (collection == null) {
            throw new ArgumentNullException(nameof(collection));
        }
        // materialize once: the source may be lazy, or may depend on this list.
        List<T> items = [.. collection];
        _items.InsertRange(index, items);
        foreach (T item in items) {
            attach(item);
        }

        return items;
    }
    List<T> removeRange(Int32 index, Int32 count) {
        List<T> removed = _items.GetRange(index, count);
        _items.RemoveRange(index, count);
        detachRange(removed);

        return removed;
    }
    void notifyReorder() {
        notify(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), false);
    }
    void notify(NotifyCollectionChangedEventArgs e, Boolean countChanged) {
        if (!IsNotifying) {
            return;
        }
        // same order as ObservableCollection<T>: property notifications precede the collection notification.
        if (countChanged) {
            OnPropertyChanged(CountName);
        }
        OnPropertyChanged(IndexerName);
        OnCollectionChanged(e);
    }
    void attach(T item) {
        if (IsNotifyingNested && item is INotifyPropertyChanged vm) {
            vm.PropertyChanged += onItemPropertyChanged;
        }
    }
    void detach(T item) {
        if (IsNotifyingNested && item is INotifyPropertyChanged vm) {
            vm.PropertyChanged -= onItemPropertyChanged;
        }
    }
    void detachRange(List<T> items) {
        foreach (T item in items) {
            detach(item);
        }
    }
    void onItemPropertyChanged(Object? sender, PropertyChangedEventArgs e) {
        if (sender is T item) {
            OnItemPropertyChanged(new ItemPropertyChangedEventArgs<T>(item, e.PropertyName));
        }
    }
    static T castItem(Object? value) {
        if (value is T item) {
            return item;
        }
        if (value == null && default(T) == null) {
            return default!;
        }

        throw new ArgumentException($"Value is not compatible with the list element type '{typeof(T)}'.", nameof(value));
    }

    #region Explicit interface implementations

    Boolean ICollection<T>.IsReadOnly => false;
    Boolean IList.IsReadOnly => false;
    Boolean IList.IsFixedSize => false;
    Boolean ICollection.IsSynchronized => false;
    Object ICollection.SyncRoot => ((ICollection)_items).SyncRoot;
    Object? IList.this[Int32 index] {
        get => this[index];
        set => this[index] = castItem(value);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
    Int32 IList.Add(Object? value) {
        Add(castItem(value));

        return _items.Count - 1;
    }
    Boolean IList.Contains(Object? value) {
        return value is T item ? Contains(item) : value == null && default(T) == null && Contains(default!);
    }
    Int32 IList.IndexOf(Object? value) {
        return value is T item ? IndexOf(item) : value == null && default(T) == null ? IndexOf(default!) : -1;
    }
    void IList.Insert(Int32 index, Object? value) {
        Insert(index, castItem(value));
    }
    void IList.Remove(Object? value) {
        if (value is T item) {
            Remove(item);
        } else if (value == null && default(T) == null) {
            Remove(default!);
        }
    }
    void ICollection.CopyTo(Array array, Int32 index) {
        ((ICollection)_items).CopyTo(array, index);
    }

    #endregion
}
