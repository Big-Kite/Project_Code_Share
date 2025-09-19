using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public GridManager manager;

    private void Start()
    {
        SetPosition(new Vector2Int(3, 3));
    }
    public void SetPosition(Vector2Int gridPos)//, GridManager gridManager)
    {
        GridPosition = gridPos;
        Vector3 worldPos = manager.GridToWorld(gridPos.x, gridPos.y);
        //transform.position = worldPos;

        StopAllCoroutines(); // 이동 중이면 중지
        StartCoroutine(MoveTo(worldPos)); // 부드럽게 이동 시작
        //
        //// 해당 타일에 점유 정보 반영
        //var tile = manager.GetTile(gridPos);
        //if (tile != null)
        //    tile.IsOccupied = true;
    }
    private IEnumerator MoveTo(Vector3 target)
    {
        float duration = 0.25f; // 이동 시간
        float time = 0f;
        Vector3 start = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null; // 다음 프레임까지 잠시 멈춤
        }

        transform.position = target; // 마지막 위치 정확하게 정리
    }
    public IEnumerator FollowPath(List<TileData> path, GridManager gridManager)
    {
        foreach (var step in path)
        {
            Vector3 target = gridManager.GridToWorld(step.GridPos.x, step.GridPos.y);
            float duration = 0.15f;
            float time = 0f;
            Vector3 start = transform.position;

            while (time < duration)
            {
                transform.position = Vector3.Lerp(start, target, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
            GridPosition = step.GridPos;
        }

        // 마지막 타일 점유 표시
        var finalTile = gridManager.GetTile(GridPosition);
        if (finalTile != null)
            finalTile.IsOccupied = true;

        // 이동 후 이동 범위 다시 표시
        FindObjectsByType<MovementRangeHighlighter>(FindObjectsSortMode.None).FirstOrDefault().unitPos = GridPosition;
        FindObjectsByType<MovementRangeHighlighter>(FindObjectsSortMode.None).FirstOrDefault().HighlightMovementRange();
    }
}
