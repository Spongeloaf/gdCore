using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GdCore
{
    public enum TweenProp
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


    /// <summary>
    /// A C# friendly wrapper around the godot tween that allows use of enums for properties.
    /// The tween is created in the tree of the target node.
    /// </summary>
    public static class BTween
    {
        public static string ToPropertyString(this TweenProp prop)
        {
            return prop switch
            {
                TweenProp.Position => "position",
                TweenProp.Scale => "scale",
                TweenProp.Rotation => "rotation",
                TweenProp.RotationDegrees => "rotation_degrees",
                TweenProp.Modulate => "modulate",
                TweenProp.GlobalPosition => "global_position",
                TweenProp.Offset => "offset",
                TweenProp.Opacity => "opacity",
                TweenProp.AnchorLeft => "anchor_left",
                TweenProp.AnchorTop => "anchor_top",
                TweenProp.AnchorRight => "anchor_right",
                TweenProp.AnchorBottom => "anchor_bottom",
                TweenProp.MarginLeft => "margin_left",
                TweenProp.MarginTop => "margin_top",
                TweenProp.MarginRight => "margin_right",
                TweenProp.MarginBottom => "margin_bottom",
                _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null),
            };
        }

        public static void TweenProperty(Node target, TweenProp prop, Variant finalValue, double duration, Tween.TransitionType transition = Tween.TransitionType.Linear, Tween.EaseType ease = Tween.EaseType.InOut)
        {
            Tween tween = target.GetTree().CreateTween();
            string propName = prop.ToPropertyString();
            tween.TweenProperty(target, propName, finalValue, duration)
                .SetTrans(transition)
                .SetEase(ease);
        }
    }

}
