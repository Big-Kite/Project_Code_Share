using TMPro;
using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;

    void OnDisable()
    {
        text.text = "0 %";
    }
    public void SetPercent(float _per)
    {
        float percent = _per * 100.0f;
        text.text = $"{percent.ToString("0")} %";
    }
}
