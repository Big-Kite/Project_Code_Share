using UnityEngine;

public class ItemDB : ScriptableObject
{
    public ItemDataSO[] dataSOs;
    public EquipItemDataSO[] equipDataSOs;
    public DropItemMapDataSO[] dropItemMapDataSOs;

    public ItemDataSO GetData(int _key)
    {
        foreach (ItemDataSO dataSO in dataSOs)
        {
            if (dataSO.GetKey() == _key)
                return dataSO;
        }
        Debug.LogError("Access out of range key!");
        return null;
    }
    public EquipItemDataSO GetEquipData(int _key)
    {
        foreach (EquipItemDataSO dataSO in equipDataSOs)
        {
            if (dataSO.GetKey() == _key)
                return dataSO;
        }
        Debug.LogError("Access out of range key!");
        return null;
    }
    public DropItemMapDataSO GetDropData(int _npcKey)
    {
        foreach (DropItemMapDataSO dataSO in dropItemMapDataSOs)
        {
            if (dataSO.GetNpcKey() == _npcKey)
                return dataSO;
        }
        Debug.LogError("Access out of range key!");
        return null;
    }
}
