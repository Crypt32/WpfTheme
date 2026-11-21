using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit.Behaviors; 
public class MarginSetter {
    public static Thickness GetMargin(DependencyObject obj) {
        return (Thickness)obj.GetValue(MarginProperty);
    }
    public static void SetMargin(DependencyObject obj, Thickness value) {
        obj.SetValue(MarginProperty, value);
    }

    // Using a DependencyProperty as the backing store for Margin. This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MarginProperty =
        DependencyProperty.RegisterAttached(
            "Margin",
            typeof(Thickness),
            typeof(MarginSetter),
            new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

    public static void MarginChangedCallback(Object sender, DependencyPropertyChangedEventArgs e) {
        // Make sure this is put on a panel
        if (sender is not Panel panel) { return; }
        
        // Remove old handler if exists to prevent duplicate subscriptions
        panel.Loaded -= panelLoaded;
        panel.Loaded += panelLoaded;
        
        // Subscribe to Unloaded to clean up event handler
        panel.Unloaded -= panelUnloaded;
        panel.Unloaded += panelUnloaded;
    }
    
    static void panelLoaded(Object sender, RoutedEventArgs e) {
        if (!(sender is Panel panel)) { return; }
        // Go over the children and set margin for them:
        foreach (FrameworkElement fe in panel.Children.OfType<FrameworkElement>()) {
            fe.Margin = GetMargin(panel);
        }
    }
    
    static void panelUnloaded(Object sender, RoutedEventArgs e) {
        if (sender is not Panel panel) { return; }
        // Clean up event handlers to prevent memory leaks
        panel.Loaded -= panelLoaded;
        panel.Unloaded -= panelUnloaded;
    }
}
