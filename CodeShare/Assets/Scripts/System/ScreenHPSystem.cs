using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHPSystem : Singleton<ScreenHPSystem>
{
    [Header("UI Elements")] // 인스펙터 세팅
    [SerializeField] GameObject hpObject;
    [SerializeField] GameObject mpObject;
    [SerializeField] Image hpValue;
    [SerializeField] Image delayHpValue;
    [SerializeField] Image mpValue;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI mpText;

    BattleUnit player = null;

    int maxHp = 0;
    int hp = 0;

    int maxMp = 0;

    float pastHp = 0.0f;
    float delayTimeLeft = 0.0f;
    float delayInterval = 0.5f;

    float updateInterval = 0.2f;
    bool isInit = false;

    void Update()
    {
        if (!isInit)
            return;

        DelayTakeDamage();

        updateInterval -= Time.deltaTime;
        if (updateInterval < 0.0f)
        {
            UpdateGraphics();
            updateInterval = 0.2f;
        }
    }
    void UpdateGraphics()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }
    public void Init(BattleUnit _player)
    {
        player = _player;

        maxHp = player.HpComponent.MaxHp;
        hpText.text = $"{maxHp} / {maxHp}";
        pastHp = player.HpComponent.CurHp;
        hp = player.HpComponent.CurHp;

        maxMp = player.HpComponent.MaxMp;
        mpText.text = $"0 / {maxMp}";

        UpdateGraphics();
        isInit = true;
    }
    void UpdateHealthBar()
    {
        if (maxHp <= 0)
            return;

        var curHp = player.HpComponent.CurHp;
        if(hp < curHp)
            pastHp = curHp;

        hp = curHp;

        float ratio = (float)hp / maxHp;
        hpValue.rectTransform.localPosition = new Vector3(hpValue.rectTransform.rect.width * ratio - hpValue.rectTransform.rect.width, 0, 0);

        hpText.text = $"{hp} / {maxHp}";
    }
    void UpdateManaBar()
    {
        if (maxMp <= 0)
            return;

        float ratio = (float)player.HpComponent.CurMp / maxMp;
        mpValue.rectTransform.localPosition = new Vector3(mpValue.rectTransform.rect.width * ratio - mpValue.rectTransform.rect.width, 0, 0);

        mpText.text = $"{player.HpComponent.CurMp} / {maxMp}";
    }
    void DelayTakeDamage()
    {
        if (pastHp <= player.HpComponent.CurHp)
            return;

        delayTimeLeft -= Time.deltaTime;
        if (delayTimeLeft <= 0.0)
        {
            pastHp = pastHp * 0.98f;

            float ratio = pastHp / maxHp;
            delayHpValue.rectTransform.localPosition = new Vector3(delayHpValue.rectTransform.rect.width * ratio - delayHpValue.rectTransform.rect.width, 0, 0);
            delayTimeLeft = delayInterval;
        }
    }
}
