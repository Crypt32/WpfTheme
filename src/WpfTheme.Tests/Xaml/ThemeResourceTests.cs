using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysadminsLV.WPF.Tests.Layering;

namespace SysadminsLV.WPF.Tests.Xaml;

[TestClass]
public sealed class ThemeResourceTests {
    const String ThemesDirectoryName = "Themes";
    const String GenericFileName = "Generic.xaml";

    // DefaultStyleKeyProperty is protected, so it is read through reflection
    static readonly DependencyProperty _defaultStyleKey = (DependencyProperty)typeof(FrameworkElement)
        .GetField("DefaultStyleKeyProperty", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)!
        .GetValue(null)!;
    static readonly Regex _packUri = new(@"^pack://application:,,,/(?<assembly>[^;/]+);component/(?<path>.+)$", RegexOptions.IgnoreCase);

    [TestMethod]
    public void EveryResourceDictionarySourceResolvesToAFile() {
        // WPF reports a misspelled or moved Source only when the dictionary is loaded at runtime
        List<String> violations = [];
        Int32 checkedSources = 0;
        foreach (String file in GetThemeFiles()) {
            foreach (String source in GetSources(file)) {
                checkedSources++;
                String? error = Resolve(file, source, out String _);
                if (error != null) {
                    violations.Add($"{GetRelativePath(file)}: Source=\"{source}\" {error}");
                }
            }
        }

        Assert.IsGreaterThan(0, checkedSources, "No Source attributes were found; the scan is broken.");
        Assert.IsEmpty(violations, String.Join(Environment.NewLine, violations));
    }

    [TestMethod]
    public void EveryCustomControlHasADefaultStyleInGenericXaml() {
        String generic = Path.Combine(LibraryUnderTest.ProjectDirectory, ThemesDirectoryName, GenericFileName);
        Assert.IsTrue(File.Exists(generic), $"{ThemesDirectoryName}\\{GenericFileName} is missing.");
        HashSet<String> styled = CollectStyleTargets(generic, []);
        Assert.IsNotEmpty(styled, "No styles were found in Generic.xaml; the scan is broken.");

        Type[] controls = LibraryUnderTest.Library.GetExportedTypes()
            .Where(x => x.Namespace == $"{LayerMap.Root}.Controls")
            .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(Control)))
            .ToArray();
        Assert.IsNotEmpty(controls, "No custom controls were found; the scan is broken.");

        List<String> violations = [];
        foreach (Type control in controls) {
            // DefaultStyleKey of the control must be the control itself, otherwise the style of its base class is used
            RuntimeHelpers.RunClassConstructor(control.TypeHandle);
            Object? key = _defaultStyleKey.GetMetadata(control).DefaultValue;
            if (!Equals(key, control)) {
                violations.Add($"{control.Name} does not override DefaultStyleKey.");
            }
            if (!styled.Contains(control.Name)) {
                violations.Add($"{control.Name} has no style with its TargetType in {GenericFileName} (or the files it merges).");
            }
        }

        Assert.IsEmpty(violations, String.Join(Environment.NewLine, violations));
    }

    // collects local type names of TargetType attributes in the file and in everything it merges
    static HashSet<String> CollectStyleTargets(String file, HashSet<String> visited) {
        HashSet<String> result = [];
        if (!visited.Add(file)) {
            return result;
        }
        foreach (XAttribute target in XDocument.Load(file).Descendants().Attributes("TargetType")) {
            // forms: "controls:Badge" or "{x:Type c:Badge}"
            String name = target.Value.Trim('{', '}').Replace("x:Type", String.Empty).Trim();
            result.Add(name.Substring(name.IndexOf(':') + 1));
        }
        foreach (String source in GetSources(file)) {
            if (Resolve(file, source, out String path) == null) {
                result.UnionWith(CollectStyleTargets(path, visited));
            }
        }
        return result;
    }

    // returns null if the Source points to an existing file of this library, otherwise the reason; path is the full path
    static String? Resolve(String file, String source, out String path) {
        path = String.Empty;
        Match pack = _packUri.Match(source);
        if (pack.Success) {
            if (!pack.Groups["assembly"].Value.Equals(LibraryUnderTest.AssemblyName, StringComparison.OrdinalIgnoreCase)) {
                return $"points to assembly '{pack.Groups["assembly"].Value}', expected '{LibraryUnderTest.AssemblyName}'.";
            }
            path = Path.GetFullPath(Path.Combine(LibraryUnderTest.ProjectDirectory, pack.Groups["path"].Value.Replace('/', Path.DirectorySeparatorChar)));
        } else if (source.Contains("://")) {
            return "is an absolute URI that this test cannot verify.";
        } else {
            path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file)!, source.Replace('/', Path.DirectorySeparatorChar)));
        }
        return File.Exists(path)
            ? null
            : "does not exist.";
    }

    static IEnumerable<String> GetSources(String file) {
        return XDocument.Load(file)
            .Descendants()
            .Where(x => x.Name.LocalName == "ResourceDictionary")
            .Select(x => x.Attribute("Source")?.Value)
            .Where(x => x != null)
            .Select(x => x!);
    }
    static IEnumerable<String> GetThemeFiles() {
        return Directory.EnumerateFiles(Path.Combine(LibraryUnderTest.ProjectDirectory, ThemesDirectoryName), "*.xaml", SearchOption.AllDirectories);
    }
    static String GetRelativePath(String path) {
        return path.Substring(LibraryUnderTest.ProjectDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
    }
}