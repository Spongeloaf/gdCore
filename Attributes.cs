namespace GdCore;

public class CriticalNodeAttribute(string nodeName) : Attribute
{
    public readonly string NodeName = nodeName;
}