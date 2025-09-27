using UnityEngine;
using UnityEngine.UI;

public class HpSystem : MonoBehaviour
{
	[Header("UI Elements")] // 인스펙터 세팅
	[SerializeField] GameObject hpObject;
    [SerializeField] GameObject mpObject;
    [SerializeField] Image hpValue;
	[SerializeField] Image delayHpValue;
    [SerializeField] Image mpValue;

    int hp = 0;
	int maxHp = 0;
	int mp = 0;
	int maxMp = 0;
    public int CurHp { get { return hp; } }
    public int MaxHp { get { return maxHp; } }
    public int CurMp { get { return mp; } }
    public int MaxMp { get { return maxMp; } }

    float pastHp = 0.0f;
    float delayTimeLeft = 0.0f;
    float delayInterval = 0.2f;

    bool regen = true; // 일단 안씀
	int regenValue = 1;
	float regenTimeleft = 0.0f;
	float regenInterval = 500.0f;

	void Update ()
	{
        ManaRegen();
        DelayTakeDamage();
	}
    void UpdateGraphics()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }
    public void Init(int _hp, int _mp, float _regenInterval = 500.0f)
    {
        maxHp = _hp;
        hp = maxHp;

        maxMp = _mp;
        mp = 0;

        regenInterval = _regenInterval;

        if (maxMp <= 0)
        {
            mpObject.SetActive(false);
            regen = false;
        }
        UpdateGraphics();
    }
    void ManaRegen()
	{
        if (!regen)
            return;

        regenTimeleft -= Time.deltaTime;
		if (regenTimeleft <= 0.0) // Interval ended - update health & mana and start new interval
		{
            RestoreMana(regenValue);	
			regenTimeleft = regenInterval;
		}
	}
    void UpdateHealthBar()
	{
        if (maxHp <= 0)
            return;

        float ratio = (float)hp / maxHp;
		hpValue.rectTransform.localPosition = new Vector3(hpValue.rectTransform.rect.width * ratio - hpValue.rectTransform.rect.width, 0, 0);
	}
	void DelayTakeDamage()
	{
		if (pastHp <= hp)
			return;

        delayTimeLeft -= Time.deltaTime;
        if (delayTimeLeft <= 0.0)
        {
            pastHp = pastHp * 0.9f;

            float ratio = pastHp / maxHp;
            delayHpValue.rectTransform.localPosition = new Vector3(delayHpValue.rectTransform.rect.width * ratio - delayHpValue.rectTransform.rect.width, 0, 0);
            delayTimeLeft = delayInterval;
        }
    }
    public void TakeDamage(int _damage)
	{
        if (pastHp < hp)
			pastHp = hp;

        hp -= _damage;
		if (hp < 1)
		{
            hp = 0;
            hpObject.SetActive(false);
            mpObject.SetActive(false);
			return;
        }
        UpdateGraphics();
	}
	public void TakeHeal(int _heal)
	{
		hp += _heal;
		if (hp > maxHp) 
			hp = maxHp;

		UpdateGraphics();
	}
	void UpdateManaBar()
	{
		if (maxMp <= 0)
			return;

		float ratio = (float)mp / maxMp;
		mpValue.rectTransform.localPosition = new Vector3(mpValue.rectTransform.rect.width * ratio - mpValue.rectTransform.rect.width, 0, 0);
    }
    public bool UseMana(int _Mana)
	{
		if ((mp - _Mana) < 0) // 보유한 마나에서 요구하는 마나를 사용했을때 0보다 작으면 쓸 수 없다.
			return false;

		mp -= _Mana;
		if (mp <= 0)
			mp = 0;

		UpdateGraphics();
		return true;
	}
	public void RestoreMana(int _Mana)
	{
		mp += _Mana;
		if (mp > maxMp) 
			mp = maxMp;

		UpdateGraphics();
	}
	public bool IsDead()
	{
		return hp <= 0;
	}
}
