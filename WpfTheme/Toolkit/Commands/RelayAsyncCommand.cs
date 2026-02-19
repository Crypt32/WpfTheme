using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

/// <summary>
/// Represents an asynchronous command that can be executed in WPF applications. 
/// This class provides support for executing asynchronous operations while managing 
/// cancellation and execution state.
/// </summary>
/// <remarks>
/// The <see cref="AsyncCommand"/> class is designed to simplify the implementation of 
/// asynchronous commands in WPF applications. It supports cancellation through a 
/// <see cref="CancelCommand"/> and ensures that the command cannot be executed while 
/// an operation is already in progress.
/// </remarks>
/// <example>
/// The following example demonstrates how to create and use an <see cref="AsyncCommand"/>:
/// <code>
/// var asyncCommand = new AsyncCommand(async (parameter, token) => {
///     await SomeAsyncOperation(parameter, token);
/// });
/// </code>
/// </example>
public class AsyncCommand : CommandBase<Object>, IAsyncCommand, IDisposable {
    readonly Func<Object, CancellationToken, Task> _command;
    readonly CancelAsyncCommand _cancelCommand;
    NotifyTaskCompletion? execution;

    public AsyncCommand(Func<Object, CancellationToken, Task> command, Predicate<Object>? canExecute = null)
        : base(canExecute) {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _cancelCommand = new CancelAsyncCommand();
    }

    /// <summary>
    /// Gets a command that allows cancellation of the currently executing asynchronous operation.
    /// </summary>
    /// <remarks>
    /// The <see cref="CancelCommand"/> property provides an <see cref="ICommand"/> implementation
    /// that can be used to signal cancellation of an ongoing asynchronous operation. This is
    /// particularly useful in scenarios where long-running tasks need to be interrupted by the user.
    /// </remarks>
    /// <value>
    /// An <see cref="ICommand"/> instance that supports cancellation of the asynchronous operation.
    /// </value>
    /// <example>
    /// The following example demonstrates how to bind the <see cref="CancelCommand"/> to a button
    /// in a WPF application:
    /// <code>
    /// <Button Command="{Binding AsyncCommand.CancelCommand}" Content="Cancel" />
    /// </code>
    /// </example>
    public ICommand CancelCommand => _cancelCommand;

    /// <inheritdoc />
    public override async void Execute(Object? parameter) {
        await ExecuteAsync(parameter);
    }
    /// <inheritdoc />
    public override Boolean CanExecute(Object? parameter) {
        // Disable command while executing
        return !_cancelCommand.IsExecuting && base.CanExecute(parameter);
    }
    /// <inheritdoc />
    public async Task ExecuteAsync(Object? parameter) {
        _cancelCommand.NotifyCommandStarting();
        execution?.Dispose();

        execution = new NotifyTaskCompletion(_command(parameter, _cancelCommand.Token));
        RaiseCanExecuteChanged();
        await execution.Task;
        _cancelCommand.NotifyCommandFinished();
        RaiseCanExecuteChanged();
    }

    /// <inheritdoc />
    public void Dispose() {
        _cancelCommand.Dispose();
    }
}

/// <summary>
/// Represents an asynchronous command that executes a task and supports cancellation.
/// </summary>
/// <typeparam name="TResult">
/// The type of the result returned by the asynchronous task executed by the command.
/// </typeparam>
/// <remarks>
/// This class is designed to facilitate the implementation of asynchronous commands in WPF applications.
/// It provides functionality for executing tasks asynchronously, determining whether the command can execute,
/// and managing cancellation of the task.
/// </remarks>
public class AsyncCommand<TResult> : CommandBase<Object>, IAsyncCommand, IDisposable {
    readonly Func<Object, CancellationToken, Task<TResult>> _command;
    readonly CancelAsyncCommand _cancelCommand;
    NotifyTaskCompletion<TResult>? execution;

    public AsyncCommand(Func<Object, CancellationToken, Task<TResult>> command, Predicate<Object>? canExecute = null)
        : base(canExecute) {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _cancelCommand = new CancelAsyncCommand();
    }

    /// <summary>
    /// Gets a command that can be used to cancel the execution of the asynchronous operation.
    /// </summary>
    /// <remarks>
    /// This property provides access to a cancellation command that allows the user to interrupt
    /// the execution of the asynchronous task associated with this command. It is particularly
    /// useful in scenarios where long-running tasks need to be stopped based on user interaction
    /// or other conditions.
    /// </remarks>
    public ICommand CancelCommand => _cancelCommand;

    /// <inheritdoc />
    public override async void Execute(Object? parameter) {
        await ExecuteAsync(parameter);
    }
    /// <inheritdoc />
    public override Boolean CanExecute(Object? parameter) {
        // Disable command while executing
        return !_cancelCommand.IsExecuting && base.CanExecute(parameter);
    }
    /// <inheritdoc />
    public async Task ExecuteAsync(Object? parameter) {
        _cancelCommand.NotifyCommandStarting();
        execution?.Dispose();

        execution = new NotifyTaskCompletion<TResult>(_command(parameter, _cancelCommand.Token), default);
        RaiseCanExecuteChanged();
        await execution.Task;
        _cancelCommand.NotifyCommandFinished();
        RaiseCanExecuteChanged();
    }

    /// <inheritdoc />
    public void Dispose() {
        _cancelCommand.Dispose();
    }
}
