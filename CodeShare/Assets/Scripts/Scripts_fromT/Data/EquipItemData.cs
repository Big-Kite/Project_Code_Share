using UnityEngine;

[System.Serializable]
public class EquipItemData
{
    [SerializeField] int key = 0;
    [SerializeField] int hp = 0;
    [SerializeField] int mp = 0;
    [SerializeField] int atk = 0;
    [SerializeField] float atkSpeed = 0.0f;
    [SerializeField] float cri = 0.0f;
    [SerializeField] int def = 0;
    [SerializeField] float eva = 0.0f;

    public int Key => key;
    public int Hp => hp;
    public int Mp => mp;
    public int Atk => atk;
    public float AtkSpeed => atkSpeed;
    public float Cri => cri;
    public int Def => def;
    public float Eva => eva;

    public void SetData(params object[] _data)
    {
        key = (int)_data[0];
        hp = (int)_data[1];
        mp = (int)_data[2];
        atk = (int)_data[3];
        atkSpeed = (float)_data[4];
        cri = (float)_data[5];
        def = (int)_data[6];
        eva = (float)_data[7];
    }
}
