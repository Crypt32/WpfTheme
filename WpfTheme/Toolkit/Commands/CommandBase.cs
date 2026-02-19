using System;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

/// <summary>
/// Represents an abstract base class for implementing commands in WPF applications.
/// </summary>
/// <typeparam name="T">
/// The type of the parameter passed to the command. Must be a reference type.
/// </typeparam>
/// <param name="canExecute">
/// A predicate that determines whether the command can execute. If <see langword="null"/>, the command can always execute.
/// </param>
/// <param name="UseCommandManager">
/// A value indicating whether the command should use the <see cref="CommandManager"/> for managing
/// the <see cref="CanExecuteChanged"/> event.
/// </param>
public abstract class CommandBase<T>(Predicate<T>? canExecute = null, Boolean UseCommandManager = false) : ICommand
    where T : class {
    EventHandler? canExecuteChanged;

    /// <summary>
    /// Raises the <see cref="CanExecuteChanged"/> event to indicate that the result of the <see cref="CanExecute"/> method
    /// may have changed.
    /// </summary>
    /// <remarks>
    /// This method is typically called to notify the command manager or any subscribers that the command's ability
    /// to execute has changed. If <see cref="UseCommandManager"/> is <see langword="true"/>, the 
    /// <see cref="CommandManager"/> will automatically handle this event.
    /// </remarks>
    protected void RaiseCanExecuteChanged() {
        canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public abstract void Execute(Object parameter);
    /// <inheritdoc />
    public virtual Boolean CanExecute(Object parameter) {
        return canExecute is null || canExecute(parameter as T);
    }

    /// <inheritdoc />
    public event EventHandler CanExecuteChanged {
        add {
            // Only use CommandManager if explicitly requested
            if (UseCommandManager && canExecute is not null) {
                CommandManager.RequerySuggested += value;
            }
            canExecuteChanged += value;
        }
        remove {
            if (UseCommandManager && canExecute is not null) {
                CommandManager.RequerySuggested -= value;
            }
            canExecuteChanged -= value;
        }
    }
}
