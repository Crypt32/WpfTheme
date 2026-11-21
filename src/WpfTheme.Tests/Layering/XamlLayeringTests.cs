using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SysadminsLV.WPF.Tests.Layering;

[TestClass]
public sealed class XamlLayeringTests {
    const String ClrNamespacePrefix = "clr-namespace:";

    [TestMethod]
    public void EveryXamlFileIsInAKnownDirectory() {
        String[] unknown = GetXamlFiles()
            .Where(x => GetLayerLevel(x) == null)
            .Select(GetRelativePath)
            .ToArray();

        Assert.IsEmpty(unknown, "XAML files outside of known directories (add the directory to LayerMap.XamlDirectories): " + String.Join(", ", unknown));
    }

    [TestMethod]
    public void XamlReferencesOnlyMappedNamespaces() {
        List<String> violations = [];
        foreach (String file in GetXamlFiles()) {
            foreach (String clrNamespace in GetOwnClrNamespaces(file)) {
                if (LayerMap.LevelOf(clrNamespace) == null) {
                    violations.Add($"{GetRelativePath(file)} references unmapped namespace {clrNamespace}");
                }
            }
        }

        Assert.IsEmpty(violations, String.Join(Environment.NewLine, violations));
    }

    [TestMethod]
    public void XamlDoesNotReferenceHigherLayers() {
        List<String> violations = [];
        foreach (String file in GetXamlFiles()) {
            Int32? fileLevel = GetLayerLevel(file);
            if (fileLevel == null) {
                continue;
            }
            foreach (String clrNamespace in GetOwnClrNamespaces(file)) {
                Int32? targetLevel = LayerMap.LevelOf(clrNamespace);
                if (targetLevel > fileLevel) {
                    violations.Add($"{GetRelativePath(file)} (layer {fileLevel}) references {clrNamespace} (layer {targetLevel})");
                }
            }
        }

        Assert.IsEmpty(violations, String.Join(Environment.NewLine, violations));
    }

    static IEnumerable<String> GetXamlFiles() {
        return Directory
            .EnumerateFiles(LibraryUnderTest.ProjectDirectory, "*.xaml", SearchOption.AllDirectories)
            .Where(x => !IsBuildOutput(x));
    }
    static Boolean IsBuildOutput(String path) {
        String[] segments = GetRelativePath(path).Split(Path.DirectorySeparatorChar);
        return segments.Contains("obj", StringComparer.OrdinalIgnoreCase)
               || segments.Contains("bin", StringComparer.OrdinalIgnoreCase);
    }
    static String GetRelativePath(String path) {
        return path.Substring(LibraryUnderTest.ProjectDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
    }
    static Int32? GetLayerLevel(String path) {
        String topDirectory = GetRelativePath(path).Split(Path.DirectorySeparatorChar)[0];
        return LayerMap.XamlDirectories.TryGetValue(topDirectory, out Int32 level)
            ? level
            : null;
    }
    // returns CLR namespaces of the library itself referenced through xmlns declarations of a XAML file
    static IEnumerable<String> GetOwnClrNamespaces(String file) {
        XDocument document = XDocument.Load(file);
        return document.Descendants()
            .SelectMany(x => x.Attributes())
            .Where(x => x.IsNamespaceDeclaration && x.Value.StartsWith(ClrNamespacePrefix, StringComparison.Ordinal))
            .Select(x => x.Value.Substring(ClrNamespacePrefix.Length).Split(';'))
            .Where(x => x.Length == 1 || x.Any(p => p.Equals($"assembly={LibraryUnderTest.AssemblyName}", StringComparison.OrdinalIgnoreCase)))
            .Select(x => x[0])
            .Distinct();
    }
}
