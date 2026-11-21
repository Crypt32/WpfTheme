using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysadminsLV.WPF.Tests.Layering;

namespace SysadminsLV.WPF.Tests.Xaml;

[TestClass]
public sealed class XmlnsDefinitionTests {
    [TestMethod]
    public void LibraryDeclaresXmlnsDefinitions() {
        Assert.IsNotEmpty(GetDefinitions());
    }

    [TestMethod]
    public void XmlnsDefinitionsPointToMappedNamespaces() {
        String[] unmapped = GetDefinitions()
            .Select(x => x.ClrNamespace)
            .Where(x => LayerMap.LevelOf(x) == null)
            .ToArray();

        Assert.IsEmpty(unmapped, "XmlnsDefinition targets missing from the layer map: " + String.Join(", ", unmapped));
    }

    [TestMethod]
    public void XmlnsDefinitionsPointToNamespacesWithPublicTypes() {
        // a typo in a namespace string is not a compile error, it silently exposes nothing
        HashSet<String> withPublicTypes = [..LibraryUnderTest.Library.GetExportedTypes().Select(x => x.Namespace ?? String.Empty)];
        String[] empty = GetDefinitions()
            .Select(x => x.ClrNamespace)
            .Where(x => !withPublicTypes.Contains(x))
            .ToArray();

        Assert.IsEmpty(empty, "XmlnsDefinition targets without public types: " + String.Join(", ", empty));
    }

    [TestMethod]
    public void EveryXmlNamespaceHasAPrefix() {
        String[] withoutPrefix = GetDefinitions()
            .Select(x => x.XmlNamespace)
            .Distinct()
            .Where(x => LibraryUnderTest.Library.GetCustomAttributes<XmlnsPrefixAttribute>().All(p => p.XmlNamespace != x))
            .ToArray();

        Assert.IsEmpty(withoutPrefix, "XML namespaces without XmlnsPrefix: " + String.Join(", ", withoutPrefix));
    }

    [TestMethod]
    public void PublicTypeNamesAreUniquePerXmlNamespace() {
        // all CLR namespaces of one XML namespace share a single XAML name scope
        List<String> duplicates = [];
        foreach (IGrouping<String, XmlnsDefinitionAttribute> uri in GetDefinitions().GroupBy(x => x.XmlNamespace)) {
            HashSet<String> clrNamespaces = [..uri.Select(x => x.ClrNamespace)];
            IEnumerable<String> names = LibraryUnderTest.Library.GetExportedTypes()
                .Where(x => clrNamespaces.Contains(x.Namespace ?? String.Empty))
                .Select(x => x.Name);
            foreach (IGrouping<String, String> group in names.GroupBy(x => x).Where(x => x.Count() > 1)) {
                duplicates.Add($"{uri.Key}: {group.Key}");
            }
        }

        Assert.IsEmpty(duplicates, "Type names that exist in several CLR namespaces of one XML namespace: " + String.Join(", ", duplicates));
    }

    static IReadOnlyList<XmlnsDefinitionAttribute> GetDefinitions() {
        return LibraryUnderTest.Library.GetCustomAttributes<XmlnsDefinitionAttribute>().ToArray();
    }
}
