using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Commands {
    public interface IAsyncCommand : ICommand {
        Task ExecuteAsync(Object parameter);
    }
}