using System;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

/// <summary>
/// Represents a command that can be executed with a parameter of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the parameter passed to the command. Must be a reference type.
/// </typeparam>
/// <remarks>
/// This class provides an implementation of the <see cref="CommandBase{T}"/> class, allowing you to define
/// the execution logic and optionally the conditions under which the command can execute.
/// </remarks>
public class RelayCommand<T> : CommandBase<T> where T : class {
    readonly Action<T> _execute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class with the specified execution logic,
    /// optional condition for execution, and an option to use the <see cref="CommandManager"/>.
    /// </summary>
    /// <param name="execute">
    /// The action to execute when the command is invoked. This parameter cannot be <see langword="null"/>.
    /// </param>
    /// <param name="canExecute">
    /// A predicate that determines whether the command can execute. If <see langword="null"/>, the command can always execute.
    /// </param>
    /// <param name="useCommandManager">
    /// A value indicating whether the command should use the <see cref="CommandManager"/> for managing
    /// the <see cref="ICommand.CanExecuteChanged"/> event.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="execute"/> parameter is <see langword="null"/>.
    /// </exception>
    public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null, Boolean useCommandManager = false)
        : base(canExecute, useCommandManager) {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    /// <inheritdoc />
    public override void Execute(Object? parameter) {
        _execute(parameter as T);
    }
}

/// <summary>
/// Represents a command that can be executed without requiring a specific parameter type.
/// </summary>
/// <remarks>
/// This class is a specialized version of <see cref="RelayCommand{T}"/> where the parameter type is <see cref="Object"/>.
/// It simplifies the creation of commands that do not require a strongly-typed parameter.
/// </remarks>
public sealed class RelayCommand : RelayCommand<Object> {
    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified execution logic,
    /// optional condition for execution, and an option to use the <see cref="CommandManager"/>.
    /// </summary>
    /// <param name="execute">
    /// The action to execute when the command is invoked. This parameter cannot be <see langword="null"/>.
    /// </param>
    /// <param name="canExecute">
    /// A predicate that determines whether the command can execute. If <see langword="null"/>, the command can always execute.
    /// </param>
    /// <param name="useCommandManager">
    /// A value indicating whether the command should use the <see cref="CommandManager"/> for managing
    /// the <see cref="ICommand.CanExecuteChanged"/> event.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="execute"/> parameter is <see langword="null"/>.
    /// </exception>
    public RelayCommand(Action<Object> execute, Predicate<Object>? canExecute = null, Boolean useCommandManager = false)
        : base(execute, canExecute, useCommandManager) { }
}
