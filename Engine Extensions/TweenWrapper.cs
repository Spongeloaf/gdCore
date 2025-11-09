namespace GdCore;

/// <summary>
/// A C# friendly wrapper around the godot tween that allows use of enums for properties.
/// The tween is created in the tree of the target node.
/// </summary>
public static class BTween
{
    public static Tween TweenProperty(
        Node target,
        NodeProperty prop,
        Variant finalValue,
        double duration,
        Tween.TransitionType transition = Tween.TransitionType.Linear,
        Tween.EaseType ease = Tween.EaseType.InOut)
    {
        Tween tween = target.GetTree().CreateTween();
        string propName = prop.ToPropertyString();
        tween.TweenProperty(target, propName, finalValue, duration)
            .SetTrans(transition)
            .SetEase(ease);

        return tween;
    }

    public static Tween TweenProperty(
        this Tween tween, Node target,
        NodeProperty prop,
        Variant finalValue,
        double duration,
        Tween.TransitionType transition = Tween.TransitionType.Linear,
        Tween.EaseType ease = Tween.EaseType.InOut)
    {
        string nodepath = prop.ToPropertyString();
        tween.TweenProperty(target, nodepath, finalValue, duration)
            .SetTrans(transition)
            .SetEase(ease);

        return tween;
    }
}