using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField] EquipSlot[] equipSlots;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotParent;

    int maxSlotCount = Define.DefaultItemSlotCount;
    int curSlotCount = 0;
    bool isDirty = false;

    List<ItemSlot> slotList = new List<ItemSlot>();

    void Start()
    {
        // 유저의 저장된 맥스 카운트를 넣어준다.
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
        curSlotCount = 0;
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
                curSlotCount++;

                if (curSlotCount >= maxSlotCount)
                    break;
            }
            if (curSlotCount >= maxSlotCount)
                break;
        }

        while (curSlotCount < maxSlotCount)
        {
            var slot = Instantiate(slotPrefab, slotParent).GetComponent<ItemSlot>();
            slotList.Add(slot);
            curSlotCount++;
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
