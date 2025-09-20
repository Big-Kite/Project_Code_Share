using UnityEngine;

public class ItemDataSO : ScriptableObject
{
    // ������Ŭ������ ��ũ���ͺ�� ������ ������ ��ġ�� �ƴ϶� ���� ����� ���ҽ��� ���� �����Ϸ��� �ǵ��Դϴ�.
    // ���� ��Ÿ�� �߿� ���� ���Ƿ� ������� �ʱ� ���Ե� �ֽ��ϴ�.
    public ItemData data = null;
    public Sprite itemIcon = null;
    public Sprite subItemIcon = null;

    public void SetDataSO(ItemData _data, Sprite _icon, Sprite _subIcon = null)
    {
        data = _data;
        itemIcon = _icon;
        subItemIcon = _subIcon;
    }
    public int GetKey()
    {
        return data.Key;
    }
}
