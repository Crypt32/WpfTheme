using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    public abstract class CommandBase<T> : ICommand where T : class {
        readonly Predicate<T> _canExecute;
        List<WeakReference> canExecuteChangedHandlers;

        protected CommandBase(Predicate<T> canExecute = null) {
            _canExecute = canExecute;
            CommandManagerHelper.RemoveHandlersFromRequerySuggested(canExecuteChangedHandlers);
        }

        void OnCanExecuteChanged() {
            // we may not be on the UI thread, and things are unhappy when they aren't on the UI thread, so make sure we are on the UI thread
            Debug.Assert(Application.Current != null);
            if (!Application.Current.Dispatcher.CheckAccess()) {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)OnCanExecuteChanged);
            } else {
                CommandManagerHelper.CallWeakReferenceHandlers(canExecuteChangedHandlers);
            }
            CommandManager.InvalidateRequerySuggested();
        }
        protected void RaiseCanExecuteChanged() {
            OnCanExecuteChanged();
        }
        public Boolean CanExecute(Object parameter) {
            return _canExecute == null || _canExecute(parameter as T);
        }
        public abstract void Execute(Object parameter);

        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
                CommandManagerHelper.AddWeakReferenceHandler(ref canExecuteChangedHandlers, value, 2);
            }
            remove {
                CommandManager.RequerySuggested -= value;
                CommandManagerHelper.RemoveWeakReferenceHandler(canExecuteChangedHandlers, value);
            }
        }
    }
}
