using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // 사용하는 스킬에서 연출해야할 오브젝트들을 풀에 담아둡니다.
    // 스킬의 주체를 등록시켜 플레이어 뿐 아니라 NPC도 쓸 수 있도록 범용적으로 만들었습니다.
    // 오퍼레이션 함수에서 스킬 로직을 짜고, 필요에 따라 인디케이터 딜레이를 기다려야 하는 스킬은 인디케이터에서 완료 호출을 받아 로직을 실행합니다.
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
