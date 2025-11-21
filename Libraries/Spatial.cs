
using System.Numerics;
using GdCore;
using GdCore.Engine_Extensions;
using Godot.Collections;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public static class Spatial
{
    public static Rect2 GetGlobalBoundingBox(Node2D node)
    {
        Rect2? boundingBox = null;

        void Traverse(Node current)
        {
            if (current is CanvasItem canvasItem && canvasItem.Visible)
            {
                // Check if it has a visual rect (like a Sprite2D or Control)
                Rect2? rect = GetItemBounds(canvasItem);

                if (rect != null)
                {
                    if (boundingBox == null)
                        boundingBox = rect;
                    else
                        boundingBox = boundingBox.Value.Merge(rect.Value);
                }
            }

            foreach (Node child in current.GetChildren())
            {
                Traverse(child);
            }
        }

        Traverse(node);

        return boundingBox ?? new Rect2(node.GlobalPosition, Vector2.Zero);
    }

    public static Rect2? GetItemBounds(CanvasItem item)
    {
        switch (item)
        {
            case Sprite2D sprite when sprite.Texture != null:
                var textureSize = sprite.Texture.GetSize() * sprite.Scale;
                var topLeft = sprite.GetGlobalTransformWithCanvas().Origin - textureSize * sprite.GetRect().GetCenter();
                return new Rect2(topLeft, textureSize);

            case Control control:
                var globalPos = control.GetGlobalTransformWithCanvas().Origin;
                return new Rect2(globalPos, control.Size);

            case CollisionShape2D collider when collider.Shape is RectangleShape2D rect:
                var extents = rect.Size * 0.5f;
                var center = collider.GetGlobalTransformWithCanvas().Origin;
                return new Rect2(center - extents, rect.Size);

            case TileMapLayer tileMapLayer:
                Vector2I sizeInPixels = tileMapLayer.GetUsedRect().End * tileMapLayer.TileSet.TileSize;
                return new Rect2(tileMapLayer.GlobalPosition, sizeInPixels);
        }

        return null;
    }

    public static Node3D WrapNodeInScaledParent(Node3D scene, float size)
    {
        Node3D scalarNode = new Node3D();
        if (scene.IsNodeInvalid())
            return scalarNode;

        Node? parent = scene.GetParent();
        if (!parent.IsNodeInvalid())
            parent.RemoveChild(scene);

        Aabb box = GetAaabbThatFitsScene(scene);

        float largestDimension = Mathf.Max(box.Size.X, box.Size.Y);
        largestDimension = Mathf.Max(largestDimension, box.Size.Z);
        largestDimension = Mathf.Max(largestDimension, 0.1f); // Prevent division by 0
        scalarNode.AddChild(scene);
        scalarNode.Scale *= size / largestDimension;
        scene.Position = -box.GetCenter();
        return scalarNode;
    }

    /// <summary>
    /// Takes a 3D scene tree and finds a scalar that can shrink it to fit the given dimension
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="size"></param>
    public static Aabb GetAaabbThatFitsScene(Node3D scene)
    {
        Aabb box = new();

        if (scene is MeshInstance3D sceneMesh)
        {
            Aabb meshBox = sceneMesh.GetAabb();
            meshBox *= sceneMesh.Transform;
            box = box.Merge(meshBox);
        }

        List<MeshInstance3D> meshes = scene.FindAllChildrenRecursive<MeshInstance3D>();
        foreach (MeshInstance3D mesh in meshes)
        {
            Aabb meshBox = mesh.GetAabb();
            meshBox *= mesh.Transform;
            box = box.Merge(meshBox);
        }

        return box;
    }

    public static RayCastHit3D DoRayCastPointToPoint(World3D world, Vector3 a, Vector3 b, Rid[]? excludes = null, uint collisionMask = 1)
    {
        Vector3 direction = a.DirectionTo(b);
        float distance = a.DistanceTo(b);
        return DoRayCastInDirection(world, a, direction, excludes, collisionMask, distance);
    }

    public static RayCastHit3D DoRayCastInDirection(World3D world, Vector3 from, Vector3 direction, Rid[]? excludes = null, uint collisionMask = 1, float length = 10000)
    {
        direction = direction.Normalized();
        Vector3 to = from + direction * length;

        Array<Rid>? excludes_array = [];
        if (excludes is not null)
        {
            foreach (Rid id in excludes)
                excludes_array.Add(id);
        }

        var query = PhysicsRayQueryParameters3D.Create(from, to, collisionMask, excludes_array);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;
        Dictionary result = world.DirectSpaceState.IntersectRay(query);
        RayCastHit3D hit = new(result);

        // Always return the point limited to 'length' argument
        if (hit.position == Vector3.Inf)
            hit.position = to;

        return hit;
    }
}