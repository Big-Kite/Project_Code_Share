using UnityEngine;

public class DataRef : Singleton<DataRef>
{
    // 게임의 모든 데이터를 들고 있는 Ref 싱글톤입니다.
    // 각 컨텐츠 내용에 맞는 DB들을 겟 프로퍼티로 외부 어디에서든 참조할 수 있게 구현했습니다.
    [SerializeField] NpcDB npcDB;
    [SerializeField] StageDB stageDB;
    [SerializeField] ItemDB itemDB;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public static NpcDB GetNpcDB
    {
        get { return Instance.npcDB; }
    }
    public static StageDB GetStageDB
    {
        get { return Instance.stageDB; }
    }
    public static ItemDB GetItemDB
    {
        get { return Instance.itemDB; }
    }
}
