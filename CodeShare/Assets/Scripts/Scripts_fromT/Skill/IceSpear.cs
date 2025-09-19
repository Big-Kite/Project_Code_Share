using System.Collections;
using UnityEngine;

public class IceSpear : Skill
{
    Vector3 testDir = new Vector3(6.0f, -10.0f, 0.0f);
    GameObject[] spears = new GameObject[20];

    int realAttack = 10;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;
        master.SkillCast(1);

        realAttack = 10;

        for (int i = 0; i < spears.Length; i++)
        {
            Vector3 randomSeed = (Vector3)Random.insideUnitCircle * 3.5f;

            var temp = pool.GetObject(0);
            temp.transform.position = randomSeed - testDir;
            spears[i] = temp;
        }
        StartCoroutine(CoDropIceSpear());
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    IEnumerator CoDropIceSpear()
    {
        for (int i = 0; i < spears.Length; i++)
        {
            StartCoroutine(CoSingleDropIceSpear(spears[i]));
        }
        yield return YieldCache.WaitForSeconds(2.5f);
        yield return null;

        master.CompleteSkillCast();
    }
    IEnumerator CoSingleDropIceSpear(GameObject _spear)
    {
        yield return YieldCache.WaitForSeconds(Random.Range(0.2f, 1.0f));

        float duration = 1.3f;
        while (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            _spear.transform.position += testDir.normalized * 8.0f * Time.deltaTime;
            yield return null;
        }

        var temp2 = pool.GetObject(1);
        temp2.transform.position = _spear.transform.position;

        _spear.SetActive(false);

        if (realAttack > 0)
        {
            realAttack--;
            yield return YieldCache.WaitForSeconds(Random.Range(0.0f, 0.7f));

            var mons = BattleManager.Instance.Npcs;
            foreach (var mon in mons)
            {
                if (mon == master) continue;
                if (mon.IsDead()) continue;

                mon.ShockFlash();
                mon.TakeDamage(7);
            }
        }
    }
}
