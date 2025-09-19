using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerDataSaveModel
{
    public int Hp;
    public int Mp;
    public int Atk;
    public float AtkSpeed;
    public float AtkDist;
    public int Def;
    public float Eva;
    public int InventorySlotCount;

    public List<BagWrapper> Bags;
    public ItemInstance[] EquipItemList;
    public int[] EquipSkill;
}

public class PlayerData : Singleton<PlayerData>
{
    // ü�� ��
    public int Hp { get; private set; } = 5000;
    public int Mp { get; private set; } = 10;
    // ���� ��
    public int Atk { get; private set; } = 5;
    public float AtkSpeed { get; private set; } = 1.0f;
    public float AtkDist { get; private set; } = 1.0f;
    // ��� ��
    public int Def { get; private set; } = 5;
    public float Eva { get; private set; } = 0.0f;
    
    // ������ ��
    // �� ���� ī��Ʈ
    public int InventorySlotCount { get; private set; } = Define.DefaultItemSlotCount;
    // �� ���� ������
    public Dictionary<int, List<ItemInstance>> Bag { get; private set; } = new Dictionary<int, List<ItemInstance>>();
    // �� ���� ������
    public ItemInstance[] EquipItem { get; private set; } = new ItemInstance[6];

    // �ӽ� ���� ��ų��
    public int[] EquipSkill { get; private set; } = new int[10];
    // ��ȭ �� (���� �ؾߵǳ�?) �ϴ� ����

    string saveFilePath => Path.Combine(Application.persistentDataPath, "PlayerData.json");
    bool initialized = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
        initialized = false;
    }
    void OnApplicationQuit()
    {
        SavePlayerData();

        PlayerPrefs.SetString("LastLogin", DateTime.UtcNow.ToString("o"));
        PlayerPrefs.Save();

        Debug.Log($"��Ʈ �α��� ����");
    }
    public void Init(params int[] _data)
    {
        if (initialized)
            return;

        PlayerPrefs.DeleteAll();

        StartCoroutine(CoCheckHack());
        GetStoredData();
        initialized = true;
    }
    IEnumerator CoCheckHack()
    {
        // ����ȭ�� ������ �ð��� ��������
        UnityWebRequest request = UnityWebRequest.Head("https://google.com"); // �Ǵ� �ٸ� �ŷ��� �� �ִ� ����
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("��Ʈ��ũ ��û ����: " + request.error);
            yield break;
        }

        string dateHeader = request.GetResponseHeader("date");
        if (string.IsNullOrEmpty(dateHeader))
        {
            Debug.LogError("Date ����� �����ϴ�.");
            yield break;
        }

        if (DateTime.TryParse(dateHeader, out DateTime serverTime))
        {
            DateTime utcServerTime = serverTime.ToUniversalTime(); // GMT �� UTC ��ȯ (��ǻ� ����, Ȯ���� �ϱ� ���� ToUniversalTime())
            //string formattedUtcServerTime = utcServerTime.ToString("o"); // ISO 8601 �������� ��ȯ (��: 2025-07-09T07:00:00.0000000Z)
            //DateTime savedTime = DateTime.Parse(formattedUtcServerTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime now = DateTime.UtcNow;

            TimeSpan diff = now - utcServerTime;

            if (diff.TotalSeconds > 10.0f)
            {
                Debug.LogError("��� �ð� ���� Ž��");
                yield break;
            }
        }
        else
        {
            Debug.LogError("�Ľ̵� �����ð� ����");
            yield break;
        }
        Debug.Log("���� �ð� üũ ���");
    }
    void GetStoredData()
    {
        bool newUser = false;

        var lastLogin = PlayerPrefs.GetString("LastLogin", string.Empty);
        if (string.IsNullOrEmpty(lastLogin))
            newUser = true;

        if (newUser)
        { // ������ ���� ������ �缳ġ�� ��쵵 �����ؾ�������, ������ ���� ���������� �׻� �ű������� ����.
            Bag.Add(1, new List<ItemInstance>() { new ItemInstance(1, 12314) });
            Bag.Add(2, new List<ItemInstance>() { new ItemInstance(2, 43) });
            Bag.Add(3, new List<ItemInstance>() { new ItemInstance(3, 1235) });
            Bag.Add(4, new List<ItemInstance>() { new ItemInstance(4, 4564) });
            Bag.Add(5, new List<ItemInstance>() { new ItemInstance(5, 2343) });
            Bag.Add(6, new List<ItemInstance>() { new ItemInstance(6, 56786) });
            Bag.Add(101, new List<ItemInstance>() { new ItemInstance(101), new ItemInstance(101), new ItemInstance(101) });
            Bag.Add(105, new List<ItemInstance>() { new ItemInstance(105), new ItemInstance(105), new ItemInstance(105) });
            Bag.Add(203, new List<ItemInstance>() { new ItemInstance(203), new ItemInstance(203) });
            Bag.Add(304, new List<ItemInstance>() { new ItemInstance(304) });
            Bag.Add(502, new List<ItemInstance>() { new ItemInstance(502) });
            Bag.Add(506, new List<ItemInstance>() { new ItemInstance(506) });
            Bag.Add(606, new List<ItemInstance>() { new ItemInstance(606) });

            EquipSkill = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 11, 12 }.ToArray();
            SavePlayerData();
        }
        else
        {
            LoadPlayerData();
        }
    }
    void SavePlayerData()
    {
        PlayerDataSaveModel saveModel = new PlayerDataSaveModel
        {
            Hp = Hp,
            Mp = Mp,
            Atk = Atk,
            AtkSpeed = AtkSpeed,
            AtkDist = AtkDist,
            Def = Def,
            Eva = Eva,
            InventorySlotCount = InventorySlotCount,
            Bags = ConvertDictToWrapper(Bag),
            EquipItemList = EquipItem,
            EquipSkill = EquipSkill
        };

        string json = JsonUtility.ToJson(saveModel);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();

        string jsonToFile = JsonUtility.ToJson(saveModel, true); // ���� ���� ����
        File.WriteAllText(saveFilePath, jsonToFile);

        Debug.Log($"�����͸� ���Ͽ� �����߽��ϴ�: {saveFilePath}");
    }
    void LoadPlayerData()
    {
        if (!PlayerPrefs.HasKey("PlayerData") || !File.Exists(saveFilePath))
        {
            Debug.LogWarning("����� ������ �����ϴ�.");
            return;
        }

        string json = PlayerPrefs.GetString("PlayerData");
        PlayerDataSaveModel saveModel = JsonUtility.FromJson<PlayerDataSaveModel>(json);

        string jsonFromFile = File.ReadAllText(saveFilePath);
        PlayerDataSaveModel saveModelFromFile = JsonUtility.FromJson<PlayerDataSaveModel>(jsonFromFile);
        Debug.Log($"�����͸� ���Ͽ��� �о����ϴ�: {saveFilePath}");

        Hp = saveModel.Hp;
        Mp = saveModel.Mp;
        Atk = saveModel.Atk;
        AtkSpeed = saveModel.AtkSpeed;
        AtkDist = saveModel.AtkDist;
        Def = saveModel.Def;
        Eva = saveModel.Eva;
        InventorySlotCount = saveModel.InventorySlotCount;

        Bag.Clear();
        Bag = ConvertWrapperToDict(saveModel.Bags);

        EquipItem = saveModel.EquipItemList.ToArray();
        EquipSkill = saveModel.EquipSkill.ToArray();
    }
    public List<BagWrapper> ConvertDictToWrapper(Dictionary<int, List<ItemInstance>> _dict)
    {
        var list = new List<BagWrapper>();
        foreach (var pair in _dict)
        {
            BagWrapper wrapper = new BagWrapper();
            wrapper.key = pair.Key;
            wrapper.datas = pair.Value;
            list.Add(wrapper);
        }
        return list;
    }
    public Dictionary<int, List<ItemInstance>> ConvertWrapperToDict(List<BagWrapper> _list)
    {
        var dict = new Dictionary<int, List<ItemInstance>>();
        foreach (var wrapper in _list)
        {
            dict[wrapper.key] = wrapper.datas;
        }
        return dict;
    }
    public void SetEquipItem(ItemInstance _itemInst)
    {
        var dataSO = DataRef.GetItemDB.GetData(_itemInst.itemKey);
        if (dataSO.data.MainType != (int)ItemMainType.EquipItem)
            return;

        if (Bag.ContainsKey(_itemInst.itemKey) == false)
            return;

        if (EquipItem[dataSO.data.SubType - 1].itemKey != 0)
        {
            Bag[EquipItem[dataSO.data.SubType - 1].itemKey].Add(EquipItem[dataSO.data.SubType - 1]);
        }

        EquipItem[dataSO.data.SubType - 1] = _itemInst;
        Bag[_itemInst.itemKey].Remove(_itemInst);

        if (PlayerCharacter.HasInstance())
        {
            PlayerCharacter.Instance.TakeOnEquip(_itemInst);
        }
        if (InventoryController.HasInstance())
        {
            InventoryController.Instance.SetDirty();
        }
    }
    public void RemoveEquipItem(ItemInstance _itemInst)
    {
        var dataSO = DataRef.GetItemDB.GetData(_itemInst.itemKey);
        if (dataSO.data.MainType != (int)ItemMainType.EquipItem)
            return;

        int i = 0;
        for (; i < EquipItem.Length; i++)
        {
            if (EquipItem[i] == _itemInst)
                break;
        }

        Bag[EquipItem[i].itemKey].Add(EquipItem[i]);
        EquipItem[i] = null;
        EquipItem[i] = new ItemInstance();

        if (PlayerCharacter.HasInstance())
        {
            PlayerCharacter.Instance.TakeOffEquip(_itemInst);
        }
        if (InventoryController.HasInstance())
        {
            InventoryController.Instance.SetDirty();
        }
    }
    public void AddItem(ItemInstance _itemInst, int _amount)
    {

    }
    public void RemoveItem()
    {

    }
    public void StoredEquipItem()
    {
        if (PlayerCharacter.HasInstance() == false)
            return;

        foreach (var storedItem in EquipItem)
        {
            if (storedItem.itemKey == 0)
                continue;

            PlayerCharacter.Instance.TakeOnEquip(storedItem);
        }
    }
}