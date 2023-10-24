using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

// Credits: http://www.blogs.intuidev.com/post/2010/post/2010/01/25/TabControlStyling_PartOne.aspx

namespace SysadminsLV.WPF.OfficeTheme.Controls {
    public class ClosableTabControl : TabControl {
        public ClosableTabControl() {
            MenuCommand = new RelayCommand(switchTab);
        }

        #region AddTabCommand

        public static readonly DependencyProperty AddTabCommandProperty = DependencyProperty.Register(
            nameof(AddTabCommand),
            typeof(ICommand),
            typeof(ClosableTabControl),
            new PropertyMetadata(null, commandChanged));
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
            new PropertyMetadata(null, commandChanged));
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
            new PropertyMetadata(null, commandChanged));
        
        public ICommand MenuCommand {
            get => (ICommand)GetValue(MenuCommandProperty);
            set => SetValue(MenuCommandProperty, value);
        }

        #endregion

        #region ShowNewTabButton

        public static readonly DependencyProperty ShowNewTabButtonProperty = DependencyProperty.Register(
            nameof(ShowNewTabButton),
            typeof(Boolean),
            typeof(ClosableTabControl),
            new PropertyMetadata(default));
        public Boolean ShowNewTabButton {
            get => (Boolean)GetValue(AddTabCommandProperty);
            set => SetValue(AddTabCommandProperty, value);
        }

        #endregion

        static void commandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cs = (ClosableTabControl)d;
            cs.hookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }
        void hookUpCommand(ICommand oldCommand, ICommand newCommand) {
            // If oldCommand is not null, then we need to remove the handlers. 
            if (oldCommand != null) {
                removeCommand(oldCommand);
            }
            addCommand(newCommand);
        }
        void addCommand(ICommand newCommand) {
            EventHandler handler = canExecuteChanged;
            if (newCommand != null) {
                newCommand.CanExecuteChanged += handler;
            }
        }
        void removeCommand(ICommand oldCommand) {
            EventHandler handler = canExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }
        void canExecuteChanged(Object sender, EventArgs e) {
            if (Command != null) {
                var command = Command as RoutedCommand;
                // If a RoutedCommand. 
                IsEnabled = command?.CanExecute(CommandParameter, CommandTarget) ?? Command.CanExecute(CommandParameter);
            }
        }

        ICommand Command { get; set; }
        Object CommandParameter { get; set; }
        IInputElement CommandTarget { get; set; }

        void switchTab(Object o) {
            if (o == null) {
                return;
            }
            foreach (Object item in Items) {
                if (item == o) {
                    SelectedItem = item;
                    return;
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride() {
            return new ClosableTabItem { IsClosable = true };
        }
        protected override Boolean IsItemItsOwnContainerOverride(Object item) {
            return item is ClosableTabItem;
        }
    }
}
