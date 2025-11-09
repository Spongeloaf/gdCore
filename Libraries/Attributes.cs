namespace GdCore.Libraries;

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

/// <summary>
/// Tags a node for auto-generation. A default node will be constructed and added as a child of the
/// object that owns this field/property. Will use the field/property name as the node name.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AutoCreateNodeAttribute : Attribute
{

}
