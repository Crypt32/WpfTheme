using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using SysadminsLV.WPF.OfficeTheme.Toolkit.ViewModels;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Log {
    public class RuntimeLogWriter : ViewModelBase, IRuntimeLogWriter {
        readonly StreamWriter sessionStream;
        readonly StringBuilder _builder = new StringBuilder();

        public RuntimeLogWriter(String path) {
            Directory.CreateDirectory(Path.Combine(path, "Logs"));
            String dt = "Log-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            sessionStream = new StreamWriter(Path.Combine(path, "Logs", $"{dt}-{Process.GetCurrentProcess().Id}.log")) { AutoFlush = true };
        }

        void write(String text) {
            _builder.AppendLine(text);
            sessionStream.WriteLine(text);
            Debug.WriteLine(text);
            OnPropertyChanged(nameof(Log));
        }

        public String Log => _builder.ToString();

        public void WriteLine(String text) {
            String dt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            write($"[{dt}] {text}");
        }
        public void WriteException(Exception e) {
            String dt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            String line = $"[{dt}]" + " An exception has been thrown:";
            write(line);
            Exception ex = e;
            do {
                line = $"\tError message: {ex.Message}\r\n\tStack trace:\r\n{ex.StackTrace.Replace("   ", "\t\t")}";
                write(line);
                ex = ex.InnerException;
            } while (ex != null);
        }
        public void Clear() {
            _builder.Clear();
            OnPropertyChanged(nameof(Log));
        }

        /// <inheritdoc />
        public void Dispose() {
            sessionStream?.Dispose();
        }
    }
}