using System.Collections;
using UnityEngine;

public class SwordStrike : Skill
{
    Vector3 testDir = new Vector3(0.0f, 13.0f, 0.0f);
    GameObject[] swords = new GameObject[9];

    int realAttack = 6;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;
        master.SkillCast(1);

        realAttack = 6;

        for (int i = 0; i < 9; i++)
        {
            Vector3 randomSeed = (Vector3)Random.insideUnitCircle * 5.0f;

            var temp = pool.GetObject(1);
            temp.transform.position = randomSeed + testDir;

            swords[i] = temp;
        }
        StartCoroutine(CoDropSwords());
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    IEnumerator CoDropSwords()
    {
        for (int i = 0; i < 9; i++)
        {
            StartCoroutine(CoSingleDropSword(swords[i]));
        }

        var circle = pool.GetObject(0);
        float routineTime = 0.0f, duration = 2.5f;
        while (routineTime < duration)
        {
            routineTime += Time.deltaTime;
            circle.transform.Rotate(0.0f, 0.0f, 30.0f * Time.deltaTime);
            yield return null;
        }

        master.CompleteSkillCast();

        var mat = circle.GetComponent<SpriteRenderer>().material;
        routineTime = 0.0f;
        duration = 1.0f;
        while (routineTime < duration)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / duration);
            mat.SetFloat("_FadeAmount", clampTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    IEnumerator CoSingleDropSword(GameObject _sword)
    {
        Vector3 startPos = _sword.transform.position;
        Vector3 endPos = Vector3.up + (Vector3)Random.insideUnitCircle * 2.0f;

        var dir = endPos - startPos;
        dir.Normalize();

        _sword.transform.rotation = Quaternion.FromToRotation(Vector2.down, dir);
        yield return YieldCache.WaitForSeconds(Random.Range(0.2f, 1.0f));

        float routineTime = 0.0f, duration = 0.3f;
        while (routineTime < duration)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / duration);

            _sword.transform.position = Vector3.Lerp(startPos, endPos, clampTime * clampTime);
            yield return null;
        }
        var mat = _sword.GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_ClipUvDown", Random.Range(0.15f, 0.4f));

        var temp = pool.GetObject(2);
        temp.transform.position = _sword.transform.position;

        yield return YieldCache.WaitForSeconds(1.0f);

        if (realAttack > 0)
        {
            realAttack--;
            yield return YieldCache.WaitForSeconds(Random.Range(0.0f, 1.0f));

            var mons = BattleManager.Instance.Npcs;
            foreach (var mon in mons)
            {
                if (mon == master) continue;
                if (mon.IsDead()) continue;

                mon.HitFlash();
                mon.TakeDamage(10);
            }
        }

        routineTime = 0.0f;
        duration = 2.0f;

        while (routineTime < duration)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / duration);
            mat.SetFloat("_FadeAmount", clampTime);
            yield return null;
        }
    }
}
