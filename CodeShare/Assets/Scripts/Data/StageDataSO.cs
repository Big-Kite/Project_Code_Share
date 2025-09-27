using UnityEngine;

public class StageDataSO : ScriptableObject
{
    public StageData data = null;
    public GameObject fieldPrefab = null;
    public GameObject battlePrefab = null;

    public void SetDataSO(StageData _data, GameObject _field, GameObject _battle)
    {
        data = _data;
        fieldPrefab = _field;
        battlePrefab = _battle;
    }
    public int GetKey()
    {
        return data.Key;
    }
}
