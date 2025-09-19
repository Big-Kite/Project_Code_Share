using UnityEngine;

public class PlayerCharacter : Singleton<PlayerCharacter>
{
    [SerializeField] SpriteRenderer weapon;
    [SerializeField] SpriteRenderer shield;
    [SerializeField] SpriteRenderer helmet;
    [SerializeField] SpriteRenderer armor;
    [SerializeField] SpriteRenderer gauntletL;
    [SerializeField] SpriteRenderer gauntletR;
    [SerializeField] SpriteRenderer bootsL;
    [SerializeField] SpriteRenderer bootsR;

    Sprite[] defaultSprites = new Sprite[6];

    void Awake()
    {
        defaultSprites[0] = helmet.sprite;
        defaultSprites[1] = armor.sprite;
        defaultSprites[2] = gauntletL.sprite;
        defaultSprites[3] = gauntletR.sprite;
        defaultSprites[4] = bootsL.sprite;
        defaultSprites[5] = bootsR.sprite;
    }
    public void TakeOnEquip(ItemInstance _itemInst)
    {
        if (_itemInst == null || _itemInst.itemKey == 0)
            return;

        var dataSO = DataRef.GetItemDB.GetData(_itemInst.itemKey);
        if (dataSO.data.MainType != (int)ItemMainType.EquipItem)
            return;

        switch (dataSO.data.SubType)
        {
            case (int)EquipItemSubType.Weapon:
                {
                    weapon.sprite = dataSO.itemIcon;
                    weapon.gameObject.SetActive(true);
                }
                break;
            case (int)EquipItemSubType.Shield:
                {
                    shield.sprite = dataSO.itemIcon;
                    shield.gameObject.SetActive(true);
                }
                break;
            case (int)EquipItemSubType.Helmet:
                {
                    helmet.sprite = dataSO.itemIcon;
                }
                break;
            case (int)EquipItemSubType.Armor:
                {
                    armor.sprite = dataSO.itemIcon;
                }
                break;
            case (int)EquipItemSubType.Gauntlet:
                {
                    gauntletL.sprite = dataSO.itemIcon;
                    gauntletR.sprite = dataSO.subItemIcon;
                }
                break;
            case (int)EquipItemSubType.Boots:
                {
                    bootsL.sprite = dataSO.itemIcon;
                    bootsR.sprite = dataSO.subItemIcon;
                }
                break;
        }
    }
    public void TakeOffEquip(ItemInstance _itemInst)
    {
        if (_itemInst == null || _itemInst.itemKey == 0)
            return;

        var dataSO = DataRef.GetItemDB.GetData(_itemInst.itemKey);
        if (dataSO.data.MainType != (int)ItemMainType.EquipItem)
            return;

        switch (dataSO.data.SubType)
        {
            case (int)EquipItemSubType.Weapon:
                {
                    weapon.gameObject.SetActive(false);
                }
                break;
            case (int)EquipItemSubType.Shield:
                {
                    shield.gameObject.SetActive(false);
                }
                break;
            case (int)EquipItemSubType.Helmet:
                {
                    helmet.sprite = defaultSprites[0];
                }
                break;
            case (int)EquipItemSubType.Armor:
                {
                    armor.sprite = defaultSprites[1];
                }
                break;
            case (int)EquipItemSubType.Gauntlet:
                {
                    gauntletL.sprite = defaultSprites[2];
                    gauntletR.sprite = defaultSprites[3];
                }
                break;
            case (int)EquipItemSubType.Boots:
                {
                    bootsL.sprite = defaultSprites[4];
                    bootsR.sprite = defaultSprites[5];
                }
                break;
        }
    }
}
