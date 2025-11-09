namespace GdCore.Libraries;

public static class ParsingUtils
{
    /// <summary>
    /// Parses an enumerator from a string. If the parsing fails, the supplied default value is returned.
    /// </summary>
    /// <returns>Parsed unsigned integer or the default value</returns>
    public static TE EnumParse<TE>(string toParse, TE defaultValue) where TE : struct
    {
        try
        {
            return Enum.Parse<TE>(toParse, true);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Parses an integer from a string. If the parsing fails, the supplied default value is returned
    /// </summary>
    /// <param name="intStr"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int ParseInt(string intStr, int defaultValue)
    {
        if (Int32.TryParse(intStr, out int result))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Parses an unsigned integer from a string. If the parsing fails, the supplied default value is returned.
    /// </summary>
    /// <returns>Parsed unsigned integer or the default value</returns>
    public static uint ParseUInt(string uintStr, uint defaultValue)
    {
        if (uint.TryParse(uintStr, out uint result))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Parses a float from a string. If the parsing fails, the supplied default value is returned.
    /// </summary>
    /// <returns>Parsed float or the default value</returns>
    public static float ParseFloat(string floatStr, float defaultValue)
    {
        if (float.TryParse(floatStr, out float result))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Parses a boolean from a string. If the parsing fails, the supplied default value is returned.
    /// </summary>
    /// <returns>Parsed boolean or the default value</returns>
    public static bool ParseBool(string boolStr, bool defaultValue)
    {
        if (bool.TryParse(boolStr, out bool result))
            return result;

        return defaultValue;
    }
}
