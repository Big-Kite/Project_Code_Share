using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    public static List<TileData> FindPath(TileData start, TileData goal, GridManager gridManager)
    {
        var openSet = new PriorityQueue<TileData>();
        var cameFrom = new Dictionary<TileData, TileData>();
        var gScore = new Dictionary<TileData, int>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            TileData current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (var neighbor in GetNeighbors(current, gridManager))
            {
                if (!neighbor.IsWalkable || neighbor.IsOccupied) continue;

                int tentativeG = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;

                    int fScore = tentativeG + Heuristic(neighbor.GridPos, goal.GridPos);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return null; // 길 없음
    }
    public static List<TileData> TFindPath(TileData start, TileData goal, GridManager gridManager)
    {
        var openSet = new PriorityQueue<TileData>();
        var cameFrom = new Dictionary<TileData, TileData>();
        var gScore = new Dictionary<TileData, int>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            TileData current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (var neighbor in TGetNeighbors(current, gridManager))
            {
                if (!neighbor.IsWalkable || neighbor.IsOccupied) continue;

                int tentativeG = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;

                    int fScore = tentativeG + Heuristic(neighbor.GridPos, goal.GridPos);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return null; // 길 없음
    }

    static int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan
    }

    static List<TileData> ReconstructPath(Dictionary<TileData, TileData> cameFrom, TileData current)
    {
        List<TileData> totalPath = new List<TileData> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }

    static List<TileData> GetNeighbors(TileData tile, GridManager gridManager)
    {
        var directions = new Vector2Int[]
        {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        };

        List<TileData> neighbors = new();
        foreach (var dir in directions)
        {
            var pos = tile.GridPos + dir;
            var neighbor = gridManager.GetTile(pos);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    static List<TileData> TGetNeighbors(TileData tile, GridManager gridManager)
    {
        var directions = new Vector2Int[]
        {
            new(1, 1), new(-1, 1), new(1, -1), new(-1, -1),
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        };

        List<TileData> neighbors = new();
        foreach (var dir in directions)
        {
            var pos = tile.GridPos + dir;
            var neighbor = gridManager.GetTile(pos);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }
}
