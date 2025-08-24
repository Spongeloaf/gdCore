namespace GdCore.CodeUtils;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CriticalNodeAttribute(string nodeName) : Attribute
{
    public readonly string NodeName = nodeName;
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class OptionalNodeAttribute(string nodeName) : Attribute
{
    public readonly string NodeName = nodeName;
}