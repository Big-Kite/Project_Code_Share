using UnityEngine;

[System.Serializable]
public class StageData
{
    [SerializeField] int key = 0;
    [SerializeField] string fieldName = string.Empty;
    [SerializeField] string battleName = string.Empty;
    [SerializeField] int spawnNormal = 0;
    [SerializeField] int spawnHard = 0;
    [SerializeField] int boss = 0;
    [SerializeField] int groupMin = 0;
    [SerializeField] int groupMax = 0;

    public int Key => key;
    public string FieldName => fieldName;
    public string BattleName => battleName;
    public int SpawnNormal => spawnNormal;
    public int SpawnHard => spawnHard;
    public int Boss => boss;
    public int GroupMin => groupMin;
    public int GroupMax => groupMax;

    public void SetData(params object[] _data)
    {
        key = (int)_data[0];
        fieldName = (string)_data[1];
        battleName = (string)_data[2];
        spawnNormal = (int)_data[3];
        spawnHard = (int)_data[4];
        boss = (int)_data[5];
        groupMin = (int)_data[6];
        groupMax = (int)_data[7];
    }
}
