using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image frame; // ��Ȱ�� ���� 0.8 Ȱ�� ���� 1.0
    [SerializeField] Image icon; // ��Ȱ�� ��Ƽ��false Ȱ�� ��Ƽ�� true
    [SerializeField] Image cool; // ��Ÿ�� 1.0���� ��� 0.0���� ����

    int skillNo = 0;
    bool usable = false;
    float maxCool = 0.0f;
    float curCool = 0.0f;

    public void SetData(int _index)
    {
        // ���⼭ ��ų ������ ����
        // ��Ÿ��
        // � ��ų���� (���ų������ �������ϼ��� ���� �׷��� �ʿ����)
        // �ӽ÷�
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
        icon.gameObject.SetActive(usable); // ��ų��ȣ�� �����Ϳ� �´� ��������Ʈ �־������.    
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
