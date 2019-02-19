using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    sealed class CancelAsyncCommand : ICommand {
        CancellationTokenSource cts = new CancellationTokenSource();
        Boolean commandExecuting;
        List<WeakReference> canExecuteChangedHandlers;

        public CancellationToken Token => cts.Token;
        public void NotifyCommandStarting() {
            commandExecuting = true;
            if (!cts.IsCancellationRequested) {
                return;
            }
            cts = new CancellationTokenSource();
            RaiseCanExecuteChanged();
        }
        public void NotifyCommandFinished() {
            commandExecuting = false;
            RaiseCanExecuteChanged();
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
        Boolean ICommand.CanExecute(Object parameter) {
            return commandExecuting && !cts.IsCancellationRequested;
        }
        void ICommand.Execute(Object parameter) {
            cts.Cancel();
            RaiseCanExecuteChanged();
        }
        public void RaiseCanExecuteChanged() {
            OnCanExecuteChanged();
        }

        // events
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
