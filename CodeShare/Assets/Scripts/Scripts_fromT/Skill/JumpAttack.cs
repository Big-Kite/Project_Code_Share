using UnityEngine;

public class JumpAttack : Skill
{
    float range = 2.0f;
    float delay = 0.5f;

    Indicator indicator;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;

        var anim = master.GetComponentInChildren<Animator>();
        if (anim == null)
            return;

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "SKILL")
            {
                delay = clip.length;
                break;
            }
        }

        indicator = SkillManager.Instance.GetIndicator(0);
        indicator.Init(this, master.transform.position, Vector3.zero, range, delay);
        indicator.BeginCharge();

        master.SkillCast(0);
    }
    public override int GetNeedMp()
    {
        return 7;
    }
    public override void CompleteIndicator()
    {
        var temp = pool.GetObject(0);
        temp.transform.position = indicator.transform.position;

        var mons = indicator.GetTargets();
        foreach (var mon in mons)
        {
            if(mon == master.Target && mon.IsDead() == false)
            {
                mon.TakeDamage(30);
                var dir = mon.transform.position - indicator.transform.position;
                mon.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 50.0f, ForceMode2D.Impulse);
                break;
            }
        }
        master.CompleteSkillCast();
        indicator.gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }
}
