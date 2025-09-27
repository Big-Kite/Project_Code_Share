using UnityEngine;

public class EquipItemDataSO : ScriptableObject
{
    public EquipItemData data = null;

    public void SetDataSO(EquipItemData _data)
    {
        data = _data;
    }
    public int GetKey()
    {
        return data.Key;
    }
}
