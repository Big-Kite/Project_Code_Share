using UnityEngine;

public class WhirlWind : Skill
{
    float timer = 3.0f;
    float range = 3.0f;
    float delay = 0.5f;

    Indicator indicator;

    GameObject temp;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;
        timer = 10.0f;

        temp = pool.GetObject(0);
        temp.transform.position = master.transform.position;

        indicator = SkillManager.Instance.GetIndicator(0);
        indicator.Init(this, master.transform.position, Vector3.zero, range, delay);
        indicator.BeginCharge();
    }
    public override int GetNeedMp()
    {
        return 4;
    }
    public override void CompleteIndicator()
    {
        var mons = indicator.GetTargets();
        foreach (var mon in mons)
        {
            if (mon == master) continue;
            if (mon.IsDead()) continue;

            mon.TakeDamage(14);
            
            var dir = mon.transform.position - indicator.transform.position;
            mon.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 100.0f);
        }
        temp.transform.position = master.transform.position;
        indicator.transform.position = master.transform.position;
        indicator.BeginCharge();
    }
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            indicator.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
