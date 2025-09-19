using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    bool stageRunning = false;
    bool stageLoad = false;

    bool holdOnTarget = false;

    StageDataSO curStageDataSO = null;

    List<StageMonster> stageMonsters = new List<StageMonster>();
    StageMonster curTarget = null;

    Vector3 playerPos = Vector3.zero;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void SetCurStage(int _index)
    {
        if (_index < 1 || _index > DataRef.GetStageDB.dataSOs.Length)
        {
            Debug.LogError("Stage Select index error!");
            return;
        }
        curStageDataSO = DataRef.GetStageDB.dataSOs[_index - 1];
    }
    public StageDataSO GetCurStageDataSO()
    {
        if (curStageDataSO == null)
        {
            Debug.LogError("Stage data null!");
            return null;
        }
        return curStageDataSO;
    }
    public List<StageMonster> GetLoadedStageMonster()
    {
        return stageMonsters;
    }
    public void AddSpawnedNpcGroupData(int _no, NpcGroupData _npcGroupData)
    {
        if (_npcGroupData == null)
            return;

        var temp = new StageMonster(_no, _npcGroupData);
        stageMonsters.Add(temp);
    }
    public void SetTarget(int _no, int _key, Vector3 _pos)
    {
        if (holdOnTarget)
            return;

        foreach (var npc in stageMonsters)
        {
            if(npc.NoByStage == _no && npc.npcGroupData.Key == _key && npc.hunted == false)
            {
                curTarget = npc;
                curTarget.fieldPos = _pos;
                break;
            }
        }
        InteractionController.Instance.Show();
    }
    public void SetTargetNull()
    {
        if (holdOnTarget)
            return;

        curTarget = null;
        InteractionController.Instance.Hide();
    }
    public int GetCurNpcGroupDataKey()
    {
        return curTarget.npcGroupData.Key;
    }
    public void HoldOnTarget()
    {
        holdOnTarget = true;
    }
    public void EnterStage()
    {
        stageRunning = true;
        stageLoad = false;
        holdOnTarget = false;

        stageMonsters.Clear();
        curTarget = null;
    }
    public bool Loaded()
    {
        return stageLoad;
    }
    public void CompleteLoad()
    {
        stageLoad = true;
    }
    public void BattleWin()
    {
        curTarget.hunted = true;
        holdOnTarget = false;
    }
    public void SavePlayerPos()
    {
        playerPos = PlayerInField.Instance.transform.position;
    }
    public void LoadPlayerPos(Transform _player)
    {
        _player.position = playerPos;
    }
}

public class StageMonster
{
    public int NoByStage = 0;
    public bool hunted = false;
    public Vector3 fieldPos = Vector3.zero;
    public NpcGroupData npcGroupData = null;

    public StageMonster(int _no, NpcGroupData _npcGroupData)
    {
        NoByStage = _no;
        hunted = false;
        fieldPos = Vector3.zero;
        npcGroupData = _npcGroupData;
    }
}