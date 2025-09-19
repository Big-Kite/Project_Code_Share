using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image[] slotUIs;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;

    bool have = false;
    ItemInstance itemInst = null;

    void Awake()
    {
        have = false;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickItemSlot);
    }
    public void Init(ItemInstance _itemInst)
    {
        if (_itemInst == null || _itemInst.itemKey == 0)
            return;

        itemInst = _itemInst;
        have = true;
        OnHave();
    }
    void OnHave()
    {
        var dataSO = DataRef.GetItemDB.GetData(itemInst.itemKey);
        if (dataSO == null)
            return;

        icon.sprite = dataSO.itemIcon;
        icon.SetNativeSize();

        foreach (var ui in slotUIs)
        {
            ui.gameObject.SetActive(true);
            ui.color = Define.GradeColor[dataSO.data.Grade - 1];
        }
        icon.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        // 무기 장비면 z : -45 로테이션 돌리기
        if(dataSO.data.MainType == (int)ItemMainType.EquipItem && dataSO.data.SubType == (int)EquipItemSubType.Weapon)
            icon.GetComponent<RectTransform>().localEulerAngles = new Vector3(0.0f, 0.0f, -45.0f);
        else
            icon.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;

        if (dataSO.data.MainType != (int)ItemMainType.EquipItem)
        {
            if(itemInst.amount > 1000000)
            {
                int temp = itemInst.amount / 1000000;
                text.text = $"{temp}M +";
            }
            else
                text.text = $"{itemInst.amount.ToString("N0")}";
        }
        else
            text.text = "Lv.1";
    }
    void OnClickItemSlot()
    {
        if (!have)
            return;

        // 아이템 인포 팝업을 띄우자
        var popup = PopupManager.Instance.OpenPopup(PopupType.ItemInfo);
        popup.GetComponent<PopupItemInfo>().Init(itemInst, true);
    }
}
