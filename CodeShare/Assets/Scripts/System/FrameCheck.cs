using TMPro;
using UnityEngine;

public class FrameCheck : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;
    float delta;

    void Update()
    {
        // FPS ��� (�����Ӹ��� �ε巴�� ����)
        delta += (Time.unscaledDeltaTime - delta) * 0.1f;
        float fps = 1.0f / delta;
        fpsText.text = $"FPS : {Mathf.Ceil(fps)}";
    }
}
