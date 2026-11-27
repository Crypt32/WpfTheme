using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysadminsLV.WPF.Collections;

namespace SysadminsLV.WPF.Tests.Collections;

[TestClass]
public class ObservableListTests {
    sealed class Child : INotifyPropertyChanged {
        public String? Name {
            get;
            set {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    sealed class Recorder {
        public Recorder(ObservableList<Int32> list) {
            list.CollectionChanged += (_, e) => {
                Order.Add("collection");
                Events.Add(e);
            };
            list.PropertyChanged += (_, e) => {
                Order.Add(e.PropertyName ?? String.Empty);
                Properties.Add(e.PropertyName ?? String.Empty);
            };
        }

        public List<NotifyCollectionChangedEventArgs> Events { get; } = [];
        public List<String> Properties { get; } = [];
        public List<String> Order { get; } = [];
    }

    static ObservableList<Int32> create(params Int32[] items) {
        return new ObservableList<Int32>(items);
    }

    #region Single item operations

    [TestMethod]
    public void AddRaisesAddWithIndex() {
        ObservableList<Int32> list = create(1, 2);
        var rec = new Recorder(list);

        list.Add(3);

        Assert.HasCount(1, rec.Events);
        Assert.AreEqual(NotifyCollectionChangedAction.Add, rec.Events[0].Action);
        Assert.AreEqual(2, rec.Events[0].NewStartingIndex);
        Assert.AreEqual(3, rec.Events[0].NewItems![0]);
    }

    [TestMethod]
    public void InsertRaisesAddWithIndex() {
        ObservableList<Int32> list = create(1, 3);
        var rec = new Recorder(list);

        list.Insert(1, 2);

        Assert.AreSequenceEqual(new[] { 1, 2, 3 }, list);
        Assert.AreEqual(NotifyCollectionChangedAction.Add, rec.Events[0].Action);
        Assert.AreEqual(1, rec.Events[0].NewStartingIndex);
    }

    [TestMethod]
    public void RemoveAtRaisesRemoveWithIndex() {
        ObservableList<Int32> list = create(1, 2, 3);
        var rec = new Recorder(list);

        list.RemoveAt(1);

        Assert.AreSequenceEqual(new[] { 1, 3 }, list);
        Assert.AreEqual(NotifyCollectionChangedAction.Remove, rec.Events[0].Action);
        Assert.AreEqual(1, rec.Events[0].OldStartingIndex);
        Assert.AreEqual(2, rec.Events[0].OldItems![0]);
    }

    [TestMethod]
    public void RemoveReturnsTrueAndNotifiesWhenFound() {
        ObservableList<Int32> list = create(1, 2, 3);
        var rec = new Recorder(list);

        Boolean result = list.Remove(2);

        Assert.IsTrue(result);
        Assert.HasCount(1, rec.Events);
        Assert.AreEqual(1, rec.Events[0].OldStartingIndex);
    }

    [TestMethod]
    public void RemoveReturnsFalseAndDoesNotNotifyWhenMissing() {
        ObservableList<Int32> list = create(1, 2, 3);
        var rec = new Recorder(list);

        Boolean result = list.Remove(42);

        Assert.IsFalse(result);
        Assert.IsEmpty(rec.Events);
        Assert.IsEmpty(rec.Properties);
    }

    [TestMethod]
    public void IndexerSetRaisesReplaceWithIndex() {
        ObservableList<Int32> list = create(1, 2, 3);
        var rec = new Recorder(list);

        list[1] = 20;

        Assert.AreSequenceEqual(new[] { 1, 20, 3 }, list);
        Assert.AreEqual(NotifyCollectionChangedAction.Replace, rec.Events[0].Action);
        Assert.AreEqual(1, rec.Events[0].NewStartingIndex);
        Assert.AreEqual(20, rec.Events[0].NewItems![0]);
        Assert.AreEqual(2, rec.Events[0].OldItems![0]);
    }

    [TestMethod]
    public void ClearRaisesReset() {
        ObservableList<Int32> list = create(1, 2);
        var rec = new Recorder(list);

        list.Clear();

        Assert.IsEmpty(list);
        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[0].Action);
    }

    [TestMethod]
    public void ClearOnEmptyListDoesNotNotify() {
        ObservableList<Int32> list = create();
        var rec = new Recorder(list);

        list.Clear();

        Assert.IsEmpty(rec.Events);
    }

    #endregion

    #region Range operations

    [TestMethod]
    public void AddRangeOfManyRaisesSingleReset() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);

        list.AddRange([2, 3, 4]);

        Assert.AreSequenceEqual(new[] { 1, 2, 3, 4 }, list);
        Assert.HasCount(1, rec.Events);
        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[0].Action);
    }

    [TestMethod]
    public void AddRangeOfOneRaisesPreciseAdd() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);

        list.AddRange([2]);

        Assert.AreEqual(NotifyCollectionChangedAction.Add, rec.Events[0].Action);
        Assert.AreEqual(1, rec.Events[0].NewStartingIndex);
    }

    [TestMethod]
    public void EmptyRangeDoesNotNotify() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);

        list.AddRange([]);
        list.InsertRange(0, []);
        list.RemoveRange(0, 0);

        Assert.IsEmpty(rec.Events);
        Assert.IsEmpty(rec.Properties);
    }

    [TestMethod]
    public void InsertRangeInsertsAtIndexAndMaterializesSourceOnce() {
        ObservableList<Int32> list = create(1, 5);
        Int32 enumerations = 0;
        IEnumerable<Int32> Source() {
            enumerations++;
            yield return 2;
            yield return 3;
            yield return 4;
        }

        list.InsertRange(1, Source());

        Assert.AreSequenceEqual(new[] { 1, 2, 3, 4, 5 }, list);
        Assert.AreEqual(1, enumerations);
    }

    [TestMethod]
    public void RemoveRangeOfManyRaisesSingleReset() {
        ObservableList<Int32> list = create(1, 2, 3, 4);
        var rec = new Recorder(list);

        list.RemoveRange(1, 2);

        Assert.AreSequenceEqual(new[] { 1, 4 }, list);
        Assert.HasCount(1, rec.Events);
        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[0].Action);
    }

    [TestMethod]
    public void RemoveRangeOfOneRaisesPreciseRemove() {
        ObservableList<Int32> list = create(1, 2, 3);
        var rec = new Recorder(list);

        list.RemoveRange(1, 1);

        Assert.AreEqual(NotifyCollectionChangedAction.Remove, rec.Events[0].Action);
        Assert.AreEqual(1, rec.Events[0].OldStartingIndex);
    }

    [TestMethod]
    public void RemoveAllReturnsCountAndRaisesSingleReset() {
        ObservableList<Int32> list = create(1, 2, 3, 4);
        var rec = new Recorder(list);

        Int32 removed = list.RemoveAll(x => x % 2 == 0);

        Assert.AreEqual(2, removed);
        Assert.AreSequenceEqual(new[] { 1, 3 }, list);
        Assert.HasCount(1, rec.Events);
        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[0].Action);
    }

    [TestMethod]
    public void RemoveAllWithoutMatchesDoesNotNotify() {
        ObservableList<Int32> list = create(1, 3);
        var rec = new Recorder(list);

        Int32 removed = list.RemoveAll(x => x % 2 == 0);

        Assert.AreEqual(0, removed);
        Assert.IsEmpty(rec.Events);
    }

    [TestMethod]
    public void SortAndReverseRaiseReset() {
        ObservableList<Int32> list = create(3, 1, 2);
        var rec = new Recorder(list);

        list.Sort();
        list.Reverse();

        Assert.AreSequenceEqual(new[] { 3, 2, 1 }, list);
        Assert.HasCount(2, rec.Events);
        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[0].Action);
        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[1].Action);
    }

    [TestMethod]
    public void RangeArgumentsAreValidated() {
        ObservableList<Int32> list = create(1);

        Assert.ThrowsExactly<ArgumentNullException>(() => list.AddRange(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => list.RemoveAll(null!));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => list.InsertRange(5, [1]));
        Assert.ThrowsExactly<ArgumentException>(() => list.RemoveRange(0, 5));
    }

    #endregion

    #region Silent operations

    [TestMethod]
    public void SilentOperationsDoNotNotify() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);

        list.AddRangeSilent([2, 3]);
        list.InsertRangeSilent(0, [0]);
        list.RemoveRangeSilent(0, 2);

        Assert.AreSequenceEqual(new[] { 2, 3 }, list);
        Assert.IsEmpty(rec.Events);
        Assert.IsEmpty(rec.Properties);
    }

    [TestMethod]
    public void ForceUpdateRaisesReset() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);

        list.ForceUpdate();

        Assert.AreEqual(NotifyCollectionChangedAction.Reset, rec.Events[0].Action);
    }

    [TestMethod]
    public void NonObservingListDoesNotNotify() {
        var list = new ObservableList<Int32>(observe: false);
        var rec = new Recorder(list);

        list.Add(1);
        list.AddRange([2, 3]);
        list.Clear();

        Assert.IsEmpty(rec.Events);
        Assert.IsEmpty(rec.Properties);
    }

    #endregion

    #region Property notifications

    [TestMethod]
    public void CountAndIndexerAreRaisedBeforeCollectionChanged() {
        ObservableList<Int32> list = create();
        var rec = new Recorder(list);

        list.Add(1);

        Assert.AreSequenceEqual(new[] { "Count", "Item[]", "collection" }, rec.Order);
    }

    [TestMethod]
    public void ReplaceDoesNotRaiseCount() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);

        list[0] = 2;

        Assert.DoesNotContain("Count", rec.Properties);
        Assert.Contains("Item[]", rec.Properties);
    }

    #endregion

    #region Child tracking

    static ObservableList<Child> trackChanges(out List<ItemPropertyChangedEventArgs<Child>> events) {
        var list = new ObservableList<Child>();
        var captured = new List<ItemPropertyChangedEventArgs<Child>>();
        list.ItemPropertyChanged += (_, e) => captured.Add(e);
        events = captured;

        return list;
    }

    [TestMethod]
    public void ChildChangeRaisesItemPropertyChangedWithItemAndName() {
        ObservableList<Child> list = trackChanges(out var events);
        var child = new Child();
        list.Add(child);

        child.Name = "x";

        Assert.HasCount(1, events);
        Assert.AreSame(child, events[0].Item);
        Assert.AreEqual(nameof(Child.Name), events[0].PropertyName);
    }

    [TestMethod]
    public void ChildChangeDoesNotRaiseCollectionChanged() {
        var list = new ObservableList<Child>();
        var child = new Child();
        list.Add(child);
        Int32 collectionEvents = 0;
        list.CollectionChanged += (_, _) => collectionEvents++;

        child.Name = "x";

        Assert.AreEqual(0, collectionEvents);
    }

    [TestMethod]
    public void InsertTracksChild() {
        ObservableList<Child> list = trackChanges(out var events);
        var child = new Child();
        list.Insert(0, child);

        child.Name = "x";

        Assert.HasCount(1, events);
    }

    [TestMethod]
    public void RangeAndSilentOperationsTrackChildren() {
        ObservableList<Child> list = trackChanges(out var events);
        var a = new Child();
        var b = new Child();
        var c = new Child();
        list.AddRange([a]);
        list.AddRangeSilent([b]);
        list.InsertRangeSilent(0, [c]);

        a.Name = "1";
        b.Name = "2";
        c.Name = "3";

        Assert.HasCount(3, events);
    }

    [TestMethod]
    public void RemovedChildrenAreNoLongerTracked() {
        ObservableList<Child> list = trackChanges(out var events);
        var a = new Child();
        var b = new Child();
        var c = new Child();
        var d = new Child();
        list.AddRange([a, b, c, d]);

        list.Remove(a);
        list.RemoveAt(0);
        list.RemoveRangeSilent(0, 1);
        list.RemoveAll(x => x == d);
        a.Name = "x";
        b.Name = "x";
        c.Name = "x";
        d.Name = "x";

        Assert.IsEmpty(events);
    }

    [TestMethod]
    public void ClearUntracksChildren() {
        ObservableList<Child> list = trackChanges(out var events);
        var child = new Child();
        list.Add(child);

        list.Clear();
        child.Name = "x";

        Assert.IsEmpty(events);
    }

    [TestMethod]
    public void IndexerSetSwapsTrackedChild() {
        ObservableList<Child> list = trackChanges(out var events);
        var oldChild = new Child();
        var newChild = new Child();
        list.Add(oldChild);

        list[0] = newChild;
        oldChild.Name = "x";
        newChild.Name = "x";

        Assert.HasCount(1, events);
        Assert.AreSame(newChild, events[0].Item);
    }

    [TestMethod]
    public void ChildrenAreNotTrackedWhenDisabled() {
        var list = new ObservableList<Child>(observeChild: false);
        Int32 count = 0;
        list.ItemPropertyChanged += (_, _) => count++;
        var child = new Child();
        list.Add(child);

        child.Name = "x";

        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public void FailedMutationDoesNotLeakSubscription() {
        ObservableList<Child> list = trackChanges(out var events);
        var child = new Child();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => list.Insert(5, child));
        child.Name = "x";

        Assert.IsEmpty(events);
    }

    #endregion

    #region Non-generic IList

    [TestMethod]
    public void NonGenericAddNotifiesAndReturnsIndex() {
        ObservableList<Int32> list = create(1);
        var rec = new Recorder(list);
        var nonGeneric = (IList)list;

        Int32 index = nonGeneric.Add(2);

        Assert.AreEqual(1, index);
        Assert.AreEqual(NotifyCollectionChangedAction.Add, rec.Events[0].Action);
    }

    [TestMethod]
    public void NonGenericMembersWork() {
        ObservableList<Int32> list = create(1, 2);
        var nonGeneric = (IList)list;

        nonGeneric.Insert(0, 0);
        nonGeneric.Remove(2);
        nonGeneric[0] = 10;

        Assert.AreSequenceEqual(new[] { 10, 1 }, list);
        Assert.IsTrue(nonGeneric.Contains(1));
        Assert.AreEqual(1, nonGeneric.IndexOf(1));
        Assert.IsFalse(nonGeneric.Contains("text"));
        Assert.AreEqual(-1, nonGeneric.IndexOf("text"));
        Assert.IsFalse(nonGeneric.IsReadOnly);
        Assert.IsFalse(nonGeneric.IsFixedSize);
    }

    [TestMethod]
    public void NonGenericAddRejectsIncompatibleValue() {
        var nonGeneric = (IList)create();

        Assert.ThrowsExactly<ArgumentException>(() => nonGeneric.Add("text"));
        Assert.ThrowsExactly<ArgumentException>(() => nonGeneric.Add(null));
    }

    [TestMethod]
    public void NonGenericNullIsAcceptedForReferenceTypes() {
        var list = new ObservableList<String?>();
        var nonGeneric = (IList)list;

        nonGeneric.Add(null);

        Assert.HasCount(1, list);
        Assert.IsNull(list[0]);
    }

    [TestMethod]
    public void NonGenericCopyToWorks() {
        ObservableList<Int32> list = create(1, 2);
        var array = new Int32[3];

        ((ICollection)list).CopyTo(array, 1);

        Assert.AreSequenceEqual(new[] { 0, 1, 2 }, array);
    }

    #endregion
}
