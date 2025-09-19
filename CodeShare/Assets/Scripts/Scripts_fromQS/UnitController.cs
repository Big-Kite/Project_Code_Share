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

        StopAllCoroutines(); // �̵� ���̸� ����
        StartCoroutine(MoveTo(worldPos)); // �ε巴�� �̵� ����
        //
        //// �ش� Ÿ�Ͽ� ���� ���� �ݿ�
        //var tile = manager.GetTile(gridPos);
        //if (tile != null)
        //    tile.IsOccupied = true;
    }
    private IEnumerator MoveTo(Vector3 target)
    {
        float duration = 0.25f; // �̵� �ð�
        float time = 0f;
        Vector3 start = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ��� ����
        }

        transform.position = target; // ������ ��ġ ��Ȯ�ϰ� ����
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

        // ������ Ÿ�� ���� ǥ��
        var finalTile = gridManager.GetTile(GridPosition);
        if (finalTile != null)
            finalTile.IsOccupied = true;

        // �̵� �� �̵� ���� �ٽ� ǥ��
        FindObjectsByType<MovementRangeHighlighter>(FindObjectsSortMode.None).FirstOrDefault().unitPos = GridPosition;
        FindObjectsByType<MovementRangeHighlighter>(FindObjectsSortMode.None).FirstOrDefault().HighlightMovementRange();
    }
}
