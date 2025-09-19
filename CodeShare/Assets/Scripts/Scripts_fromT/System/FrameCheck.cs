using TMPro;
using UnityEngine;

public class FrameCheck : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;
    float delta;

    void Update()
    {
        // FPS 계산 (프레임마다 부드럽게 보정)
        delta += (Time.unscaledDeltaTime - delta) * 0.1f;
        float fps = 1.0f / delta;
        fpsText.text = $"FPS : {Mathf.Ceil(fps)}";
    }
}
