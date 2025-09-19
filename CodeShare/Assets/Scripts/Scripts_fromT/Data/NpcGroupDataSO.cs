using UnityEngine;

public class NpcGroupDataSO : ScriptableObject
{
    public NpcGroupData data = null;

    public void SetDataSO(NpcGroupData _data)
    {
        data = _data;
    }
    public int GetKey()
    {
        return data.Key;
    }
}
