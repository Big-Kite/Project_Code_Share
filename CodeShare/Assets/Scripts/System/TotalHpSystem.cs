using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TotalHpSystem : Singleton<TotalHpSystem>
{
    [Header("UI Elements")] // 인스펙터 세팅
    [SerializeField] Slider hpValue;
    [SerializeField] TextMeshProUGUI hpText;

    int hp = 0;
    int totalHp = 0;
    float interval = 0.5f;
    bool isInit = false;
    List<BattleUnit> monsters;

    void LateUpdate()
    {
        if (!isInit)
            return;

        interval -= Time.deltaTime;
        if (interval < 0)
        {
            UpdateTotalHp();
            interval = 0.5f;
        }
    }
    public void Init(List<BattleUnit> _monsters)
    {
        monsters = _monsters;
        foreach (var mon in monsters)
        {
            if (mon == null) continue;

            totalHp += mon.HpComponent.MaxHp;
        }
        hp = totalHp;
        UpdateGraphic();

        isInit = true;
    }
    void UpdateGraphic()
    {
        float ratio = ((float)hp / totalHp) * 100;
        hpValue.value = ratio;
        hpText.text = $"{(int)ratio} %";
    }
    void UpdateTotalHp()
    {
        hp = 0;
        foreach (var mon in monsters)
        {
            if (mon == null) continue;

            hp += mon.HpComponent.CurHp;
        }
        UpdateGraphic();
    }
}
