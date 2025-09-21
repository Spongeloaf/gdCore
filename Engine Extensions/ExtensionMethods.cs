
using System.Text;


namespace GdCore.Extensions;

public static class ExtensionMethodsEnum
{
    // Formats an enum value like "myEnum" like "My Enum"
    public static string ToPrettyString(this Enum e)
    {
        string enumString = e.ToString();
        bool seenLowerCaseYet = false;
        StringBuilder newResult = new(enumString.Length * 2);
        foreach (char c in enumString)
        {
            if (char.IsUpper(c) && seenLowerCaseYet)
            {
                newResult.Append(' ');
            }
            else if (!char.IsUpper(c))
            {
                // Prevents weirdness like 'Q C' or ' Print'.
                seenLowerCaseYet = true;
            }

            newResult.Append(c);
        }

        return newResult.ToString();
    }
}

public static class ExtensionMethodsRect
{
    // Godots 2d coord space is Y+ == down.
    
    public static Vector2I BottomLeft(this Rect2I rect)
    {
        return new Vector2I(rect.Position.X, rect.End.Y);
    }

    public static Vector2I BottomRight(this Rect2I rect)
    {
        return rect.End;
    }

    public static Vector2I TopLeft(this Rect2I rect)
    {
        return rect.Position;
    }

    public static Vector2I TopRight(this Rect2I rect)
    {
        return new Vector2I(rect.End.X, rect.Position.Y);
    }

    public static Vector2 BottomLeft(this Rect2 rect)
    {
        return new Vector2(rect.Position.X, rect.End.Y);
    }

    public static Vector2 BottomRight(this Rect2 rect)
    {
        return rect.End;
    }

    public static Vector2 TopLeft(this Rect2 rect)
    {
        return rect.Position;
    }

    public static Vector2 TopRight(this Rect2 rect)
    {
        return new Vector2(rect.End.X, rect.Position.Y);
    }
}