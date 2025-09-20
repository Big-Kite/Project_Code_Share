using UnityEngine;

public class ItemDB : ScriptableObject
{
    // 데이터 파싱툴로 데이터를 담은 스크립터블 그룹을 배열로 담은 형태입니다.
    // 데이터의 키값을 비교하여 해당 스크립터블이 멤버로 들고있는 데이터 클래스를 반환합니다.
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
