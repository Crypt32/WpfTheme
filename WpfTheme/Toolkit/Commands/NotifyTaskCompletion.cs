using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    /// <summary>
    /// Watches a task and raises property-changed notifications when the task completes.
    /// </summary>
    public sealed class NotifyTaskCompletion : INotifyPropertyChanged {
        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        internal NotifyTaskCompletion(Task task) {
            Task = task;
            TaskCompleted = MonitorTaskAsync(task);
        }

        async Task MonitorTaskAsync(Task task) {
            try {
                await task;
            } catch {
            } finally {
                NotifyProperties(task);
            }
        }

        void NotifyProperties(Task task) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null) {
                return;
            }
            propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));
            if (task.IsCanceled) {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            } else if (task.IsFaulted) {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
            } else {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
            }
        }

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task Task { get; }

        /// <summary>
        /// Gets a task that completes successfully when <see cref="Task"/> completes (successfully, faulted, or canceled). This property never changes and is never <c>null</c>.
        /// </summary>
        public Task TaskCompleted { get; }

        /// <summary>
        /// Gets the current task status. This property raises a notification when the task completes.
        /// </summary>
        public TaskStatus Status => Task.Status;

        /// <summary>
        /// Gets whether the task has completed. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public Boolean IsCompleted => Task.IsCompleted;

        /// <summary>
        /// Gets whether the task is busy (not completed). This property raises a notification when the value changes to <c>false</c>.
        /// </summary>
        public Boolean IsNotCompleted => !Task.IsCompleted;

        /// <summary>
        /// Gets whether the task has completed successfully. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public Boolean IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets whether the task has been canceled. This property raises a notification only if the task is canceled (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public Boolean IsCanceled => Task.IsCanceled;

        /// <summary>
        /// Gets whether the task has faulted. This property raises a notification only if the task faults (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public Boolean IsFaulted => Task.IsFaulted;

        /// <summary>
        /// Gets the wrapped faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public AggregateException Exception => Task.Exception;

        /// <summary>
        /// Gets the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message for the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public String ErrorMessage => InnerException?.Message;

        /// <summary>
        /// Event that notifies listeners of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        public static NotifyTaskCompletion Create(Task task) {
            return new NotifyTaskCompletion(task);
        }

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <typeparam name="TResult">The type of the task result.</typeparam>
        /// <param name="task">The task to watch.</param>
        /// <param name="defaultResult">The default "result" value for the task while it is not yet complete.</param>
        public static NotifyTaskCompletion<TResult> Create<TResult>(Task<TResult> task, TResult defaultResult = default(TResult)) {
            return new NotifyTaskCompletion<TResult>(task, defaultResult);
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        public static NotifyTaskCompletion Create(Func<Task> asyncAction) {
            return Create(asyncAction());
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        /// <param name="defaultResult">The default "result" value for the task while it is not yet complete.</param>
        public static NotifyTaskCompletion<TResult> Create<TResult>(Func<Task<TResult>> asyncAction, TResult defaultResult = default(TResult)) {
            return Create(asyncAction(), defaultResult);
        }
    }


    // https://msdn.microsoft.com/magazine/dn605875
    /// <summary>
    /// Watches a task and raises property-changed notifications when the task completes.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged {
        /// <summary>
        /// The "result" of the task when it has not yet completed.
        /// </summary>
        readonly TResult _defaultResult;

        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        /// <param name="defaultResult">The value to return from <see cref="Result"/> while the task is not yet complete.</param>
        internal NotifyTaskCompletion(Task<TResult> task, TResult defaultResult) {
            _defaultResult = defaultResult;
            Task = task;
            TaskCompleted = MonitorTaskAsync(task);
        }

        async Task MonitorTaskAsync(Task task) {
            try {
                await task;
            } catch {
            } finally {
                NotifyProperties(task);
            }
        }

        void NotifyProperties(Task task) {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null) {
                return;
            }

            propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));
            if (task.IsCanceled) {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            } else if (task.IsFaulted) {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
            } else {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
            }
        }

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task<TResult> Task { get; }

        /// <summary>
        /// Gets a task that completes successfully when <see cref="Task"/> completes (successfully, faulted, or canceled). This property never changes and is never <c>null</c>.
        /// </summary>
        public Task TaskCompleted { get; }

        /// <summary>
        /// Gets the result of the task. Returns the "default result" value specified in the constructor if the task has not yet completed successfully. This property raises a notification when the task completes successfully.
        /// </summary>
        public TResult Result => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : _defaultResult;

        /// <summary>
        /// Gets the current task status. This property raises a notification when the task completes.
        /// </summary>
        public TaskStatus Status => Task.Status;

        /// <summary>
        /// Gets whether the task has completed. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public Boolean IsCompleted => Task.IsCompleted;

        /// <summary>
        /// Gets whether the task is busy (not completed). This property raises a notification when the value changes to <c>false</c>.
        /// </summary>
        public Boolean IsNotCompleted => !Task.IsCompleted;

        /// <summary>
        /// Gets whether the task has completed successfully. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public Boolean IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets whether the task has been canceled. This property raises a notification only if the task is canceled (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public Boolean IsCanceled => Task.IsCanceled;

        /// <summary>
        /// Gets whether the task has faulted. This property raises a notification only if the task faults (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public Boolean IsFaulted => Task.IsFaulted;

        /// <summary>
        /// Gets the wrapped faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public AggregateException Exception => Task.Exception;

        /// <summary>
        /// Gets the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message for the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public String ErrorMessage => InnerException?.Message;

        /// <summary>
        /// Event that notifies listeners of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
