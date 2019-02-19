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
        public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register(
            nameof(CloseTabCommand),
            typeof(ICommand),
            typeof(ClosableTabControl),
            new PropertyMetadata(null, CloseCommandChanged));
        public static readonly DependencyProperty AddTabCommandProperty = DependencyProperty.Register(
            nameof(AddTabCommand),
            typeof(ICommand),
            typeof(ClosableTabControl),
            new PropertyMetadata(null, AddCommandChanged));

        static void CloseCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cs = (ClosableTabControl)d;
            cs.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }
        static void AddCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cs = (ClosableTabControl)d;
            cs.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
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
        public class TabItems : ObservableCollection<TabItem> {
            public IList<TabItem> MyItems {
                get => Items;
                set {
                    foreach (TabItem item in value) {
                        Items.Add(item);
                    }
                }
            }
        }
        public ICommand AddTabCommand {
            get => (ICommand)GetValue(AddTabCommandProperty);
            set => SetValue(AddTabCommandProperty, value);
        }
        public ICommand CloseTabCommand {
            get => (ICommand)GetValue(CloseTabCommandProperty);
            set => SetValue(CloseTabCommandProperty, value);
        }
        ICommand Command { get; set; }
        Object CommandParameter { get; set; }
        IInputElement CommandTarget { get; set; }
    }
}
