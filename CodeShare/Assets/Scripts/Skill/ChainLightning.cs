using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightning : Skill
{
    Vector3 start;

    int order = -1;
    int count = 7;
    List<BattleUnit> units = new List<BattleUnit>();

    public override void OperationSkill(BattleUnit _unit)
    {
        //List<int> indexList = Enumerable.Range(0, 5).ToList(); // 블럭 랜덤 지정을 위한 리스트
        master = _unit;

        System.Random random = new System.Random();
        units = BattleManager.Instance.Npcs.OrderBy(a => random.Next()).ToList();

        order = -1;
        count = 7;

        start = master.Front.position;
        StartCoroutine(CoElectrocute());
    }
    public override int GetNeedMp()
    {
        return 2;
    }
    IEnumerator CoElectrocute()
    {
        while (count > 0)
        {
            yield return StartCoroutine(CoSingleElectrocute());
            yield return null;
        }
    }
    IEnumerator CoSingleElectrocute()
    {
        order = (order + 1) % units.Count;

        var target = units[order];
        if(target == null || target.IsDead())
        {
            yield break;
        }

        var temp = pool.GetObject(0);
        temp.transform.position = start;

        while (true)
        {
            var dir = target.transform.position - temp.transform.position;
            if (dir.magnitude < 0.3f)
                break;

            temp.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir.normalized);
            temp.transform.position += dir.normalized * 13.0f * Time.deltaTime;
            yield return null;
        }

        var temp2 = pool.GetObject(1);
        temp2.transform.position = target.transform.position;

        target.ShockFlash();
        target.TakeDamage(Random.Range(3, 6));

        temp.SetActive(false);
        start = target.transform.position;

        count--;
        yield return null;
    }
}
