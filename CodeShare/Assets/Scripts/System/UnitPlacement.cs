using System.Collections.Generic;
using UnityEngine;

public class UnitPlacement : Singleton<UnitPlacement>
{
    [SerializeField] BoxCollider2D area;
    public Bounds PlayerMoveBounds { get { return area.bounds; } }

    public void SetUnitPlace(BattleUnit _player, List<BattleUnit> _mons)
    {
        Bounds bounds = area.bounds;

        Vector3 playerPos = new Vector3(bounds.min.x + 1.0f, bounds.min.y, 1.0f);
        _player.transform.position = playerPos;

        Vector3 firstMonPos = new Vector3(bounds.max.x - 2.0f, bounds.max.y - 2.0f, 1.0f);
        foreach (var mon in _mons)
        {
            Vector3 randomSeed = (Vector3)Random.insideUnitCircle * 2.0f;
            mon.transform.position = firstMonPos + randomSeed;
        }
    }
}
