namespace GdCore.Libraries;

/// <summary>
/// When calling <see cref="NodeExtensionMethods.TrySetupTaggedNodes_Throws"/>,
/// if a matching node is not found in the scene tree, an exception will be thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CriticalNodeAttribute(string nodeName) : Attribute
{
    public readonly string nodeName = nodeName;
}

/// <summary>
/// When calling <see cref="NodeExtensionMethods.TrySetupTaggedNodes_Throws"/>,
/// if a matching node exists in the tree, it will be stored in the tagged member.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class OptionalNodeAttribute(string nodeName) : Attribute
{
    public readonly string nodeName = nodeName;
}

/// <summary>
/// When calling <see cref="NodeExtensionMethods.TrySetupTaggedNodes_Throws"/>,
/// a default node will be constructed and added as a child of the
/// object that owns this field/property. Will use the field/property name as the node name.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AutoCreateNodeAttribute : Attribute
{

}
