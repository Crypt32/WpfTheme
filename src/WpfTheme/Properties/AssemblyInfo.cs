using System;
using System.Windows;
using System.Windows.Markup;

// Tells WPF where the default styles of this assembly's own controls live: no theme-specific dictionaries, and the
// generic dictionary Themes/Generic.xaml inside this assembly. This does not restyle controls of other assemblies.
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

// Maps the XML namespace below to the CLR namespaces whose types are used from XAML, so that consumers declare a
// single xmlns instead of one clr-namespace per CLR namespace. The URI is a contract: do not change it.
// The XAML files of this assembly cannot use the URI themselves (the XAML compiler reports MC3066 for types of the
// assembly being compiled), so they keep using clr-namespace.
[assembly: XmlnsDefinition(XmlNamespaces.Wpf, "SysadminsLV.WPF.Controls")]
[assembly: XmlnsDefinition(XmlNamespaces.Wpf, "SysadminsLV.WPF.Behaviors")]
[assembly: XmlnsDefinition(XmlNamespaces.Wpf, "SysadminsLV.WPF.Converters")]
[assembly: XmlnsPrefix(XmlNamespaces.Wpf, "wpf")]

/// <summary>
/// Contains the XML namespace URIs exposed by the library.
/// </summary>
static class XmlNamespaces {
    /// <summary>
    /// XML namespace that exposes controls, behaviors and converters of the library to XAML.
    /// </summary>
    public const String Wpf = "http://schemas.sysadmins.lv/wpf";
}
