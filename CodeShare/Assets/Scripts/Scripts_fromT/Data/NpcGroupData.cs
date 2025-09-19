using UnityEngine;

[System.Serializable]
public class NpcGroupData
{
    [SerializeField] int key = 0;
    [SerializeField] int[] units = new int[9];

    public int Key => key;
    public int[] Units => units;

    public void SetData(params object[] _data)
    {
        key = (int)_data[0];
        units = (int[])_data[1];
    }
}
