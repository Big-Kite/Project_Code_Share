using System.Collections;

public class Heal : Skill
{
    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;

        var temp = pool.GetObject(0);
        temp.transform.position = master.transform.position;

        master.TakeHeal(100);
        StartCoroutine(CoEndWork());
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    IEnumerator CoEndWork()
    {
        yield return YieldCache.WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
