using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DropItemMapData
{
    [SerializeField] int npcKey = 0;
    [SerializeField] List<DropItemData> dropList;

    public int NpcKey => npcKey;
    public List<DropItemData> DropList => dropList;

    public void SetData(int _npcKey, List<DropItemData> _list)
    {
        npcKey = _npcKey;
        dropList = _list;
    }
}

[System.Serializable]
public class DropItemData
{
    [SerializeField] int key = 0;
    [SerializeField] int dropItem = 0;
    [SerializeField] int dropWeight = 0;
    [SerializeField] int min = 0;
    [SerializeField] int max = 0;

    public int Key => key;
    public int DropItem => dropItem;
    public int DropWeight => dropWeight;
    public int Min => min;
    public int Max => max;

    public void SetData(params object[] _data)
    {
        key = (int)_data[0];
        dropItem = (int)_data[1];
        dropWeight = (int)_data[2];
        min = (int)_data[3];
        max = (int)_data[4];
    }
}