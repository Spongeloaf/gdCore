using System.Reflection;
using GdCore.Libraries;
using GdCore.Services;

using JetBrains.Annotations;

namespace GdCore;

public static class NodeExtensionMethods
{
    public static List<T> FindDirectChildrenExcluding<T, E>(this Node node) where T : Node
    {
        List<T> results = new List<T>();
        foreach (Node child in node.GetChildren())
        {
            switch (child)
            {
                case E:
                    continue;
                case T candidate:
                    results.Add(candidate);
                    break;
            }
        }
        return results;
    }

    public static List<T> FindDirectChildren<T>(this Node node) where T : Node
    {
        List<T> results = new List<T>();
        foreach (Node child in node.GetChildren())
        {
            if (child is T candidate)
                results.Add(candidate);
        }
        return results;
    }

    /// <summary>
    /// Returns the first child that is directly below 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns>Null if no candidate is found</returns>
    public static T? FindFirstDirectChild<T>(this Node node) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T candidate)
                return candidate;
        }
        return null;
    }

    /// <summary>
    /// Returns the first child that is directly below,
    /// where the name of the node matches the passed-in string, ignoring case.
    /// 
    /// Godot's node paths are case-sensitive, but this project, in general, is not.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <param name="name"></param>
    /// <returns>Null if no candidate is found</returns>
    public static T? FindFirstDirectChild<T>(this Node node, string name) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T candidate &&
                string.Compare(child.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
                return candidate;
        }
        return null;
    }

    /// <summary>
    /// Returns the first instance of the given node type anywhere in the tree below the node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns>Null if no candidate is found</returns>
    public static T? FindFirstChildRecursive<T>(this Node node) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T candidate)
                return candidate;

            T? result = FindFirstChildRecursive<T>(child);
            if (GodotObject.IsInstanceValid(result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Returns the first instance of the given node type anywhere in the tree below the node,
    /// where the name of the node matches the passed-in string, ignoring case.
    ///
    /// Godot's node paths are case-sensitive, but this project, in general, is not.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <param name="name">Name of the node we're searching for</param>
    /// <returns>Null if no candidate is found</returns>
    public static T? FindFirstChildRecursive<T>(this Node node, string name) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T candidate &&
                string.Compare(child.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
                return candidate;

            T? result = FindFirstChildRecursive<T>(child, name);
            if (GodotObject.IsInstanceValid(result))
                return result;
        }
        return null;
    }

    public static List<T> FindAllChildrenRecursive<T>(this Node node) where T : Node
    {
        List<T> result = new();
        node.FindAllChildrenRecursive_Impl(result);
        return result;
    }

    private static void FindAllChildrenRecursive_Impl<T>(this Node node, List<T> output) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T candidate)
                output.Add(candidate);

            FindAllChildrenRecursive_Impl(child, output);
        }
    }


    public static void TakeOwnershipOfTree(this Node owner, Node node)
    {
        if (!owner.IsAncestorOf(node) && node != owner)
        {
            Log.GameError("Error in TakeOwnershipOfTree: owner is not an ancestor of node");
            return;
        }

        node.TakeOwnership(owner);
    }

    private static void TakeOwnership(this Node node, Node owner)
    {
        node.Owner = owner;

        foreach (Node child in node.GetChildren())
            child.TakeOwnership(owner);
    }

    /// <summary>
    /// Removes a node from its parent and adds it as a child of another parent, with the supplied owner.
    /// </summary>
    /// <param name="child"></param>
    /// <param name="newParent"></param>
    public static void AdoptNode(this Node newParent, Node child, Node owner)
    {
        Node? oldParent = child.GetParent();
        if (oldParent is not null)
        {
            oldParent.RemoveChild(child);
        }

        newParent.AddChild(child);
        child.Owner = owner;
    }

    /// <summary>
    /// Removes a node from its parent and adds it as a child of another parent.
    /// This overload uses the parent as the owner as well.
    /// </summary>
    /// <param name="child"></param>
    /// <param name="newParent"></param>
    public static void AdoptNode(this Node newParent, Node child)
    {
        newParent.AdoptNode(child, newParent);
    }

    /// <summary>
    /// Removes a node from its parent and adds it as a child of another parent.
    /// This overload uses the parent as the owner as well.
    /// </summary>
    /// <param name="child"></param>
    /// <param name="newParent"></param>
    public static void AdoptNodes(this Node newParent, IEnumerable<Node> nodes)
    {
        foreach (Node child in nodes) 
            newParent.AdoptNode(child, newParent);
    }

    public static T CreateParentedChild<T>(this Node parent, Node sceneRoot, string name) where T : Node, new()
    {
        T node = new();
        node.AdoptNode(parent, sceneRoot);
        node.Name = name;
        return node;
    }

    public static T CreateChild<T>(this Node parent) where T : Node, new()
    {
        T node = new();
        node.Name = typeof(T).Name;
        parent.AddChild(node);
        return node;
    }

    public static T CreateParentedChild<T>(this Node parent) where T : Node, new()
    {
        return parent.CreateParentedChild<T>(parent);
    }

    public static T CreateParentedChild<T>(this Node parent, Node sceneRoot) where T : Node, new()
    {
        return parent.CreateParentedChild<T>(sceneRoot, typeof(T).Name);
    }

    public static T? FindAncestor<T>(this Node node) where T : Node
    {
        return node.FindAncestor_Impl<T>();
    }

    private static T? FindAncestor_Impl<T>(this Node node) where T : class
    {
        while (true)
        {
            if (node.IsNodeInvalid())
                return null;

            Node? parent = node.GetParent();
            if (parent.IsNodeInvalid())
                return null;

            if (parent is T t)
                return t;

            node = parent;
        }
    }

    // TODO: Move this to spatial
    public static void AddChildrenInAGridPattern(this Node parent, List<Node3D> children, float spacing)
    {
        // + 0.5f makes sure we round up 
        int rows = (int)Mathf.Round(Mathf.Sqrt(children.Count) + 0.5f);
        int index = 0;
        Vector3 position = Vector3.Zero;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (index >= children.Count)
                    break;

                Node3D child = children[index];
                parent.AddChild(child);
                child.Position = position;
                position.X += spacing;
                index++;
            }
            position.Z += spacing;
            position.X = 0;
        }
    }

    /// <summary>
    /// Removes (but does not delete) all Children from a node
    /// </summary>
    /// <param name="parent"></param>
    public static void DetachAllChildren(this Node parent)
    {
        foreach (Node child in parent.GetChildren())
            parent.RemoveChild(child);
    }

    public static void DeleteAllChildren(this Node parent)
    {
        foreach (Node child in parent.GetChildren())
            child.QueueFree();
    }

    [Obsolete($"Use {nameof(IsDeletedOrNull)}() instead. Same functionality, more auto-complete friendly.")]
    [ContractAnnotation("null => true; notnull => false")]
    public static bool IsNodeInvalid(this Node? node)
    {
        return IsDeletedOrNull(node);
    }

    /// <summary>
    /// Returns true if the node reference is null, or points to a node that has been queued for deletion.
    /// </summary>
    [ContractAnnotation("null => true; notnull => false")]
    public static bool IsDeletedOrNull(this Node? node)
    {
        if (node is null)
            return true;

        return !GodotObject.IsInstanceValid(node);
    }

    /// <summary>
    /// Finds all children in the scene who match fields/properties on the parent node which are
    /// tagged with CriticalNodeAttribute or OptionalNodeAttribute.
    /// <br/>
    /// Throws MissingCriticalNodeException if any critical node fails. Optional nodes DO NOT throw
    /// if they are not found in the scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node">The Node-derived class which is being operated on</param>
    /// <exception cref="MissingCriticalNodeException">
    /// If any critical node fails to assign for any reason.
    /// If the reason is code-related, i.e. the reflection functions fail, then the actual
    /// exception will be wrapped in this exception.</exception>
    public static void TryLocateSceneNodes_Throws(this Node node) 
    {
    // TODO: Would be rad to have a "Crash the game gracefully" system where I could cleanly display a popup message,
    // shut down the game, and dump some logs. Then instead of throwing, I could do that when a critical node fails.
        TypeInfo to = node.GetType().GetTypeInfo();
        FieldInfo[] fields = to.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        bool foundAnyTaggedFields = false;
        List<string> failedNodes = [];
        foreach (FieldInfo field in fields)
        {
            // TODO: If you add more of these, make it a switch please.
            OptionalNodeAttribute? optional = field.GetCustomAttribute<OptionalNodeAttribute>();
            if (optional is not null)
            {
                LocateOptionalNode(node, field, optional.NodeName);
                continue;
            }

            CriticalNodeAttribute? attr = field.GetCustomAttribute<CriticalNodeAttribute>();
            if (attr is null)
                continue;

            foundAnyTaggedFields = true;
            if (!LocateCriticalNode(node, field, attr.NodeName)) 
                failedNodes.Add(attr.NodeName);
        }

        if (failedNodes.Count != 0)
            throw new MissingCriticalNodeException($"Failed to locate critical nodes: {string.Join(", ", failedNodes)}");

        if (!foundAnyTaggedFields) 
            Log.Debug("{0} does not contain any member nodes tagged as 'Critical'", node.Name);
    }

    private static bool LocateCriticalNode(Node node, FieldInfo nodeInfo, string name)
    {
        // TODO: Consider using a better method that is not case-sensitive
        Node? candidate = node.FindChild(name, true, false);
        if (IsInvalidCandidate(candidate, nodeInfo))
            return false;

        try
        {
            nodeInfo.SetValue(node, candidate);
        }
        catch (Exception e)
        {
            throw new MissingCriticalNodeException($"Failed to assign node {name}: {e.GetType().Name}", e);
        }

        Log.Debug("Found node {0} at {1}", name, candidate.GetPath());
        return true;
    }

    private static void LocateOptionalNode(Node parent, FieldInfo nodeInfo, string name)
    {
        // TODO: Consider using a better method that is not case-sensitive
        Node? candidate = parent.FindChild(name, true, false);
        if (IsInvalidCandidate(candidate, nodeInfo))
            return;

        try
        {
            nodeInfo.SetValue(parent, candidate);
        }
        catch
        {
            // nothing to do
        }

        Log.Debug("Found optional node {0} at {1}", name, candidate.GetPath());
        return;
    }

    private static bool IsInvalidCandidate(Node? candidate, FieldInfo nodeInfo)
    {
        if (candidate is null)
            return true;

        return !candidate.GetType().IsSubclassOf(nodeInfo.FieldType) && candidate.GetType() != nodeInfo.FieldType;
    }

    public static void UnparentNode(this Node? node)
    {
        if (node.IsDeletedOrNull())
            return;

        Node? parent = node.GetParent();
        if (parent.IsDeletedOrNull())
            return;

        parent.RemoveChild(node);
    }
}