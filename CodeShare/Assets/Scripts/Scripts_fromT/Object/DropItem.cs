using UnityEngine;

public class DropItem : MonoBehaviour
{
    // 드랍아이템마다 가중치가 있고, 몬스터마다 가지고 있는 목록의 가중치합에서 난수가 위치하는 범위 구간의 드랍아이템을 생성합니다.

    [SerializeField] SpriteRenderer itemIcon;

    public void Init(int _npcKey)
    {
        var dropData = DataRef.GetItemDB.GetDropData(_npcKey);
        // 전체 가중치 구하기
        int totalWeight = 0;
        foreach(var data in dropData.data.DropList)
        {
            totalWeight += data.DropWeight;
        }
        // 확률 뽑기
        int randomSeed = Random.Range(0, totalWeight);
        int resultItemKey = 0, curWeight = 0;
        foreach (var data in dropData.data.DropList)
        {
            curWeight += data.DropWeight;
            if(randomSeed < curWeight)
            {
                resultItemKey = data.DropItem;
                break;
            }
        }

        var resultItem = DataRef.GetItemDB.GetData(resultItemKey);
        itemIcon.sprite = resultItem.itemIcon;

        // 이후 획득수량 분배
    }
}
