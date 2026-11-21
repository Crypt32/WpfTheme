using System;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysadminsLV.WPF.Controls;

namespace SysadminsLV.WPF.Tests.Controls;

[TestClass]
public sealed class ClosableTabControlTests {
    [STATestMethod]
    public void AssigningCommandsDoesNotSubscribeToThem() {
        // a handler on a long-lived command would keep the control alive
        var add = new CountingCommand();
        var close = new CountingCommand();
        var tabs = new ClosableTabControl { AddTabCommand = add, CloseTabCommand = close };

        Assert.AreEqual(0, add.Subscribers);
        Assert.AreEqual(0, close.Subscribers);
        GC.KeepAlive(tabs);
    }

    [STATestMethod]
    public void ReplacingCommandsDoesNotSubscribeToThem() {
        var first = new CountingCommand();
        var second = new CountingCommand();
        var tabs = new ClosableTabControl { AddTabCommand = first };
        tabs.AddTabCommand = second;

        Assert.AreEqual(0, first.Subscribers);
        Assert.AreEqual(0, second.Subscribers);
    }

    [STATestMethod]
    public void MenuCommandSelectsTheGivenTab() {
        var first = new ClosableTabItem();
        var second = new ClosableTabItem();
        var tabs = new ClosableTabControl();
        tabs.Items.Add(first);
        tabs.Items.Add(second);
        tabs.SelectedItem = first;

        tabs.MenuCommand.Execute(second);

        Assert.AreSame(second, tabs.SelectedItem);
    }

    [STATestMethod]
    public void MenuCommandIgnoresNullAndForeignItems() {
        var first = new ClosableTabItem();
        var tabs = new ClosableTabControl();
        tabs.Items.Add(first);
        tabs.SelectedItem = first;

        tabs.MenuCommand.Execute(null);
        tabs.MenuCommand.Execute(new ClosableTabItem());

        Assert.AreSame(first, tabs.SelectedItem);
    }

    [STATestMethod]
    public void MenuDisplayMemberPathDefaultsToHeader() {
        Assert.AreEqual("Header", new ClosableTabControl().MenuDisplayMemberPath);
    }

    sealed class CountingCommand : ICommand {
        EventHandler? _canExecuteChanged;

        public Int32 Subscribers => _canExecuteChanged?.GetInvocationList().Length ?? 0;

        public event EventHandler? CanExecuteChanged {
            add => _canExecuteChanged += value;
            remove => _canExecuteChanged -= value;
        }

        public Boolean CanExecute(Object? parameter) => true;
        public void Execute(Object? parameter) { }
    }
}
