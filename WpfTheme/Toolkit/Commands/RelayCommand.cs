using System;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    public abstract class RelayCommand<T> : CommandBase<T> where T : class {
        readonly Action<T> _execute;

        // constructors
        protected RelayCommand(Action<T> execute, Predicate<T> canExecute = null) : base(canExecute) {
            _execute = execute;
        }

        public override void Execute(Object parameter) {
            _execute(parameter as T);
        }
    }
    public sealed class RelayCommand : RelayCommand<Object> {
        public RelayCommand(Action<Object> execute, Predicate<Object> canExecute = null) : base(execute, canExecute) { }
    }
    
}
