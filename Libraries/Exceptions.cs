namespace GdCore.Libraries;

[Serializable]
public abstract class GdException : Exception
{
    protected GdException()
    { }

    protected GdException(string message)
        : base(message)
    { }

    protected GdException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

[Serializable]
public class MissingCriticalNodeException : GdException
{
    public MissingCriticalNodeException()
    { }

    public MissingCriticalNodeException(string message)
        : base(message)
    { }

    public MissingCriticalNodeException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

[Serializable]
public class AutoGenerateNodeFailedException : GdException
{
    public AutoGenerateNodeFailedException()
    { }

    public AutoGenerateNodeFailedException(string message)
        : base(message)
    { }

    public AutoGenerateNodeFailedException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

[Serializable]
public class MissingResourceException : GdException
{
    private static string MakeMessage(string message)
    {
        return "Failed to locate resource: " + message;
    }

    public MissingResourceException()
    {
    }

    public MissingResourceException(ResPath missingResource)
        : base(MakeMessage(missingResource.ToString()))
    {
    }

    public MissingResourceException(string message)
        : base(MakeMessage(message))
    {
    }

    public MissingResourceException(ResPath missingResource, Exception innerException)
        : base(MakeMessage(missingResource.ToString()), innerException)
    {
    }
}

/// <summary>
/// A TODO style exception you can throw for interfaces or methods that should be replaced.
/// The message should contain a reason for this code being condemned.
/// </summary>
[Serializable]
public class CondemnedCodeException : GdException
{
    private const string prefix = "This code has been flagged to be replaced: ";

    /// <inheritdoc cref="CondemnedCodeException"/>
    public CondemnedCodeException(string replaceWithWhat)
        : base(prefix + replaceWithWhat)
    { }

    /// <inheritdoc cref="CondemnedCodeException"/>
    public CondemnedCodeException(string replaceWithWhat, Exception innerException)
        : base(prefix + replaceWithWhat, innerException)
    { }
}

[Serializable]
public class SceneTreeLayoutException : GdException
{
    public SceneTreeLayoutException()
    { }

    public SceneTreeLayoutException(string message)
        : base(message)
    { }

    public SceneTreeLayoutException(string message, Exception innerException)
        : base(message, innerException)
    { }
}