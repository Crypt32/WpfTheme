using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SysadminsLV.WPF.Mvvm.Commands {
    public interface IAsyncCommand : ICommand {
        Task ExecuteAsync(Object parameter);
    }
}