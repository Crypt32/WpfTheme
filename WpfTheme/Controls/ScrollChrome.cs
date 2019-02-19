using System;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// Credits: Microsoft WPF Luna theme https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.0/aa972127(v=vs.85)

namespace SysadminsLV.WPF.OfficeTheme.Controls {
    public enum ScrollGlyph {
        None,
        LeftArrow,
        RightArrow,
        UpArrow,
        DownArrow,
        VerticalGripper,
        HorizontalGripper,
    }

    public class ScrollChrome : FrameworkElement {
        public static readonly DependencyProperty HasOuterBorderProperty = DependencyProperty.Register(
            nameof(HasOuterBorder),
            typeof(Boolean),
            typeof(ScrollChrome),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ScrollGlyphProperty = DependencyProperty.RegisterAttached(
            nameof(ScrollGlyph),
            typeof(ScrollGlyph),
            typeof(ScrollChrome),
            new FrameworkPropertyMetadata(ScrollGlyph.None, FrameworkPropertyMetadataOptions.AffectsRender),
            IsValidScrollGlyph);
        public static readonly DependencyProperty PaddingProperty = Control.PaddingProperty.AddOwner(
            typeof(ScrollChrome));
        public static readonly DependencyProperty RenderMouseOverProperty = DependencyProperty.Register(
            nameof(RenderMouseOver),
            typeof(Boolean),
            typeof(ScrollChrome),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty RenderPressedProperty = DependencyProperty.Register(
            nameof(RenderPressed),
            typeof(Boolean),
            typeof(ScrollChrome),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        
        static readonly Object GlyphAccess = new Object();
        static readonly Object ResourceAccess = new Object();
        static Geometry LeftArrowGeometry;
        static Geometry RightArrowGeometry;
        static Geometry UpArrowGeometry;
        static Geometry DownArrowGeometry;
        static LinearGradientBrush LineButtonFill;
        static LinearGradientBrush VerticalFill;
        static LinearGradientBrush HorizontalFill;
        static LinearGradientBrush HoverLineButtonFill;
        static LinearGradientBrush HoverVerticalFill;
        static LinearGradientBrush HoverHorizontalFill;
        static LinearGradientBrush PressedLineButtonFill;
        static LinearGradientBrush PressedVerticalFill;
        static LinearGradientBrush PressedHorizontalFill;
        static LinearGradientBrush DisabledFill;
        static SolidColorBrush DisabledGlyph;
        static SolidColorBrush ArrowGlyph;
        static SolidColorBrush GripperGlyph;
        static SolidColorBrush HoverGripperGlyph;
        static SolidColorBrush PressedGripperGlyph;
        static SolidColorBrush DisabledGripperGlyphShadow;
        static SolidColorBrush GripperGlyphShadow;
        static SolidColorBrush HoverGripperGlyphShadow;
        static SolidColorBrush PressedGripperGlyphShadow;
        static Pen OuterBorderPen;
        static Pen DisabledInnerBorderPen;
        static Pen InnerBorderPen;
        static Pen HoverInnerBorderPen;
        static Pen HoverThumbInnerBorderPen;
        static Pen PressedInnerBorderPen;
        static Pen DisabledShadowPen;
        static Pen ShadowPen;
        MatrixTransform Transform;

        public Boolean HasOuterBorder {
            get => (Boolean)GetValue(HasOuterBorderProperty);
            set => SetValue(HasOuterBorderProperty, value);
        }
        public ScrollGlyph ScrollGlyph {
            get => (ScrollGlyph)GetValue(ScrollGlyphProperty);
            set => SetValue(ScrollGlyphProperty, value);
        }
        public Thickness Padding {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        public Boolean RenderMouseOver {
            get => (Boolean)GetValue(RenderMouseOverProperty);
            set => SetValue(RenderMouseOverProperty, value);
        }
        public Boolean RenderPressed {
            get => (Boolean)GetValue(RenderPressedProperty);
            set => SetValue(RenderPressedProperty, value);
        }

        static Geometry RenderLeftArrowGeometry {
            get {
                if (LeftArrowGeometry == null) {
                    lock (GlyphAccess) {
                        if (LeftArrowGeometry == null) {
                            var local_0 = new PathFigure {StartPoint = new Point(4.5, 0.0)};
                            local_0.Segments.Add(new LineSegment(new Point(0.0, 4.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(4.5, 9.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(6.0, 7.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(3.0, 4.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(6.0, 1.5), true));
                            local_0.IsClosed = true;
                            local_0.Freeze();
                            var local_1 = new PathGeometry();
                            local_1.Figures.Add(local_0);
                            local_1.Freeze();
                            return LeftArrowGeometry = local_1;
                        }
                    }
                }
                return LeftArrowGeometry;
            }
        }
        static Geometry RenderRightArrowGeometry {
            get {
                if (RightArrowGeometry == null) {
                    lock (GlyphAccess) {
                        if (RightArrowGeometry == null) {
                            var local_0 = new PathFigure {StartPoint = new Point(3.5, 0.0)};
                            local_0.Segments.Add(new LineSegment(new Point(8.0, 4.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(3.5, 9.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(2.0, 7.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(5.0, 4.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(2.0, 1.5), true));
                            local_0.IsClosed = true;
                            local_0.Freeze();
                            var local_1 = new PathGeometry();
                            local_1.Figures.Add(local_0);
                            local_1.Freeze();
                            RightArrowGeometry = local_1;
                        }
                    }
                }
                return RightArrowGeometry;
            }
        }
        static Geometry RenderUpArrowGeometry {
            get {
                if (UpArrowGeometry == null) {
                    lock (GlyphAccess) {
                        if (UpArrowGeometry == null) {
                            var local_0 = new PathFigure {StartPoint = new Point(0.0, 4.5)};
                            local_0.Segments.Add(new LineSegment(new Point(4.5, 0.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(9.0, 4.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(7.5, 6.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(4.5, 3.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(1.5, 6.0), true));
                            local_0.IsClosed = true;
                            local_0.Freeze();
                            var local_1 = new PathGeometry();
                            local_1.Figures.Add(local_0);
                            local_1.Freeze();
                            UpArrowGeometry = local_1;
                        }
                    }
                }
                return UpArrowGeometry;
            }
        }
        static Geometry RenderDownArrowGeometry {
            get {
                if (DownArrowGeometry == null) {
                    lock (GlyphAccess) {
                        if (DownArrowGeometry == null) {
                            var local_0 = new PathFigure {StartPoint = new Point(0.0, 3.5)};
                            local_0.Segments.Add(new LineSegment(new Point(4.5, 8.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(9.0, 3.5), true));
                            local_0.Segments.Add(new LineSegment(new Point(7.5, 2.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(4.5, 5.0), true));
                            local_0.Segments.Add(new LineSegment(new Point(1.5, 2.0), true));
                            local_0.IsClosed = true;
                            local_0.Freeze();
                            var local_1 = new PathGeometry();
                            local_1.Figures.Add(local_0);
                            local_1.Freeze();
                            DownArrowGeometry = local_1;
                        }
                    }
                }
                return DownArrowGeometry;
            }
        }

        internal Int32 EffectiveValuesInitialSize => 9;

        static LinearGradientBrush RenderLineButtonFill {
            get {
                if (LineButtonFill == null) {
                    lock (ResourceAccess) {
                        if (LineButtonFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 225, 234, 254), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 195, 211, 253), 0.3));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 195, 211, 253), 0.6));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 187, 205, 249), 1.0));
                            local_0.Freeze();
                            return LineButtonFill = local_0;
                        }
                    }
                }
                return LineButtonFill;
            }
        }
        static LinearGradientBrush RenderVerticalFill {
            get {
                if (VerticalFill == null) {
                    lock (ResourceAccess) {
                        if (VerticalFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 0.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 201, 216, 252), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 194, 211, 252), 0.65));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 182, 205, 251), 1.0));
                            local_0.Freeze();
                            VerticalFill = local_0;
                        }
                    }
                }
                return VerticalFill;
            }
        }
        static LinearGradientBrush RenderHorizontalFill {
            get {
                if (HorizontalFill == null) {
                    lock (ResourceAccess) {
                        if (HorizontalFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(0.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 201, 216, 252), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 194, 211, 252), 0.65));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 182, 205, 251), 1.0));
                            local_0.Freeze();
                            HorizontalFill = local_0;
                        }
                    }
                }
                return HorizontalFill;
            }
        }
        static LinearGradientBrush RenderHoverLineButtonFill {
            get {
                if (HoverLineButtonFill == null) {
                    lock (ResourceAccess) {
                        if (HoverLineButtonFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 253, Byte.MaxValue, Byte.MaxValue), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 226, 243, 253), 0.25));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 185, 218, 251), 1.0));
                            local_0.Freeze();
                            HoverLineButtonFill = local_0;
                        }
                    }
                }
                return HoverLineButtonFill;
            }
        }
        static LinearGradientBrush RenderHoverVerticalFill {
            get {
                if (HoverVerticalFill == null) {
                    lock (ResourceAccess) {
                        if (HoverVerticalFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 0.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 218, 233, Byte.MaxValue), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 212, 230, Byte.MaxValue), 0.65));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 202, 224, Byte.MaxValue), 1.0));
                            local_0.Freeze();
                            HoverVerticalFill = local_0;
                        }
                    }
                }
                return HoverVerticalFill;
            }
        }
        static LinearGradientBrush RenderHoverHorizontalFill {
            get {
                if (HoverHorizontalFill == null) {
                    lock (ResourceAccess) {
                        if (HoverHorizontalFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(0.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 218, 233, Byte.MaxValue), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 212, 230, Byte.MaxValue), 0.65));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 202, 224, Byte.MaxValue), 1.0));
                            local_0.Freeze();
                            HoverHorizontalFill = local_0;
                        }
                    }
                }
                return HoverHorizontalFill;
            }
        }
        static LinearGradientBrush RenderPressedLineButtonFill {
            get {
                if (PressedLineButtonFill == null) {
                    lock (ResourceAccess) {
                        if (PressedLineButtonFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 110, 142, 241), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, Byte.MinValue, 157, 241), 0.3));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 175, 191, 237), 0.7));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 210, 222, 235), 1.0));
                            local_0.Freeze();
                            PressedLineButtonFill = local_0;
                        }
                    }
                }
                return PressedLineButtonFill;
            }
        }
        static LinearGradientBrush RenderPressedVerticalFill {
            get {
                if (PressedVerticalFill == null) {
                    lock (ResourceAccess) {
                        if (PressedVerticalFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 0.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 168, 190, 245), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 161, 189, 250), 0.65));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 152, 176, 238), 1.0));
                            local_0.Freeze();
                            PressedVerticalFill = local_0;
                        }
                    }
                }
                return PressedVerticalFill;
            }
        }
        static LinearGradientBrush RenderPressedHorizontalFill {
            get {
                if (PressedHorizontalFill == null) {
                    lock (ResourceAccess) {
                        if (PressedHorizontalFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(0.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 168, 190, 245), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 161, 189, 250), 0.65));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 152, 176, 238), 1.0));
                            local_0.Freeze();
                            PressedHorizontalFill = local_0;
                        }
                    }
                }
                return PressedHorizontalFill;
            }
        }
        static LinearGradientBrush RenderDisabledFill {
            get {
                if (DisabledFill == null) {
                    lock (ResourceAccess) {
                        if (DisabledFill == null) {
                            var local_0 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(1.0, 1.0)
                            };
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 247, 247, 247), 0.0));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 240, 240, 240), 0.3));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 236, 236, 236), 0.6));
                            local_0.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 227, 227, 227), 1.0));
                            local_0.Freeze();
                            DisabledFill = local_0;
                        }
                    }
                }
                return DisabledFill;
            }
        }

        Brush Fill {
            get {
                if (!IsEnabled)
                    return RenderDisabledFill;
                ScrollGlyph scrollGlyph = ScrollGlyph;
                if (scrollGlyph == ScrollGlyph.VerticalGripper) {
                    if (RenderPressed) { return RenderPressedVerticalFill; }
                    return RenderMouseOver ? RenderHoverVerticalFill : RenderVerticalFill;
                }
                if (scrollGlyph == ScrollGlyph.HorizontalGripper) {
                    if (RenderPressed) { return RenderPressedHorizontalFill; }
                    return RenderMouseOver ? RenderHoverHorizontalFill : RenderHorizontalFill;
                }
                if (RenderPressed) {
                    return RenderPressedLineButtonFill;
                }
                return RenderMouseOver ? RenderHoverLineButtonFill : RenderLineButtonFill;
            }
        }

        static SolidColorBrush RenderDisabledGlyph {
            get {
                if (DisabledGlyph == null) {
                    lock (ResourceAccess) {
                        if (DisabledGlyph == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 201, 201, 194));
                            local_0.Freeze();
                            DisabledGlyph = local_0;
                        }
                    }
                }
                return DisabledGlyph;
            }
        }
        static SolidColorBrush RenderArrowGlyph {
            get {
                if (ArrowGlyph == null) {
                    lock (ResourceAccess) {
                        if (ArrowGlyph == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 77, 97, 133));
                            local_0.Freeze();
                            ArrowGlyph = local_0;
                        }
                    }
                }
                return ArrowGlyph;
            }
        }
        static SolidColorBrush RenderGripperGlyph {
            get {
                if (GripperGlyph == null) {
                    lock (ResourceAccess) {
                        if (GripperGlyph == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 238, 244, 254));
                            local_0.Freeze();
                            GripperGlyph = local_0;
                        }
                    }
                }
                return GripperGlyph;
            }
        }
        static SolidColorBrush RenderHoverGripperGlyph {
            get {
                if (HoverGripperGlyph == null) {
                    lock (ResourceAccess) {
                        if (HoverGripperGlyph == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 252, 253, Byte.MaxValue));
                            local_0.Freeze();
                            HoverGripperGlyph = local_0;
                        }
                    }
                }
                return HoverGripperGlyph;
            }
        }
        static SolidColorBrush RenderPressedGripperGlyph {
            get {
                if (PressedGripperGlyph == null) {
                    lock (ResourceAccess) {
                        if (PressedGripperGlyph == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 207, 221, 253));
                            local_0.Freeze();
                            PressedGripperGlyph = local_0;
                        }
                    }
                }
                return PressedGripperGlyph;
            }
        }

        Brush Glyph {
            get {
                if (!IsEnabled) { return RenderDisabledGlyph; }
                switch (ScrollGlyph) {
                    case ScrollGlyph.HorizontalGripper:
                    case ScrollGlyph.VerticalGripper:
                        if (RenderPressed) { return RenderPressedGripperGlyph; }
                        return RenderMouseOver ? RenderHoverGripperGlyph : RenderGripperGlyph;
                    default:
                        return RenderArrowGlyph;
                }
            }
        }

        static SolidColorBrush RenderDisabledGripperGlyphShadow {
            get {
                if (DisabledGripperGlyphShadow == null) {
                    lock (ResourceAccess) {
                        if (DisabledGripperGlyphShadow == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 185, 185, 178));
                            local_0.Freeze();
                            DisabledGripperGlyphShadow = local_0;
                        }
                    }
                }
                return DisabledGripperGlyphShadow;
            }
        }
        static SolidColorBrush RenderGripperGlyphShadow {
            get {
                if (GripperGlyphShadow == null) {
                    lock (ResourceAccess) {
                        if (GripperGlyphShadow == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 140, 176, 248));
                            local_0.Freeze();
                            GripperGlyphShadow = local_0;
                        }
                    }
                }
                return GripperGlyphShadow;
            }
        }
        static SolidColorBrush RenderHoverGripperGlyphShadow {
            get {
                if (HoverGripperGlyphShadow == null) {
                    lock (ResourceAccess) {
                        if (HoverGripperGlyphShadow == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 156, 197, Byte.MaxValue));
                            local_0.Freeze();
                            HoverGripperGlyphShadow = local_0;
                        }
                    }
                }
                return HoverGripperGlyphShadow;
            }
        }
        static SolidColorBrush RenderPressedGripperGlyphShadow {
            get {
                if (PressedGripperGlyphShadow == null) {
                    lock (ResourceAccess) {
                        if (PressedGripperGlyphShadow == null) {
                            var local_0 = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 131, 158, 216));
                            local_0.Freeze();
                            PressedGripperGlyphShadow = local_0;
                        }
                    }
                }
                return PressedGripperGlyphShadow;
            }
        }

        Brush GlyphShadow {
            get {
                switch (ScrollGlyph) {
                    case ScrollGlyph.HorizontalGripper:
                    case ScrollGlyph.VerticalGripper:
                        if (!IsEnabled) { return RenderDisabledGripperGlyphShadow; }
                        if (RenderPressed) { return RenderPressedGripperGlyphShadow; }
                        return RenderMouseOver ? RenderHoverGripperGlyphShadow : RenderGripperGlyphShadow;
                    default:
                        return null;
                }
            }
        }

        static Pen RenderOuterBorderPen {
            get {
                if (OuterBorderPen == null) {
                    lock (ResourceAccess) {
                        if (OuterBorderPen == null) {
                            var local_0 = new Pen {
                                Brush = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue))
                            };
                            local_0.Freeze();
                            return OuterBorderPen = local_0;
                        }
                    }
                }
                return OuterBorderPen;
            }
        }

        Pen OuterBorder => !HasOuterBorder ? null : RenderOuterBorderPen;

        static Pen RenderDisabledInnerBorderPen {
            get {
                if (DisabledInnerBorderPen == null) {
                    lock (ResourceAccess) {
                        if (DisabledInnerBorderPen == null) {
                            var local_0 = new Pen { Brush = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 232, 232, 223)) };
                            local_0.Freeze();
                            DisabledInnerBorderPen = local_0;
                        }
                    }
                }
                return DisabledInnerBorderPen;
            }
        }
        static Pen RenderInnerBorderPen {
            get {
                if (InnerBorderPen == null) {
                    lock (ResourceAccess) {
                        if (InnerBorderPen == null) {
                            var local_0 = new Pen { Brush = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 180, 200, 246)) };
                            local_0.Freeze();
                            InnerBorderPen = local_0;
                        }
                    }
                }
                return InnerBorderPen;
            }
        }
        static Pen RenderHoverInnerBorderPen {
            get {
                if (HoverInnerBorderPen == null) {
                    lock (ResourceAccess) {
                        if (HoverInnerBorderPen == null) {
                            var local_0 = new Pen { Brush = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 152, 177, 228)) };
                            local_0.Freeze();
                            HoverInnerBorderPen = local_0;
                        }
                    }
                }
                return HoverInnerBorderPen;
            }
        }
        static Pen RenderHoverThumbInnerBorderPen {
            get {
                if (HoverThumbInnerBorderPen == null) {
                    lock (ResourceAccess) {
                        if (HoverThumbInnerBorderPen == null) {
                            var local_0 = new Pen { Brush = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 172, 206, Byte.MaxValue)) };
                            local_0.Freeze();
                            HoverThumbInnerBorderPen = local_0;
                        }
                    }
                }
                return HoverThumbInnerBorderPen;
            }
        }
        static Pen RenderPressedInnerBorderPen {
            get {
                if (PressedInnerBorderPen == null) {
                    lock (ResourceAccess) {
                        if (PressedInnerBorderPen == null) {
                            var local_0 = new Pen { Brush = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, 131, 143, 218)) };
                            local_0.Freeze();
                            PressedInnerBorderPen = local_0;
                        }
                    }
                }
                return PressedInnerBorderPen;
            }
        }

        Pen InnerBorder {
            get {
                ScrollGlyph scrollGlyph = ScrollGlyph;
                if (!IsEnabled) { return RenderDisabledInnerBorderPen; }
                if (RenderPressed) { return RenderPressedInnerBorderPen; }
                if (RenderMouseOver) {
                    if (scrollGlyph == ScrollGlyph.HorizontalGripper || scrollGlyph == ScrollGlyph.VerticalGripper) {
                        return RenderHoverThumbInnerBorderPen;
                    }
                    return RenderHoverInnerBorderPen;
                }
                return RenderInnerBorderPen;
            }
        }

        static Pen RenderDisabledShadowPen {
            get {
                if (DisabledShadowPen == null) {
                    lock (ResourceAccess) {
                        if (DisabledShadowPen == null) {
                            var local_0 = new Pen();
                            var local_1 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(0.0, 1.0)
                            };
                            local_1.GradientStops.Add(new GradientStop(Color.FromArgb(0, 204, 204, 186), 0.0));
                            local_1.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 204, 204, 186), 0.5));
                            local_1.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 196, 196, 175), 1.0));
                            local_1.Freeze();
                            local_0.Brush = local_1;
                            local_0.Freeze();
                            DisabledShadowPen = local_0;
                        }
                    }
                }
                return DisabledShadowPen;
            }
        }
        static Pen RenderShadowPen {
            get {
                if (ShadowPen == null) {
                    lock (ResourceAccess) {
                        if (ShadowPen == null) {
                            var local_0 = new Pen();
                            var local_1 = new LinearGradientBrush {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(0.0, 1.0)
                            };
                            local_1.GradientStops.Add(new GradientStop(Color.FromArgb(0, 160, 181, 211), 0.0));
                            local_1.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 160, 181, 211), 0.5));
                            local_1.GradientStops.Add(new GradientStop(Color.FromArgb(Byte.MaxValue, 124, 159, 211), 1.0));
                            local_1.Freeze();
                            local_0.Brush = local_1;
                            local_0.Freeze();
                            ShadowPen = local_0;
                        }
                    }
                }
                return ShadowPen;
            }
        }

        Pen Shadow {
            get {
                if (!HasOuterBorder) { return null; }
                return !IsEnabled ? RenderDisabledShadowPen : RenderShadowPen;
            }
        }

        static ScrollChrome() {
            IsEnabledProperty.OverrideMetadata(typeof(ScrollChrome), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ScrollChrome() {
        }

        public static ScrollGlyph GetScrollGlyph(DependencyObject element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            return (ScrollGlyph)element.GetValue(ScrollGlyphProperty);
        }

        public static void SetScrollGlyph(DependencyObject element, ScrollGlyph value) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            element.SetValue(ScrollGlyphProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            Transform = null;
            return new Size(0.0, 0.0);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            Transform = null;
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext) {
            var bounds = new Rect(0.0, 0.0, ActualWidth, ActualHeight);
            ScrollGlyph scrollGlyph = ScrollGlyph;
            Thickness padding = Padding;
            Double num1 = padding.Left + padding.Right;
            if (num1 >= bounds.Width) {
                bounds.Width = 0.0;
            } else {
                bounds.X += padding.Left;
                bounds.Width -= num1;
            }
            Double num2 = padding.Top + padding.Bottom;
            if (num2 >= bounds.Height) {
                bounds.Height = 0.0;
            } else {
                bounds.Y += padding.Top;
                bounds.Height -= num2;
            }
            if (bounds.Width >= 1.0 && bounds.Height >= 1.0) {
                bounds.X += 0.5;
                bounds.Y += 0.5;
                --bounds.Width;
                --bounds.Height;
            }
            switch (scrollGlyph) {
                case ScrollGlyph.LeftArrow:
                case ScrollGlyph.RightArrow:
                case ScrollGlyph.HorizontalGripper:
                    if (bounds.Height >= 1.0) {
                        ++bounds.Y;
                        --bounds.Height;
                    }
                    break;
                case ScrollGlyph.UpArrow:
                case ScrollGlyph.DownArrow:
                case ScrollGlyph.VerticalGripper:
                    if (bounds.Width >= 1.0) {
                        ++bounds.X;
                        --bounds.Width;
                    }
                    break;
            }
            DrawShadow(drawingContext, ref bounds);
            DrawBorders(drawingContext, ref bounds);
            DrawGlyph(drawingContext, scrollGlyph, ref bounds);
        }

        static Boolean IsValidScrollGlyph(Object o) {
            var scrollGlyph = (ScrollGlyph)o;
            switch (scrollGlyph) {
                case ScrollGlyph.None:
                case ScrollGlyph.LeftArrow:
                case ScrollGlyph.RightArrow:
                case ScrollGlyph.UpArrow:
                case ScrollGlyph.DownArrow:
                case ScrollGlyph.VerticalGripper: return true;
                default:
                    return scrollGlyph == ScrollGlyph.HorizontalGripper;
            }
        }

        void DrawShadow(DrawingContext dc, ref Rect bounds) {
            if (bounds.Width <= 0.0 || bounds.Height <= 2.0) { return; }
            Pen shadow = Shadow;
            if (shadow == null) { return; }
            dc.DrawRoundedRectangle(null, shadow, new Rect(bounds.X, bounds.Y + 2.0, bounds.Width, bounds.Height - 2.0), 3.0, 3.0);
            --bounds.Height;
            bounds.Width = Math.Max(0.0, bounds.Width - 1.0);
        }

        void DrawBorders(DrawingContext dc, ref Rect bounds) {
            if (bounds.Width < 2.0 || bounds.Height < 2.0) { return; }
            Brush brush = Fill;
            Pen outerBorder = OuterBorder;
            if (outerBorder != null) {
                dc.DrawRoundedRectangle(brush, outerBorder, new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height), 2.0, 2.0);
                brush = null;
                bounds.Inflate(-1.0, -1.0);
            }
            if (bounds.Width < 2.0 || bounds.Height < 2.0) { return; }
            Pen innerBorder = InnerBorder;
            if (innerBorder == null && brush == null) { return; }
            dc.DrawRoundedRectangle(brush, innerBorder, new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height), 1.5, 1.5);
            bounds.Inflate(-1.0, -1.0);
        }

        void DrawGlyph(DrawingContext dc, ScrollGlyph glyph, ref Rect bounds) {
            if (bounds.Width <= 0.0 || bounds.Height <= 0.0) { return; }
            Brush glyph1 = Glyph;
            if (glyph1 == null) { return; }
            switch (glyph) {
                case ScrollGlyph.LeftArrow:
                case ScrollGlyph.RightArrow:
                case ScrollGlyph.UpArrow:
                case ScrollGlyph.DownArrow:
                    DrawArrow(dc, glyph1, bounds, glyph);
                    break;
                case ScrollGlyph.VerticalGripper:
                    DrawVerticalGripper(dc, glyph1, bounds);
                    break;
                case ScrollGlyph.HorizontalGripper:
                    DrawHorizontalGripper(dc, glyph1, bounds);
                    break;
            }
        }

        void DrawHorizontalGripper(DrawingContext dc, Brush brush, Rect bounds) {
            if (bounds.Width <= 8.0 || bounds.Height <= 2.0) { return; }
            Brush glyphShadow = GlyphShadow;
            Double num1 = Math.Min(6.0, bounds.Height);
            Double num2 = bounds.X + (bounds.Width * 0.5 - 4.0);
            Double y = bounds.Y + (bounds.Height - num1) * 0.5;
            Double height = num1 - 1.0;
            Int32 num3 = 0;
            while (num3 < 8) {
                dc.DrawRectangle(brush, null, new Rect(num2 + num3, y, 1.0, height));
                if (glyphShadow != null)
                    dc.DrawRectangle(glyphShadow, null, new Rect(num2 + num3 + 1.0, y + 1.0, 1.0, height));
                num3 += 2;
            }
        }

        void DrawVerticalGripper(DrawingContext dc, Brush brush, Rect bounds) {
            if (bounds.Width <= 2.0 || bounds.Height <= 8.0) { return; }
            Brush glyphShadow = GlyphShadow;
            Double num1 = Math.Min(6.0, bounds.Width);
            Double x = bounds.X + (bounds.Width - num1) * 0.5;
            Double num2 = bounds.Y + (bounds.Height * 0.5 - 4.0);
            Double width = num1 - 1.0;
            Int32 num3 = 0;
            while (num3 < 8) {
                dc.DrawRectangle(brush, null, new Rect(x, num2 + num3, width, 1.0));
                if (glyphShadow != null) {
                    dc.DrawRectangle(glyphShadow, null, new Rect(x + 1.0, num2 + num3 + 1.0, width, 1.0));
                }
                num3 += 2;
            }
        }

        void DrawArrow(DrawingContext dc, Brush brush, Rect bounds, ScrollGlyph glyph) {
            if (Transform == null) {
                Double val1_1 = 9.0;
                Double val1_2 = 9.0;
                switch (glyph) {
                    case ScrollGlyph.LeftArrow:
                    case ScrollGlyph.RightArrow:
                        val1_1 = 8.0;
                        break;
                    case ScrollGlyph.UpArrow:
                    case ScrollGlyph.DownArrow:
                        val1_2 = 8.0;
                        break;
                }
                var matrix = new Matrix();
                if (bounds.Width < val1_1 || bounds.Height < val1_2) {
                    Double num1 = Math.Min(val1_1, bounds.Width) / val1_1;
                    Double num2 = Math.Min(val1_2, bounds.Height) / val1_2;
                    Double num3 = (bounds.X + bounds.Width * 0.5) / num1 - val1_1 * 0.5;
                    Double num4 = (bounds.Y + bounds.Height * 0.5) / num2 - val1_2 * 0.5;
                    if (
                        Double.IsNaN(num1)
                        || Double.IsInfinity(num1)
                        || Double.IsNaN(num2)
                        || Double.IsInfinity(num2)
                        || Double.IsNaN(num3)
                        || Double.IsInfinity(num3)
                        || Double.IsNaN(num4)
                        || Double.IsInfinity(num4)) {
                        return;
                    }
                    matrix.Translate(num3, num4);
                    matrix.Scale(num1, num2);
                } else {
                    Double offsetX = bounds.X + bounds.Width * 0.5 - val1_1 * 0.5;
                    Double offsetY = bounds.Y + bounds.Height * 0.5 - val1_2 * 0.5;
                    matrix.Translate(offsetX, offsetY);
                }
                Transform = new MatrixTransform {Matrix = matrix};
            }
            dc.PushTransform(Transform);
            switch (glyph) {
                case ScrollGlyph.LeftArrow:
                    dc.DrawGeometry(brush, null, RenderLeftArrowGeometry);
                    break;
                case ScrollGlyph.RightArrow:
                    dc.DrawGeometry(brush, null, RenderRightArrowGeometry);
                    break;
                case ScrollGlyph.UpArrow:
                    dc.DrawGeometry(brush, null, RenderUpArrowGeometry);
                    break;
                case ScrollGlyph.DownArrow:
                    dc.DrawGeometry(brush, null, RenderDownArrowGeometry);
                    break;
            }
            dc.Pop();
        }
    }
}
