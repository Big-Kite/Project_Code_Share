using UnityEngine;

public class StageDB : ScriptableObject
{
    public StageDataSO[] dataSOs;

    public StageDataSO GetData(int _key)
    {
        foreach (StageDataSO dataSO in dataSOs)
        {
            if (dataSO.GetKey() == _key)
                return dataSO;
        }
        Debug.LogError("Access out of range key!");
        return null;
    }
}
