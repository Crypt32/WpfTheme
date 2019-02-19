using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    public class AsyncCommand : CommandBase<Object>, IAsyncCommand {
        readonly Func<CancellationToken, Object, Task> _command;
        readonly CancelAsyncCommand _cancelCommand;
        NotifyTaskCompletion execution;
        public AsyncCommand(Func<CancellationToken, Object, Task> command, Predicate<Object> canExecute = null)
            : base(canExecute) {
            _command = command;
            _cancelCommand = new CancelAsyncCommand();
        }
        public async Task ExecuteAsync(Object parameter) {
            _cancelCommand.NotifyCommandStarting();
            execution = new NotifyTaskCompletion(_command(_cancelCommand.Token, parameter));
            RaiseCanExecuteChanged();
            await execution.Task;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }
        public ICommand CancelCommand => _cancelCommand;
        public override async void Execute(Object parameter) {
            await ExecuteAsync(parameter);
        }
    }
    public class AsyncCommand<TResult> : CommandBase<Object>, IAsyncCommand {
        readonly Func<CancellationToken, Object, Task<TResult>> _command;
        readonly CancelAsyncCommand _cancelCommand;
        NotifyTaskCompletion<TResult> execution;
        public AsyncCommand(Func<CancellationToken, Object, Task<TResult>> command, Predicate<Object> canExecute = null)
            : base(canExecute) {
            _command = command;
            _cancelCommand = new CancelAsyncCommand();
        }
        public async Task ExecuteAsync(Object parameter) {
            _cancelCommand.NotifyCommandStarting();
            execution = new NotifyTaskCompletion<TResult>(_command(_cancelCommand.Token, parameter), default(TResult));
            RaiseCanExecuteChanged();
            await execution.Task;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }
        public ICommand CancelCommand => _cancelCommand;
        public override async void Execute(Object parameter) {
            await ExecuteAsync(parameter);
        }
    }
}
