using System;
using System.Collections.Generic;
using Mono.Cecil;
using NetArchTest.Rules;

namespace SysadminsLV.WPF.Tests.Layering;

/// <summary>
/// Selects types whose namespace is exactly one of the specified namespaces. The built-in namespace predicate of
/// NetArchTest matches by prefix, which would pull nested namespaces into a layer they do not belong to.
/// </summary>
sealed class ExactNamespaceRule : ICustomRule {
    readonly HashSet<String> _namespaces;

    public ExactNamespaceRule(IEnumerable<String> namespaces) {
        _namespaces = [..namespaces];
    }

    public Boolean MeetsRule(TypeDefinition type) {
        // nested and compiler-generated types report an empty namespace, the owner is the outermost declaring type
        TypeDefinition outermost = type;
        while (outermost.DeclaringType != null) {
            outermost = outermost.DeclaringType;
        }
        return _namespaces.Contains(outermost.Namespace);
    }
}
