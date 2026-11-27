using System;
using System.Collections.Generic;

namespace SysadminsLV.WPF.Tests.Layering;

/// <summary>
/// Describes one layer of the library architecture.
/// </summary>
/// <param name="Level">
/// Position in the stack. A layer may only depend on layers with a lower or equal level.
/// </param>
/// <param name="Name">
/// Human-readable layer name, used in failure messages.
/// </param>
/// <param name="Namespaces">
/// CLR namespaces that belong to the layer. Each entry is matched exactly, nested namespaces must be listed on their own.
/// </param>
public sealed record Layer(Int32 Level, String Name, IReadOnlyList<String> Namespaces);
