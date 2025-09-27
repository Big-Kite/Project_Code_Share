using System.Collections;
using UnityEngine;

public class IceStrike : Skill
{
    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;
        master.SkillCast(1);

        StartCoroutine(CoShoot());
    }
    IEnumerator CoShoot()
    {
        Transform ice = pool.GetObject(0).transform;
        ice.position = master.Front.position;

        Vector3 direction = master.Target.Front.transform.position - master.Front.position;
        ice.rotation = Quaternion.FromToRotation(Vector2.up, direction.normalized);

        yield return null;

        while (true)
        {
            var dir = master.Target.Front.transform.position - ice.position;
            if (dir.magnitude < 0.1f)
                break;

            ice.position += dir.normalized * 7.0f * Time.deltaTime;
            yield return null;
        }
        ice.gameObject.SetActive(false);
        CauseDamage();
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    void CauseDamage()
    {
        var temp = pool.GetObject(1);
        temp.transform.position = master.Target.Front.transform.position;

        master.Target.TakeDamage(11);
        master.Target.TakeDebuff(DebuffType.UnableAct, 4.0f, Color.blue);

        EffectController.Instance.ShowAttachBattleEffect(BattleEffect.IceColumn, master.Target, master.Target.transform.position, 4.0f);

        master.CompleteSkillCast();
        StartCoroutine(CoEndWork());
    }
    IEnumerator CoEndWork()
    {
        yield return YieldCache.WaitForSeconds(4.0f);
        gameObject.SetActive(false);
    }
}
