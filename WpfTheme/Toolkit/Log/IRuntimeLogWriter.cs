using System;

namespace AdcsCollectorAnalyzer.Core.Abstractions {
    public interface IRuntimeLogWriter : IDisposable {
        String Log { get; }
        void WriteLine(String text);
        void WriteException(Exception e);
        void Clear();

    }
}
