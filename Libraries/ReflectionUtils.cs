using System.Reflection;

namespace GdCore.Libraries;

public static class ReflectionUtils
{
    public static MemberInfo? FindMember<T>(T obj, string memberName) where T : class
    {
        Type type = obj.GetType();
        foreach (MemberInfo mi in type.GetMembers())
            if (mi.Name == memberName)
                return mi;

        return null;
    }
}