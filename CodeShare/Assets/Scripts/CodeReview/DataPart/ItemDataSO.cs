using UnityEngine;

public class ItemDataSO : ScriptableObject
{
    // 데이터클래스를 스크립터블로 래핑한 이유는 수치뿐 아니라 씬에 사용할 리소스를 같이 공유하려는 의도입니다.
    // 또한 런타임 중에 값을 임의로 변경되지 않기 위함도 있습니다.
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
