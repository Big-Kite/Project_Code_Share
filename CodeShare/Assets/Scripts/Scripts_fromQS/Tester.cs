using UnityEngine;

public class Tester : MonoBehaviour
{
    public GameObject tilePrefab; // IsoTile ÇÁ¸®ÆÕ
    public int width = 5;
    public int height = 5;
    public float tileWidth = 1f;
    public float tileHeight = 0.5f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = GridToIso(x, y);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.name = $"Tile {x},{y}";
            }
        }
    }

    Vector3 GridToIso(int x, int y)
    {
        float isoX = (x - y) * tileWidth / 2f;
        float isoY = (x + y) * tileHeight / 2f;
        return new Vector3(isoX, isoY, 0f);
    }
}
