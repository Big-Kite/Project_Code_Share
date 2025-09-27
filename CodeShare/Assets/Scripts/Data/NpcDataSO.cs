using UnityEngine;

public class NpcDataSO : ScriptableObject
{
    public NpcData data = null;
    public GameObject prefab = null;

    public void SetDataSO(NpcData _data, GameObject _prefab)
    {
        data = _data;
        prefab = _prefab;
    }
    public int GetKey()
    {
        return data.Key;
    }
}
