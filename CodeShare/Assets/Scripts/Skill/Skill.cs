using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Pool pool = null;
    protected BattleUnit master = null;

    protected virtual void Awake()
    {
        pool = GetComponent<Pool>();
    }
    protected virtual void OnDisable()
    {
        master = null;
    }
    public abstract void OperationSkill(BattleUnit _unit);
    public abstract int GetNeedMp();
    public virtual void CompleteIndicator() { }
}
