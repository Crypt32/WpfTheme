using System;

namespace SysadminsLV.WPF.Logging {
    public interface IRuntimeLogWriter : IDisposable {
        String Log { get; }
        void WriteLine(String text);
        void WriteException(Exception e);
        void Clear();
    }
}
