
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

}