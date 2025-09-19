using System.Collections;
using UnityEngine;

public class ShieldAttack : Skill
{
    float range = 1.5f;
    float delay = 0.1f;

    Vector3 startPos = Vector3.zero;
    Vector3 endPos = Vector3.zero;
    Vector3 dir = Vector3.zero;

    Indicator indicator;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;
        startPos = master.transform.position;
        dir = master.GetDirection();
        endPos = startPos + (dir * range);

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
        StartCoroutine(CoAttack());
    }
    IEnumerator CoAttack()
    {
        master.SkillCast(2);

        bool isHit = false;
        var mons = indicator.GetTargets();
        foreach (var mon in mons)
        {
            if (mon == master) continue;
            if (mon.IsDead()) continue;

            mon.TakeDamage(3);
            mon.TakeDebuff(DebuffType.UnableAct, 4.0f, Color.gray);

            var stunPos = new Vector3(mon.GetTotalBounds().center.x, mon.GetTotalBounds().max.y, 0.0f);
            EffectController.Instance.ShowAttachBattleEffect(BattleEffect.Stun, mon, stunPos, 4.0f);

            var dir = mon.transform.position - startPos;
            mon.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 30.0f, ForceMode2D.Impulse);

            var temp = pool.GetObject(1);
            temp.transform.position = mon.transform.position;
            temp.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

            isHit = true;
        }
        if (isHit)
        {
            var temp = pool.GetObject(0);
            temp.transform.position = master.transform.position;
        }

        float routineTime = 0.0f;
        while (routineTime < 0.15f)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / 0.15f);

            master.transform.position = Vector3.Lerp(startPos, endPos, clampTime);
            yield return null;
        }
        indicator.gameObject.SetActive(false);
        master.CompleteSkillCast();
        yield return null;
    }
}
