using GdCore.Libraries;

namespace GdCore.Services;

public static class ResourceUtils
{
    public static T LoadResource_Throws<T>(ResPath path) where T : class
    {
        T? res = ResourceLoader.Load<T>(path.ToString());
        if (res is null)
            throw new MissingResourceException(path);

        return res;
    }

    /// <summary>
    /// Loads a scene and returns the root node.
    /// Throws if scene cannot be loaded or root node differs from T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="MissingResourceException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static T LoadScene_Throws<T>(ResPath path) where T : Node
    {
        PackedScene scn = ResourceLoader.Load<PackedScene>(path.ToString());
        if (scn is null)
            throw new MissingResourceException(path);

        return scn.Instantiate<T>();
    }

    /// <summary>
    /// Loads a scene and returns its root node. Also adds the scene as a child of the given node.
    /// Throws if scene cannot be loaded or root node differs from T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="MissingResourceException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static T LoadSceneAsChild_Throws<T>(ResPath path, Node parent) where T : Node
    {
        T node = LoadScene_Throws<T>(path);
        parent.AddChild(node);
        return node;
    }
}