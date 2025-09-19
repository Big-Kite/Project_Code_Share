using UnityEngine;

public class DropItemMapDataSO : ScriptableObject
{
    public static int index = 0;
    public DropItemMapData data = null;

    public void SetDataSO(DropItemMapData _data)
    {
        data = _data;
    }
    public int GetNpcKey()
    {
        return data.NpcKey;
    }
}
