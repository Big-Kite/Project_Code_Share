using UnityEngine;

public class LogManager : Singleton<LogManager>
{
    [SerializeField] Transform prefabParent;
    [SerializeField] GameObject logPrefab;
    //int logCount = 0;

    public void PrintLog(string _log)
    {
        //logCount++;
        //
        //BattleLog newLog = Instantiate(logPrefab, prefabParent).GetComponent<BattleLog>();
        //newLog.SetText(_log);
        //
        //if ((logCount % 2) == 0)
        //    newLog.SetColor();
    }
}
