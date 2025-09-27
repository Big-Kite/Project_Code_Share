using UnityEngine;

public class SpinArea : MonoBehaviour
{
    SpriteRenderer spriter;
    Color on = Color.yellow;

    bool isOn = false;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }
    public void CheckOn()
    {
        if (!isOn)
        {
            Debug.Log("On!");
            spriter.color = on;
        }
    }
}
