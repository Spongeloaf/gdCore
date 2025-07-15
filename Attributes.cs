
namespace GdCore;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CriticalNodeAttribute(string nodeName) : Attribute
{
    public readonly string NodeName = nodeName;
}
