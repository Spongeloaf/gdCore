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

    public static object? GetMemberValue(MemberInfo memberInfo, object? forObject)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                return ((FieldInfo)memberInfo).GetValue(forObject);
            case MemberTypes.Property:
                return ((PropertyInfo)memberInfo).GetValue(forObject);
            default:
                throw new NotImplementedException();
        }
    }
}