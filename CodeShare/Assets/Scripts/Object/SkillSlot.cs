using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image frame; // 비활성 알파 0.8 활성 알파 1.0
    [SerializeField] Image icon; // 비활성 액티브false 활성 액티브 true
    [SerializeField] Image cool; // 쿨타임 1.0에서 출발 0.0으로 도착

    int skillNo = 0;
    bool usable = false;
    float maxCool = 0.0f;
    float curCool = 0.0f;

    public void SetData(int _index)
    {
        // 여기서 스킬 데이터 적용
        // 쿨타임
        // 어떤 스킬인지 (어떤스킬인지는 고정값일수도 있음 그러면 필요없음)
        // 임시로
        skillNo = _index;

        maxCool = 5.0f;
        curCool = 0.0f;
    }
    public void SetUsable(bool _usable)
    {
        usable = _usable;

        button.enabled = usable;
        button.onClick.RemoveAllListeners();

        frame.color = usable ? Color.white : new Color(1.0f, 1.0f, 1.0f, 0.8f);
        icon.gameObject.SetActive(usable); // 스킬번호로 데이터에 맞는 스프라이트 넣어줘야함.    
        cool.fillAmount = 0.0f;

        if (usable)
            button.onClick.AddListener(() => SkillManager.Instance.OnClickSkill(this, skillNo));
    }
    public void CoolOperation()
    {
        button.enabled = false;

        curCool = maxCool;
        cool.fillAmount = 1.0f;

        StartCoroutine(CoCool());
    }
    IEnumerator CoCool()
    {
        while (curCool > 0.0f)
        {
            curCool -= Time.deltaTime;
            float clamp = Mathf.Clamp01(curCool / maxCool);

            cool.fillAmount = clamp;
            yield return null;
        }

        curCool = 0.0f;
        button.enabled = true;
    }
}
