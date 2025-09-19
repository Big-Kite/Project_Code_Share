using UnityEngine;

public class TileData
{
    public Vector2Int GridPos { get; private set; }
    public bool IsWalkable { get; set; } = true;
    public bool IsOccupied { get; set; } = false;

    public TileView View; // 연결된 시각 오브젝트

    public TileData(int x, int y)
    {
        GridPos = new Vector2Int(x, y);
    }
}
