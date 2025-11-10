namespace GdCore;

public enum NodeProperty
{
    Position,
    Scale,
    Rotation,
    RotationDegrees,
    Modulate,
    GlobalPosition,
    Offset,
    Opacity,
    AnchorLeft,
    AnchorTop,
    AnchorRight,
    AnchorBottom,
    MarginLeft,
    MarginTop,
    MarginRight,
    MarginBottom
}

public enum AnimationProperty
{
    RotationY,
}

public static class EnumExtensions
{

    public static string ToPropertyString(this NodeProperty prop)
    {
        return prop switch
        {
            NodeProperty.Position => "position",
            NodeProperty.Scale => "scale",
            NodeProperty.Rotation => "rotation",
            NodeProperty.RotationDegrees => "rotation_degrees",
            NodeProperty.Modulate => "modulate",
            NodeProperty.GlobalPosition => "global_position",
            NodeProperty.Offset => "offset",
            NodeProperty.Opacity => "opacity",
            NodeProperty.AnchorLeft => "anchor_left",
            NodeProperty.AnchorTop => "anchor_top",
            NodeProperty.AnchorRight => "anchor_right",
            NodeProperty.AnchorBottom => "anchor_bottom",
            NodeProperty.MarginLeft => "margin_left",
            NodeProperty.MarginTop => "margin_top",
            NodeProperty.MarginRight => "margin_right",
            NodeProperty.MarginBottom => "margin_bottom",
            _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null),
        };
    }

    public static string ToPropertyString(this AnimationProperty prop)
    {
        return prop switch
        {
            AnimationProperty.RotationY => ":rotation:y",
            _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
        };
    }
}