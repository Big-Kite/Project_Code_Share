using System.Collections.Generic;
using UnityEngine;

public class MovementRangeHighlighter : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int unitPos;
    public int moveRange = 3;
    private static readonly Color HighlightColor = new Color(0f, 1f, 1f, 0.5f); // 투명 또는 매우 어둡게
    private static readonly Color UnWalkableColor = new Color(1f, 0f, 0f, 0.5f); // 투명 또는 매우 어둡게

    public List<TileData> CurrentRangeTiles { get; private set; } = new();
    private void Start()
    {
        HighlightMovementRange();
    }
    public void HighlightMovementRange()
    {
        ClearHighlights();

        HashSet<TileData> visited = new();
        Queue<(TileData tile, int cost)> frontier = new();

        TileData startTile = gridManager.GetTile(unitPos);
        if (startTile == null) return;

        visited.Add(startTile);
        frontier.Enqueue((startTile, 0));

        while (frontier.Count > 0)
        {
            var (currentTile, cost) = frontier.Dequeue();

            if (cost > moveRange) continue;

            currentTile.View.SetHighlight(HighlightColor); // 표시 색상
            CurrentRangeTiles.Add(currentTile);

            foreach (var neighbor in GetNeighbors(currentTile))
            {
                if (visited.Contains(neighbor)) continue;
                if (!neighbor.IsWalkable || neighbor.IsOccupied) continue;

                visited.Add(neighbor);
                frontier.Enqueue((neighbor, cost + 1));
            }
        }
        //ClearHighlights();
        //CurrentRangeTiles = GetTilesInRange(unitPos, moveRange);
        //
        //foreach (var tile in CurrentRangeTiles)
        //{
        //    tile.View.SetHighlight(Color.cyan);
        //}
    }
    private List<TileData> GetNeighbors(TileData tile)
    {
        var dirs = new Vector2Int[]
        {
        new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        };

        List<TileData> neighbors = new();
        foreach (var dir in dirs)
        {
            var pos = tile.GridPos + dir;
            var neighbor = gridManager.GetTile(pos);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }
    public void ClearHighlights()
    {
        foreach (var tile in gridManager.Grid)
        {
            if(!tile.IsWalkable)
            {
                tile.View.SetHighlight(UnWalkableColor); // 표시 색상
                continue;
            }
            
            tile.View.ClearHighlight();
        }
    }

    List<TileData> GetTilesInRange(Vector2Int center, int range)
    {
        List<TileData> result = new List<TileData>();

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                if (Mathf.Abs(dx) + Mathf.Abs(dy) <= range)
                {
                    Vector2Int pos = center + new Vector2Int(dx, dy);
                    TileData tile = gridManager.GetTile(pos);
                    if (tile != null && tile.IsWalkable)
                        result.Add(tile);
                }
            }
        }

        return result;
    }
}
