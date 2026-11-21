using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SysadminsLV.WPF.Mvvm.Commands;

// Credits: http://www.blogs.intuidev.com/post/2010/post/2010/01/25/TabControlStyling_PartOne.aspx

namespace SysadminsLV.WPF.Controls;

public class ClosableTabControl : TabControl {
    static ClosableTabControl() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ClosableTabControl), new FrameworkPropertyMetadata(typeof(ClosableTabControl)));
    }
    public ClosableTabControl() {
        MenuCommand = new RelayCommand(switchTab);
    }

    #region AddTabCommand

    public static readonly DependencyProperty AddTabCommandProperty = DependencyProperty.Register(
        nameof(AddTabCommand),
        typeof(ICommand),
        typeof(ClosableTabControl),
        new PropertyMetadata(null));
    public ICommand AddTabCommand {
        get => (ICommand)GetValue(AddTabCommandProperty);
        set => SetValue(AddTabCommandProperty, value);
    }

    #endregion

    #region CloseTabCommand

    public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register(
        nameof(CloseTabCommand),
        typeof(ICommand),
        typeof(ClosableTabControl),
        new PropertyMetadata(null));
    public ICommand CloseTabCommand {
        get => (ICommand)GetValue(CloseTabCommandProperty);
        set => SetValue(CloseTabCommandProperty, value);
    }

    #endregion

    #region MenuCommand

    public static readonly DependencyProperty MenuCommandProperty = DependencyProperty.Register(
        nameof(MenuCommand),
        typeof(ICommand),
        typeof(ClosableTabControl),
        new PropertyMetadata(null));
        
    public ICommand MenuCommand {
        get => (ICommand)GetValue(MenuCommandProperty);
        set => SetValue(MenuCommandProperty, value);
    }

    #endregion

    #region MenuDisplayMemberPath

    public static readonly DependencyProperty MenuDisplayMemberPathProperty = DependencyProperty.Register(
        nameof(MenuDisplayMemberPath),
        typeof(String),
        typeof(ClosableTabControl),
        new PropertyMetadata("Header"));
    /// <summary>
    /// Gets or sets the path to the property of a tab item (or bound view model item) that is displayed in the
    /// tab list menu. Default is <c>Header</c>.
    /// </summary>
    public String MenuDisplayMemberPath {
        get => (String)GetValue(MenuDisplayMemberPathProperty);
        set => SetValue(MenuDisplayMemberPathProperty, value);
    }

    #endregion

    #region ShowNewTabButton

    public static readonly DependencyProperty ShowNewTabButtonProperty = DependencyProperty.Register(
        nameof(ShowNewTabButton),
        typeof(Boolean),
        typeof(ClosableTabControl),
        new PropertyMetadata(default));
    public Boolean ShowNewTabButton {
        get => (Boolean)GetValue(ShowNewTabButtonProperty);
        set => SetValue(ShowNewTabButtonProperty, value);
    }

    #endregion

    void switchTab(Object? o) {
        if (o != null && Items.Contains(o)) {
            SelectedItem = o;
        }
    }

    protected override DependencyObject GetContainerForItemOverride() {
        return new ClosableTabItem { IsClosable = true };
    }
    protected override Boolean IsItemItsOwnContainerOverride(Object item) {
        return item is ClosableTabItem;
    }
}