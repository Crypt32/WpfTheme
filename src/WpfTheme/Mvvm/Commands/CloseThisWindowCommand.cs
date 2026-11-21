using System;
using System.Windows;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    public class CloseThisWindowCommand : ICommand {

        CloseThisWindowCommand() { }

        public Boolean CanExecute(Object parameter) {
            //we can only close Windows
            return parameter is Window;
        }
        public void Execute(Object parameter) {
            if (CanExecute(parameter)) {
                ((Window)parameter).Close();
            }
        }
        public static readonly ICommand Instance = new CloseThisWindowCommand();
        public event EventHandler CanExecuteChanged;
    }
}
