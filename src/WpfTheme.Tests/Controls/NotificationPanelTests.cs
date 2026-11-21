using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysadminsLV.WPF.Controls;
using SysadminsLV.WPF.Tests.Layering;

namespace SysadminsLV.WPF.Tests.Controls;

[TestClass]
public sealed class NotificationPanelTests {
    [STATestMethod]
    public void DismissHidesItemAndRaisesEvent() {
        var item = new NotificationItem();
        Int32 raised = 0;
        item.DismissRequested += (_, _) => raised++;

        item.Dismiss();

        Assert.IsTrue(item.IsDismissed);
        Assert.AreEqual(1, raised);
    }

    [STATestMethod]
    public void DismissDoesNothingWhenNotDismissable() {
        var item = new NotificationItem { IsDismissable = false };
        Int32 raised = 0;
        item.DismissRequested += (_, _) => raised++;

        item.Dismiss();

        Assert.IsFalse(item.IsDismissed);
        Assert.AreEqual(0, raised);
    }

    [STATestMethod]
    public void XamlItemIsKeptWhenRemoveItemOnCloseIsFalse() {
        var item = new NotificationItem();
        var panel = new NotificationPanel();
        panel.Items.Add(item);

        item.Dismiss();

        Assert.HasCount(1, panel.Items);
    }

    [STATestMethod]
    public void XamlItemIsRemovedWhenRemoveItemOnCloseIsTrue() {
        var item = new NotificationItem();
        var other = new NotificationItem();
        var panel = new NotificationPanel { RemoveItemOnClose = true };
        panel.Items.Add(item);
        panel.Items.Add(other);

        item.Dismiss();

        Assert.HasCount(1, panel.Items);
        Assert.AreSame(other, panel.Items[0]);
    }

    [STATestMethod]
    public void BoundDataItemIsRemovedFromSource() {
        ObservableCollection<String> source = ["first", "second"];
        NotificationPanel panel = CreateLaidOutPanel(source);

        GetFirstContainer(panel).Dismiss();

        Assert.AreSequenceEqual(["second"], source);
    }

    [STATestMethod]
    public void ReadOnlySourceIsLeftUntouchedAndDoesNotThrow() {
        String[] source = ["first", "second"];
        NotificationPanel panel = CreateLaidOutPanel(source);

        GetFirstContainer(panel).Dismiss();

        Assert.AreSequenceEqual(["first", "second"], source);
    }

    [STATestMethod]
    public void DefaultStyleOfPanelDefinesATemplate() {
        // the panel does not inherit a template from ItemsControl, so without its own template it renders nothing
        String path = Path.Combine(LibraryUnderTest.ProjectDirectory, "Themes", "BaseControlStyles", "NotificationPanel.xaml");
        // the file has no assembly in its clr-namespace (it is compiled inside the library), so add it for loading outside
        String xaml = File.ReadAllText(path).Replace("clr-namespace:SysadminsLV.WPF.Controls", $"clr-namespace:SysadminsLV.WPF.Controls;assembly={LibraryUnderTest.AssemblyName}");
        var dictionary = (ResourceDictionary)XamlReader.Parse(xaml);
        var style = (Style)dictionary[typeof(NotificationPanel)];

        Assert.IsTrue(style.Setters.OfType<Setter>().Any(x => x.Property == Control.TemplateProperty));
    }

    // the dismiss event bubbles through the visual tree, so the panel needs a template with an items host that has been laid out
    static NotificationPanel CreateLaidOutPanel(IEnumerable source) {
        var panel = new NotificationPanel {
            RemoveItemOnClose = true,
            ItemsSource = source,
            Template = new ControlTemplate(typeof(NotificationPanel)) { VisualTree = new FrameworkElementFactory(typeof(ItemsPresenter)) }
        };
        panel.Measure(new Size(200, 200));
        panel.Arrange(new Rect(0, 0, 200, 200));
        return panel;
    }
    static NotificationItem GetFirstContainer(NotificationPanel panel) {
        return (NotificationItem)panel.ItemContainerGenerator.ContainerFromIndex(0)!;
    }
}