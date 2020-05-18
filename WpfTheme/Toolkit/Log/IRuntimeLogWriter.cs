using System;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Log {
    public interface IRuntimeLogWriter : IDisposable {
        String Log { get; }
        void WriteLine(String text);
        void WriteException(Exception e);
        void Clear();
    }
}
