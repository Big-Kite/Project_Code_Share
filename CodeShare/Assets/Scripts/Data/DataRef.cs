using UnityEngine;

public class DataRef : Singleton<DataRef>
{
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
