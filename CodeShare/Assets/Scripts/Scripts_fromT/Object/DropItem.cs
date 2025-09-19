using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer itemIcon;

    public void Init(int _npcKey)
    {
        var dropData = DataRef.GetItemDB.GetDropData(_npcKey);
        // ÀüÃ¼ °¡ÁßÄ¡ ±¸ÇÏ±â
        int totalWeight = 0;
        foreach(var data in dropData.data.DropList)
        {
            totalWeight += data.DropWeight;
        }
        // È®·ü »Ì±â
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

        // ÀÌÈÄ È¹µæ¼ö·® ºÐ¹è
    }
}
