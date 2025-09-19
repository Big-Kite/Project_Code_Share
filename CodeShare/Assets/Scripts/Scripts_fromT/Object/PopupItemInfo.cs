using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupItemInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI infoText;

    [SerializeField] GameObject[] stars;

    [SerializeField] Image[] popupUIs;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Button closeButton;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;

    ItemInstance itemInst = null;
    bool forEquip = false;

    void Awake()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnClickClose);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnClickConfirm);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(OnClickCancel);
    }
    public void Init(ItemInstance _itemInst, bool _forEquip)
    {
        if (_itemInst == null || _itemInst.itemKey == 0)
            return;

        itemInst = _itemInst;
        forEquip = _forEquip;

        var dataSO = DataRef.GetItemDB.GetData(itemInst.itemKey);
        icon.sprite = dataSO.itemIcon;
        icon.SetNativeSize();

        nameText.text = dataSO.data.Name;
        foreach (var ui in popupUIs)
        {
            ui.color = Define.GradeColor[dataSO.data.Grade - 1];
        }

        if(dataSO.data.MainType != (int)ItemMainType.Coin)
        {
            for (int i = 0; i < dataSO.data.Grade; i++)
                stars[i].SetActive(true);
        }

        // 무기 장비면 z : -45 로테이션 돌리기
        if (dataSO.data.MainType == (int)ItemMainType.EquipItem && dataSO.data.SubType == (int)EquipItemSubType.Weapon)
            icon.GetComponent<RectTransform>().localEulerAngles = new Vector3(0.0f, 0.0f, -45.0f);
        else
            icon.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;

        if (dataSO.data.MainType == (int)ItemMainType.EquipItem)
        {
            text.text = "Lv.1";
            confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = forEquip ? "장착" : "해제";
        }
        else
        {
            text.text = "";
            confirmButton.gameObject.SetActive(false);
        }

        SetInfoText();
    }
    public void OnClickClose()
    {
        PopupManager.Instance.ClosePopup();
    }
    public void OnClickConfirm()
    {
        if (forEquip)
            PlayerData.Instance.SetEquipItem(itemInst);
        else
            PlayerData.Instance.RemoveEquipItem(itemInst);

        PopupManager.Instance.ClosePopup();
    }
    public void OnClickCancel()
    {

    }
    void SetInfoText()
    {
        var dataSO = DataRef.GetItemDB.GetData(itemInst.itemKey);

        infoText.text = dataSO.data.Info;
        switch (dataSO.data.MainType)
        {
            case (int)ItemMainType.Coin:
            case (int)ItemMainType.Material:
                {
                    infoText.text += $"\n\n보유량 : {itemInst.amount.ToString("N0")}";
                }
                break;
            case (int)ItemMainType.EquipItem:
                {
                    SetInfoText_EquiptSubType(dataSO.data.SubType);
                }
                break;
        }
    }
    void SetInfoText_EquiptSubType(int _subType)
    {
        switch(_subType)
        {
            case (int)EquipItemSubType.Weapon:
                {
                    infoText.text += $"\n\n공격력 : 1\n공격속도 : 1\n공격거리 : 1";
                }
                break;
            case (int)EquipItemSubType.Shield:
                {
                    infoText.text += $"\n\n방어력 : 1\n회피율 : 1";
                }
                break;
            case (int)EquipItemSubType.Helmet:
                {
                    infoText.text += $"\n\n회피율 : 1";
                }
                break;
            case (int)EquipItemSubType.Armor:
                {
                    infoText.text += $"\n\n방어력 : 1";
                }
                break;
            case (int)EquipItemSubType.Gauntlet:
                {
                    infoText.text += $"\n\n공격속도 : 1\n회피율 : 1";
                }
                break;
            case (int)EquipItemSubType.Boots:
                {
                    infoText.text += $"\n\n공격력 : 1\n회피율 : 1";
                }
                break;
        }
    }
}
