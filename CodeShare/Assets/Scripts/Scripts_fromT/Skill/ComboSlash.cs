using System.Collections;
using UnityEngine;

public class ComboSlash : Skill
{
    float range = 3.0f;
    float delay = 0.3f;

    Vector3 dir = Vector3.zero;

    Indicator indicator;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;
        dir = master.GetDirection();

        master.SkillCast(1); // 새로운 모션 필요
        indicator = SkillManager.Instance.GetIndicator(1);
        indicator.Init(this, master.transform.position, dir, range, delay);
        indicator.BeginCharge();
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    public override void CompleteIndicator()
    {
        master.CompleteSkillCast();
        StartCoroutine(CoCombo());
    }
    IEnumerator CoCombo()
    {
        var temp = pool.GetObject(0);
        temp.transform.position = master.transform.position;
        Filper.Filp(temp.transform, dir.x < 0.0f);
        yield return StartCoroutine(CoAttack());

        temp = pool.GetObject(1);
        temp.transform.position = master.transform.position;
        Filper.Filp(temp.transform, dir.x < 0.0f);
        yield return StartCoroutine(CoAttack());

        temp = pool.GetObject(2);
        temp.transform.position = master.transform.position;
        Filper.Filp(temp.transform, dir.x < 0.0f);
        yield return StartCoroutine(CoAttack());

        indicator.gameObject.SetActive(false);
    }
    IEnumerator CoAttack()
    {
        master.SkillCast(2); // 새로운 모션 필요
        yield return YieldCache.WaitForSeconds(0.2f);

        var mons = indicator.GetTargets();
        foreach (var mon in mons)
        {
            if (mon == master) continue;
            if (mon.IsDead()) continue;

            mon.TakeDamage(12);
        }
        yield return YieldCache.WaitForSeconds(0.1f);

        master.CompleteSkillCast();
        yield return null;
    }
}
