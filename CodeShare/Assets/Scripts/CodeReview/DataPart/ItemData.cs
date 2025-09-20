using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [SerializeField] int key = 0;
    [SerializeField] string name = string.Empty;
    [SerializeField] string spriteName = string.Empty;
    [SerializeField] int mainType = 0;
    [SerializeField] int subType = 0;
    [SerializeField] int maxAmount = 0;
    [SerializeField] int grade = 0;
    [SerializeField] string info = string.Empty;

    public int Key => key;
    public string Name => name;
    public string SpriteName => spriteName;
    public int MainType => mainType;
    public int SubType => subType;
    public int MaxAmount => maxAmount;
    public int Grade => grade;
    public string Info => info;

    public void SetData(params object[] _data)
    {
        key = (int)_data[0];
        name = (string)_data[1];
        spriteName = (string)_data[2];
        mainType = (int)_data[3];
        subType = (int)_data[4];
        maxAmount = (int)_data[5];
        grade = (int)_data[6];
        info = (string)_data[7];
    }
}

[System.Serializable]
public class ItemInstance
{
    public string id;
    public int itemKey;
    public int amount;

    public ItemInstance()
    {
        id = "";
        itemKey = 0;
        amount = 0;
    }
    public ItemInstance(int _key, int _curAmount = 1)
    {
        id = DateTime.UtcNow.ToString("o");
        itemKey = _key;
        amount = _curAmount;
    }
}

[System.Serializable]
public class BagWrapper
{
    public int key;
    public List<ItemInstance> datas;
}