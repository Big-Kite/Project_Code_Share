using UnityEngine;

public class TileClickMover : MonoBehaviour
{
    public Camera mainCamera;
    public GridManager gridManager;
    public MovementRangeHighlighter rangeHighlighter;
    public UnitController unit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            world.z = 0f;

            Vector2Int gridPos = gridManager.WorldToGrid(world);
            TileData clickedTile = gridManager.GetTile(gridPos);

            if (clickedTile != null &&
                clickedTile.IsWalkable &&
                !clickedTile.IsOccupied)
            {
                TileData startTile = gridManager.GetTile(unit.GridPosition);
                var path = AStarPathfinder.FindPath(startTile, clickedTile, gridManager);

                Debug.Log($"��� ����: {path.Count - 1}, �̵�����: {rangeHighlighter.moveRange}");
                if (path != null && path.Count - 1 <= rangeHighlighter.moveRange) // �̵� ���� ���� �߰�!
                {
                    var fastPath = AStarPathfinder.TFindPath(startTile, clickedTile, gridManager);

                    gridManager.GetTile(unit.GridPosition).IsOccupied = false;
                    StartCoroutine(unit.FollowPath(fastPath, gridManager));
                }
            }
        }
    }
}
