using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField] EquipSlot[] equipSlots;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotParent;

    int slotCount = Define.DefaultItemSlotCount;
    bool isDirty = false;

    List<ItemSlot> slotList = new List<ItemSlot>();

    void Start()
    {
        // ������ ����� �ƽ� ī��Ʈ�� �־��ش�.
        isDirty = true;
    }
    void LateUpdate()
    {
        if (!isDirty)
            return;

        UpdateInventory();
        UpdateEquipSlots();
        isDirty = false;
    }
    public void SetDirty()
    {
        isDirty = true;
    }
    void UpdateInventory()
    {
        slotList.Clear();
        foreach (Transform slot in slotParent)
        {
            Destroy(slot.gameObject);
        }

        var playerBag = PlayerData.Instance.Bag;
        foreach (var items in playerBag)
        {
            foreach(var item in items.Value)
            {
                var slot = Instantiate(slotPrefab, slotParent).GetComponent<ItemSlot>();
                slot.Init(item);
                slotList.Add(slot);
            }
        }
    }
    void UpdateEquipSlots()
    {
        var equipList = PlayerData.Instance.EquipItem;
        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].Init(equipList[i]);
        }
    }
}
