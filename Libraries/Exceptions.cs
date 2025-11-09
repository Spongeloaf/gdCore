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