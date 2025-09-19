using System.Collections;
using UnityEngine;

public class ArrowShower : Skill
{
    Vector3 testDir = new Vector3(4.0f, 10.0f, 0.0f);
    GameObject[] arrows = new GameObject[15];

    int realAttack = 8;
    float range = 3.0f;
    float delay = 1.0f;

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
        indicator.Init(this, master.Target.transform.position, Vector3.zero, range, delay);
        indicator.BeginCharge();

        realAttack = 8;
        master.SkillCast(0);
    }
    public override int GetNeedMp()
    {
        return 5;
    }
    public override void CompleteIndicator()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            var temp = pool.GetObject(0);
            temp.transform.position = testDir;
            arrows[i] = temp;
        }
        StartCoroutine(CoDropArrow());
    }
    IEnumerator CoDropArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            StartCoroutine(CoSingleDropArrow(arrows[i]));
        }
        yield return YieldCache.WaitForSeconds(2.5f);
        yield return null;

        master.CompleteSkillCast();
        indicator.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    IEnumerator CoSingleDropArrow(GameObject _arrow)
    {
        Vector3 startPos = _arrow.transform.position;
        Vector3 endPos = indicator.transform.position + (Vector3)Random.insideUnitCircle * 1.0f;

        Vector3 direction = endPos - startPos;
        _arrow.transform.rotation = Quaternion.FromToRotation(Vector2.down, direction.normalized);

        yield return YieldCache.WaitForSeconds(Random.Range(0.2f, 1.0f));

        float routineTime = 0.0f, duration = 1.0f;
        while (routineTime < duration)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / duration);

            _arrow.transform.position = Vector3.Lerp(startPos, endPos, clampTime);

            yield return null;
        }
        var temp2 = pool.GetObject(1);
        temp2.transform.position = _arrow.transform.position;

        _arrow.SetActive(false);

        if (realAttack > 0)
        {
            realAttack--;
            var mons = indicator.GetTargets();
            foreach (var mon in mons)
            {
                if (mon == master.Target && mon.IsDead() == false)
                {
                    mon.HitFlash();
                    mon.TakeDamage(2);
                }
            }
        }
    }
}
