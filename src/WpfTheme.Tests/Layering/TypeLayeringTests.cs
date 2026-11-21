using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;

namespace SysadminsLV.WPF.Tests.Layering;

[TestClass]
public sealed class TypeLayeringTests {
    [TestMethod]
    public void LowerLayersDoNotDependOnHigherLayers() {
        List<String> violations = [];
        foreach (Layer layer in LayerMap.Layers) {
            String[] higher = LayerMap.NamespacesAbove(layer.Level);
            if (layer.Namespaces.Count == 0 || higher.Length == 0) {
                continue;
            }
            NetArchTest.Rules.TestResult result = Types.InAssembly(LibraryUnderTest.Library)
                .That().MeetCustomRule(new ExactNamespaceRule(layer.Namespaces))
                .ShouldNot().HaveDependencyOnAny(higher)
                .GetResult();
            foreach (String typeName in result.FailingTypeNames ?? []) {
                violations.Add($"[{layer.Name}] {typeName} depends on a higher layer");
            }
        }

        Assert.IsEmpty(violations, String.Join(Environment.NewLine, violations));
    }

    [TestMethod]
    public void EveryTypeBelongsToAMappedNamespace() {
        String[] unmapped = GetAuthoredTypes()
            .Select(x => x.Namespace)
            .Where(x => !String.IsNullOrEmpty(x))
            .Where(x => !LayerMap.GeneratedNamespaces.Contains(x!))
            .Where(x => LayerMap.LevelOf(x!) == null)
            .Distinct()
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        Assert.IsEmpty(unmapped, "Namespaces missing from the layer map: " + String.Join(", ", unmapped));
    }

    [TestMethod]
    public void EveryMappedNamespaceContainsTypes() {
        // an empty namespace in the map would make its rule pass without checking anything
        HashSet<String> existing = [..GetAuthoredTypes().Select(x => x.Namespace ?? String.Empty)];
        String[] stale = LayerMap.Layers
            .SelectMany(x => x.Namespaces)
            .Where(x => !existing.Contains(x))
            .ToArray();

        Assert.IsEmpty(stale, "Mapped namespaces without any type: " + String.Join(", ", stale));
    }

    [TestMethod]
    public void DependencyDetectionSeesKnownDependencies() {
        // Controls (layer 4) are known to use the command infrastructure (layer 1). If the detector finds nothing
        // here, the rules above would pass for the wrong reason.
        Layer controls = LayerMap.Layers.Single(x => x.Level == 4);
        Layer mvvm = LayerMap.Layers.Single(x => x.Level == 1);

        IEnumerable<Type> dependents = Types.InAssembly(LibraryUnderTest.Library)
            .That().MeetCustomRule(new ExactNamespaceRule(controls.Namespaces))
            .And().HaveDependencyOnAny(mvvm.Namespaces.ToArray())
            .GetTypes();

        Assert.IsNotEmpty(dependents);
    }

    // The compiler embeds helper attributes (NullableAttribute and similar) into the assembly. They are marked with
    // EmbeddedAttribute and are not part of the library design.
    static IEnumerable<Type> GetAuthoredTypes() {
        return LibraryUnderTest.Library.GetTypes()
            .Where(x => x.CustomAttributes.All(a => a.AttributeType.FullName != "Microsoft.CodeAnalysis.EmbeddedAttribute"));
    }
}
