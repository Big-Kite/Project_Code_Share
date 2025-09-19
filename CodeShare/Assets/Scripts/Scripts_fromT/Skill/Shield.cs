using UnityEngine;

public class Shield : Skill
{
    float timer = 10.0f;
    GameObject shieldObject = null;

    HpSystem hPSystem = null;

    public override void OperationSkill(BattleUnit _unit)
    {
        master = _unit;

        timer = 10.0f;
        shieldObject = pool.GetObject(0);
        shieldObject.transform.position = master.transform.position;

        hPSystem = shieldObject.GetComponentInChildren<HpSystem>();

        master.AttachShield(this);
        hPSystem.Init(100, 0);
    }
    public override int GetNeedMp()
    {
        return 1;
    }
    public bool TakeDamage(ref int _damage)
    {
        bool perfectShield = true;
        
        int temp = _damage;
        
        _damage -= hPSystem.CurHp;
        if (_damage > 0)
            perfectShield =  false; // 다 못막은거

        EffectController.Instance.ShowFloatingUI(FloatingType.Protect, shieldObject.transform.position, temp);
        hPSystem.TakeDamage(temp);

        if (hPSystem.IsDead())
            timer = -1.0f;

        return perfectShield; // 다 막은거
    }
    void LateUpdate()
    {
        if (shieldObject == null)
            return;

        timer -= Time.deltaTime;
        shieldObject.transform.position = master.transform.position;

        if (timer < 0)
        {
            master.DetachShield();
            gameObject.SetActive(false);
        }
    }
}
