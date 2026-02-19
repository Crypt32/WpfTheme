using System;
using System.Threading;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

/// <summary>
/// Represents a command that supports asynchronous cancellation and execution tracking.
/// </summary>
/// <remarks>
/// This class is designed to manage the lifecycle of asynchronous operations, providing mechanisms
/// to cancel ongoing tasks and track whether the command is currently executing. It implements
/// the <see cref="ICommand"/> interface for integration with WPF command bindings and the
/// <see cref="IDisposable"/> interface for proper resource cleanup.
/// </remarks>
sealed class CancelAsyncCommand : ICommand, IDisposable {
    CancellationTokenSource cts = new();
    Boolean disposed;
    EventHandler? canExecuteChanged;

    /// <summary>
    /// Gets the <see cref="CancellationToken"/> associated with the current command.
    /// </summary>
    /// <remarks>
    /// This token is used to signal cancellation requests for asynchronous operations managed by the command.
    /// It is refreshed whenever a new execution starts to ensure proper lifecycle management of tasks.
    /// </remarks>
    public CancellationToken Token => cts.Token;
    /// <summary>
    /// Gets a value indicating whether the command is currently executing an asynchronous operation.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the command is executing; otherwise, <see langword="false"/>.
    /// </value>
    /// <remarks>
    /// This property is used to track the execution state of the command. It is updated automatically
    /// when the command starts or finishes execution, and it can be used to disable UI elements
    /// bound to the command while it is running.
    /// </remarks>
    public Boolean IsExecuting { get; private set; }

    /// <summary>
    /// Notifies that the command is starting execution.
    /// </summary>
    /// <remarks>
    /// This method ensures that the command is not executed concurrently by checking the <see cref="IsExecuting"/> property.
    /// If the command is already executing, the method returns immediately. Otherwise, it resets the 
    /// <see cref="CancellationTokenSource"/> if a cancellation was previously requested and sets the <see cref="IsExecuting"/> 
    /// property to <c>true</c>. Additionally, it raises the <see cref="CanExecuteChanged"/> event to update the command's state.
    /// </remarks>
    public void NotifyCommandStarting() {
        // Reject new executions while running (button will be disabled)
        if (IsExecuting) {
            return; // Do nothing
        }

        // Always create fresh CTS for new execution
        if (cts.IsCancellationRequested) {
            cts.Dispose();
            cts = new CancellationTokenSource();
        }

        IsExecuting = true;
        RaiseCanExecuteChanged();
    }
    /// <summary>
    /// Notifies that the command has finished execution.
    /// </summary>
    /// <remarks>
    /// This method is used to reset the execution state of the command. It sets the <see cref="IsExecuting"/> property
    /// to <c>false</c> and raises the <see cref="ICommand.CanExecuteChanged"/> event to update the command's availability.
    /// </remarks>
    public void NotifyCommandFinished() {
        IsExecuting = false;
        RaiseCanExecuteChanged();
    }

    Boolean ICommand.CanExecute(Object? parameter) {
        return IsExecuting && !cts.IsCancellationRequested;
    }

    void ICommand.Execute(Object? parameter) {
        cts.Cancel();
        RaiseCanExecuteChanged();
    }
    /// <summary>
    /// Raises the <see cref="ICommand.CanExecuteChanged"/> event.
    /// </summary>
    /// <remarks>
    /// This method notifies subscribers that the command's execution status has changed, 
    /// prompting the UI to re-evaluate the command's availability. It is typically called 
    /// after changes to the command's state, such as when <see cref="IsExecuting"/> is updated.
    /// </remarks>
    public void RaiseCanExecuteChanged() {
        canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged {
        add => canExecuteChanged += value;
        remove => canExecuteChanged -= value;
    }

    /// <inheritdoc />
    public void Dispose() {
        if (disposed) {
            return;
        }
        disposed = true;
        cts.Dispose();
        canExecuteChanged = null;
    }
}
