using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float tileWidth = 1.4f;
    public float tileHeight = 1f;
    public GameObject tileViewPrefab;

    private TileData[,] grid;

    public TileData[,] Grid => grid;

    void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new TileData[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileData tile = new TileData(x, y);
                Vector3 worldPos = GridToWorld(x, y);

                GameObject tileObj = Instantiate(tileViewPrefab, worldPos, Quaternion.identity, transform);
                TileView view = tileObj.GetComponent<TileView>();
                tile.View = view;

                grid[x, y] = tile;
            }
        }

        grid[4, 3].IsWalkable = false;
        grid[4, 4].IsWalkable = false;
        grid[4, 5].IsWalkable = false;

        grid[5, 6].IsWalkable = false;
        grid[6, 6].IsWalkable = false;
        grid[7, 6].IsWalkable = false;
    }

    public Vector3 GridToWorld(int x, int y)
    {
        float isoX = (x - y) * tileWidth / 2f;
        float isoY = (x + y) * tileHeight / 2f;
        return new Vector3(isoX, isoY, 0);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.y / (tileHeight / 2f) + worldPos.x / (tileWidth / 2f)) / 2f);
        int y = Mathf.RoundToInt((worldPos.y / (tileHeight / 2f) - worldPos.x / (tileWidth / 2f)) / 2f);
        return new Vector2Int(x, y);
    }

    public TileData GetTile(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height) return null;
        return grid[pos.x, pos.y];
    }
}
