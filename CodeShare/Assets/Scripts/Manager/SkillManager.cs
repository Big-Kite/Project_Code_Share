using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] SkillSlot[] skillSlots;
    [SerializeField] Pool skillPool;
    [SerializeField] Pool indicatorPool;

    void Start()
    {
        int equipedIndex = 0;
        foreach (SkillSlot slot in skillSlots)
        {
            slot.SetData(PlayerData.Instance.EquipSkill[equipedIndex++]);
            slot.SetUsable(true);
        }
    }
    public void OnClickSkill(SkillSlot _slot, int _skillNo)
    {
        if (BattleManager.Instance.Player.CheckNeedMP(_skillNo))
        {
            var skill = skillPool.GetComponentDirect<Skill>(_skillNo - 1);
            skill.OperationSkill(BattleManager.Instance.Player);

            _slot.CoolOperation();
        }
    }
    public Indicator GetIndicator(int _type)
    {
        var indicator = indicatorPool.GetComponentDirect<Indicator>(_type);
        return indicator;
    }
    public void ActivateSkill(BattleUnit _unit, int _skillNo) // NPC ��ü
    {
        var skill = skillPool.GetComponentDirect<Skill>(_skillNo - 1);
        skill.OperationSkill(_unit);
    }
    public int GetNeedMp(int _skillNo)
    {
        var skill = skillPool.GetComponentDirect<Skill>(_skillNo - 1);
        int needMp = skill.GetNeedMp();
        skill.gameObject.SetActive(false);
        return needMp;
    }
    /// ��ų�� ��� �� ���ΰ�
    /// ��ų ���� ��ư �гο� �� ������ ������
    /// ��ư �ε������� �ϳ��� ��ư ����
    /// �� ���Ժ� ��Ÿ�� ���� ����
    /// ��ư�� ������ �÷��̾��� ���� ���ݿ� ����ǰ�
    /// ���� �ý����̶�� �ؾ��ϳ�?
    /// ��
    /// ��ų Ÿ������ ���� �������� ���� ��ǥ
    /// �ν��ϸ� ����Ÿ���� ������ ���� �־��ֱ�
}
