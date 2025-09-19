using TMPro;
using UnityEngine;

public class NpcNameTag : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTag;
    Transform owner;

    public void SetOwner(GameObject _owner, NpcDataSO _data)
    {
        owner = _owner.transform;

        nameTag.text = $"Lv.1 {_data.data.Name}";
    }
    public void SetBossColor()
    {
        nameTag.color = Color.red;
    }
    void LateUpdate()
    {
        transform.position = owner.position;
    }
}
