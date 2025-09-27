using System.Collections.Generic;
using UnityEngine;

public abstract class Indicator : MonoBehaviour
{
    [SerializeField] protected Transform charging;   // 커지는 원

    protected float chargeTime = 0.0f;    // 커지는 시간
    protected float currentTime = 0.0f;
    protected bool isCharging = false;

    protected HashSet<BattleUnit> targets = new HashSet<BattleUnit>();
    protected Skill skill;

    public abstract void Init(Skill _skill, Vector3 _pos, Vector3 _dir, float _range, float _delay);
    protected abstract void Update();

    void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<BattleUnit>();

        if (unit != null)
            targets.Add(collision.GetComponent<BattleUnit>());
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        var unit = collision.GetComponent<BattleUnit>();

        if (unit != null)
            targets.Remove(collision.GetComponent<BattleUnit>());
    }
    void OnDisable()
    {
        targets.Clear();
    }
    public void BeginCharge()
    {
        currentTime = 0.0f;
        isCharging = true;
        charging.gameObject.SetActive(true);
    }
    public void CancelCharge()
    {
        isCharging = false;
        charging.localScale = Vector3.zero;
        charging.gameObject.SetActive(false);
    }
    public HashSet<BattleUnit> GetTargets()
    {
        return targets;
    }
}
