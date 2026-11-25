# WpfTheme

A light-blue "Office" style theme and a few helper controls for WPF desktop apps. It is mostly built for my own
applications, so expect it to follow what they need rather than to be a general-purpose toolkit.

<img width="957" height="793" alt="image" src="https://github.com/user-attachments/assets/946b99c7-91f8-4cf9-9aee-f7f71f5e50f6" />

## What's inside

- **Theme**: styles for the standard WPF controls (Button, TextBox, ComboBox, CheckBox, RadioButton, Slider, ScrollBar,
  TabControl, TreeView, ListView, DataGrid, Menu, ToolBar, StatusBar, Expander, GroupBox and others), driven by a
  single color palette so they stay consistent.
- **Custom controls** in the `SysadminsLV.WPF.Controls` namespace:
  - `ClosableTabControl` / `ClosableTabItem`: tabs with a close button, an optional "new tab" button, and a scrollable
    strip with a tab-list menu (the `ScrollableTabControl` style).
  - `Badge`, `LoadingSpinner`, `NotificationPanel` / `NotificationItem`, `BindableRichTextBox`.
- Helpers used by my apps: MVVM base classes and commands, behaviors, converters, collections, dialogs and logging.

## Requirements

.NET Framework 4.7.2 (WPF).

## Usage

Reference the `SysadminsLV.WPF` assembly (project reference or the NuGet package of the same name) and merge the theme
in `App.xaml`:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/SysadminsLV.WPF;component/Themes/OfficeBlue.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

`OfficeBlue.xaml` merges every control style, so there is nothing else to register. Without it, the custom controls
still get their default styles, but plain WPF controls keep the system look.

To use the scrollable tab strip:

```xml
xmlns:wpf="http://schemas.sysadmins.lv/wpf"

<wpf:ClosableTabControl Style="{StaticResource ScrollableTabControl}" ShowNewTabButton="True"/>
```

## Demo

`src/Wpf.Demo` shows every styled control on one screen. Open `src/WpfTheme.slnx` and run `Wpf.Demo`.

## Layout

| Path | Contents |
| --- | --- |
| `src/WpfTheme` | The library: `Controls`, helpers (`Mvvm`, `Behaviors`, `Converters` and so on), and `Themes` (`Brushes` for colors and gradients, `BaseControlStyles` for per-control styles) |
| `src/Wpf.Demo` | Demo application |
| `src/WpfTheme.Tests` | MSTest tests, including checks of the theme resources and the project layering |
