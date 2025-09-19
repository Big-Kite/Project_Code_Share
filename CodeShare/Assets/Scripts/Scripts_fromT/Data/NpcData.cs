using UnityEngine;

[System.Serializable]
public class NpcData
{
    [SerializeField] int key = 0;
    [SerializeField] string name = string.Empty;
    [SerializeField] string prefabRef = string.Empty;
    [SerializeField] int hp = 0;
    [SerializeField] int mp = 0;
    [SerializeField] int atk = 0;
    [SerializeField] float atkSpeed = 0.0f;
    [SerializeField] float atkDist = 0.0f;
    [SerializeField] int projectileType = 0;
    [SerializeField] int def = 0;
    [SerializeField] float eva = 0.0f;
    [SerializeField] int skillNo = 0;

    public int Key => key;
    public string Name => name;
    public string PrefabRef => prefabRef;
    public int Hp => hp;
    public int Mp => mp;
    public int Atk => atk;
    public float AtkSpeed => atkSpeed;
    public float AtkDist => atkDist;
    public int ProjectileType => projectileType;
    public int Def => def;
    public float Eva => eva;
    public int SkillNo => skillNo;

    public void SetData(params object[] _data)
    {
        key = (int)_data[0];
        name = (string)_data[1];
        prefabRef = (string)_data[2];
        hp = (int)_data[3];
        mp = (int)_data[4];
        atk = (int)_data[5];
        atkSpeed = (float)_data[6];
        atkDist = (float)_data[7];
        projectileType = (int)_data[8];
        def = (int)_data[9];
        eva = (float)_data[10];
        skillNo = (int)_data[11];
    }
}