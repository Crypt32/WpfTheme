using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

// Credits: http://www.blogs.intuidev.com/post/2010/post/2010/01/25/TabControlStyling_PartOne.aspx

namespace SysadminsLV.WPF.OfficeTheme.Controls {
    public class ClosableTabControl : TabControl {
        #region AddTabCommand

        public static readonly DependencyProperty AddTabCommandProperty = DependencyProperty.Register(
            nameof(AddTabCommand),
            typeof(ICommand),
            typeof(ClosableTabControl),
            new PropertyMetadata(null, AddCommandChanged));
        static void AddCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cs = (ClosableTabControl)d;
            cs.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }
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
            new PropertyMetadata(null, CloseCommandChanged));
        static void CloseCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cs = (ClosableTabControl)d;
            cs.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }
        public ICommand CloseTabCommand {
            get => (ICommand)GetValue(CloseTabCommandProperty);
            set => SetValue(CloseTabCommandProperty, value);
        }

        #endregion

        public static readonly DependencyProperty ShowNewTabButtonProperty = DependencyProperty.Register(
            nameof(ShowNewTabButton),
            typeof(Boolean),
            typeof(ClosableTabControl),
            new PropertyMetadata(default));
        public Boolean ShowNewTabButton {
            get => (Boolean)GetValue(AddTabCommandProperty);
            set => SetValue(AddTabCommandProperty, value);
        }

        void HookUpCommand(ICommand oldCommand, ICommand newCommand) {
            // If oldCommand is not null, then we need to remove the handlers. 
            if (oldCommand != null) {
                RemoveCommand(oldCommand);
            }
            AddCommand(newCommand);
        }
        void RemoveCommand(ICommand oldCommand) {
            EventHandler handler = CanExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }
        void CanExecuteChanged(Object sender, EventArgs e) {
            if (Command != null) {
                var command = Command as RoutedCommand;
                // If a RoutedCommand. 
                IsEnabled = command?.CanExecute(CommandParameter, CommandTarget) ?? Command.CanExecute(CommandParameter);
            }
        }

        // Add the command. 
        void AddCommand(ICommand newCommand) {
            EventHandler handler = CanExecuteChanged;
            if (newCommand != null) {
                newCommand.CanExecuteChanged += handler;
            }
        }
        [ContentProperty("ItemsSource")]
        public class TabItems : ObservableCollection<ClosableTabItem> {
            public IList<ClosableTabItem> MyItems {
                get => Items;
                set {
                    foreach (ClosableTabItem item in value) {
                        Items.Add(item);
                    }
                }
            }
        }

        ICommand Command { get; set; }
        Object CommandParameter { get; set; }
        IInputElement CommandTarget { get; set; }

        protected override DependencyObject GetContainerForItemOverride() {
            return new ClosableTabItem { IsClosable = true };
        }
        protected override Boolean IsItemItsOwnContainerOverride(Object item) {
            return item is ClosableTabItem;
        }
    }
}
