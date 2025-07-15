
using System.Text;

namespace GdCore;

public static class ExtensionMethods
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