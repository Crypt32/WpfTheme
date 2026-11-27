using System;
using System.IO;
using System.Reflection;

namespace SysadminsLV.WPF.Tests.Layering;

/// <summary>
/// Locates the library assembly and its sources.
/// </summary>
static class LibraryUnderTest {
    public const String AssemblyName = "SysadminsLV.WPF";
    public const String ProjectDirectoryName = "WpfTheme";

    public static readonly Assembly Library = Assembly.Load(AssemblyName);
    public static readonly String ProjectDirectory = FindProjectDirectory();

    static String FindProjectDirectory() {
        // test binaries live in <repo>\WpfTheme.Tests\bin\...; walk up to the solution file
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null) {
            if (File.Exists(Path.Combine(current.FullName, "WpfTheme.slnx"))) {
                return Path.Combine(current.FullName, ProjectDirectoryName);
            }
            current = current.Parent;
        }
        throw new InvalidOperationException("Cannot find the repository root (WpfTheme.slnx) above the test binaries.");
    }
}
