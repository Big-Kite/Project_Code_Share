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
    public void ActivateSkill(BattleUnit _unit, int _skillNo) // NPC 주체
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
    /// 스킬을 어떻게 할 것인가
    /// 스킬 슬롯 버튼 패널에 이 레프를 붙히고
    /// 버튼 인덱스별로 하나씩 버튼 연결
    /// 각 슬롯별 쿨타임 개별 진행
    /// 버튼을 누르면 플레이어의 다음 공격에 실행되게
    /// 예약 시스템이라고 해야하나?
    /// ㅇ
    /// 스킬 타겟팅은 현재 공격중인 몬스터 좌표
    /// 부실하면 오토타겟팅 설정을 조금 넣어주기
}
