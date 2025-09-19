using UnityEngine;

public class TileData
{
    public Vector2Int GridPos { get; private set; }
    public bool IsWalkable { get; set; } = true;
    public bool IsOccupied { get; set; } = false;

    public TileView View; // ����� �ð� ������Ʈ

    public TileData(int x, int y)
    {
        GridPos = new Vector2Int(x, y);
    }
}
