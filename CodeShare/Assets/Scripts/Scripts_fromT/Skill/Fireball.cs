using UnityEngine;
using System.Collections;

public class Fireball : Skill
{
    Transform ball;

    float range = 2.5f;
    float delay = 0.7f;

    Indicator indicator;
    GameObject magicCircle;
    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;

        master.SkillCast(1);

        magicCircle = pool.GetObject(0);
        magicCircle.transform.position = master.Front.position;

        indicator = SkillManager.Instance.GetIndicator(0);
        indicator.Init(this, master.Target.transform.position, Vector3.zero, range, delay);
        indicator.BeginCharge();
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    public override void CompleteIndicator()
    {
        ball = pool.GetObject(1, true).transform;
        ball.position = master.Front.position;

        Vector3 direction = indicator.transform.position - master.Front.position;
        ball.rotation = Quaternion.FromToRotation(Vector2.up, direction.normalized);

        StartCoroutine(CoShoot());
    }
    IEnumerator CoShoot()
    {
        yield return YieldCache.WaitForSeconds(0.3f);
        magicCircle.SetActive(false);
        master.CompleteSkillCast();

        ball.gameObject.SetActive(true);

        while (true)
        {
            var dir = indicator.transform.position - ball.position;
            if (dir.magnitude < 0.1f)
                break;

            ball.position += dir.normalized * 8.0f * Time.deltaTime;
            yield return null;
        }
        ball.gameObject.SetActive(false);
        Burn();
    }
    void Burn()
    {
        var temp = pool.GetObject(2);
        temp.transform.position = indicator.transform.position;

        var mons = indicator.GetTargets();
        foreach (var mon in mons)
        {
            if(mon == master) continue;
            if (mon.IsDead()) continue;

            var burn = pool.GetObject(3);
            burn.transform.position = mon.transform.position;
        
            mon.TakeDamage(30);
        }
        StartCoroutine(CoEndWork());
    }
    IEnumerator CoEndWork()
    {
        indicator.gameObject.SetActive(false);
        yield return YieldCache.WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
}
