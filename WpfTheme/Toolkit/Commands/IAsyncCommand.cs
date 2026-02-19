using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

/// <summary>
/// Represents an asynchronous command that extends the <see cref="ICommand"/> interface.
/// This interface is designed to support asynchronous operations in WPF applications.
/// </summary>
/// <remarks>
/// Implementations of this interface provide a mechanism to execute asynchronous tasks 
/// while maintaining compatibility with the WPF command infrastructure. It is particularly 
/// useful for scenarios where long-running operations need to be executed without blocking 
/// the UI thread.
/// </remarks>
public interface IAsyncCommand : ICommand {
    /// <summary>
    /// Executes the asynchronous operation associated with the command.
    /// </summary>
    /// <param name="parameter">
    /// An optional parameter passed to the command. This parameter can be used to provide 
    /// additional data required for the execution of the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation. The task completes when 
    /// the operation has finished executing.
    /// </returns>
    /// <remarks>
    /// This method is designed to be invoked in scenarios where the command needs to execute 
    /// an asynchronous task. It ensures that the operation does not block the UI thread, 
    /// maintaining responsiveness in WPF applications.
    /// </remarks>
    Task ExecuteAsync(Object parameter);
}