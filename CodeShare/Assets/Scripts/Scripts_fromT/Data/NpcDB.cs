using UnityEngine;

public class NpcDB : ScriptableObject
{
    public NpcDataSO[] dataSOs;
    public NpcGroupDataSO[] groupDataSOs;

    public NpcDataSO GetNpcData(int _key)
    {
        foreach(NpcDataSO dataSO in dataSOs)
        {
            if(dataSO.GetKey() == _key)
                return dataSO;
        }
        Debug.LogError("Access out of range key!");
        return null;
    }
    public NpcGroupDataSO GetNpcGroupData(int _key)
    {
        foreach (NpcGroupDataSO dataSO in groupDataSOs)
        {
            if (dataSO.GetKey() == _key)
                return dataSO;
        }
        Debug.LogError("Access out of range key!");
        return null;
    }
}
