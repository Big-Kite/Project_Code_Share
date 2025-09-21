using UnityEngine;

public class PlayerCharacter : Singleton<PlayerCharacter>
{
    // 제작된 캐릭터 리소스가 파츠 애니메이션을 실행하는 형태로, 그에 맞게 캐릭터 파츠 컨트롤을 할 수 있게 구성했습니다.
    // 리소스 최초 의도는 클래스 체인지로 통짜 교체였는데, 파츠로 나뉘어진 리소스를 탈부착이 가능하게 변경하였습니다.

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
