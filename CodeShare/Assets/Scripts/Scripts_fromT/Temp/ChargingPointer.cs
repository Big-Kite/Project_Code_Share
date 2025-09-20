using UnityEngine;

public class ChargingPointer : MonoBehaviour
{
    RectTransform pointerRect;
    Animator anim;

    float fallSpeed = 0.0001f; // 낙하 속도 // 이거도 디파인값 삭제해서 값없음
    float PullSpeed = 0.0001f; // 미는 속도

    float maxX = 0.0f;
    public float GetPosX { get { return pointerRect.anchoredPosition.x; } }

    void Awake()
    {
        pointerRect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }
    public void SetBounds(RectTransform _base)
    {
        float halfWidth = _base.rect.width * 0.5f;
        maxX = halfWidth;

        anim.enabled = false;

        pointerRect.anchoredPosition = Vector2.zero;
    }
    public void HandleInput(float _delta)
    {
        Vector3 pos = pointerRect.anchoredPosition;
        pos.x -= fallSpeed * _delta; // 기본적으로 왼쪽으로 밀림
        // 입력 시 오른쪽으로 밀어줌
        if (Input.GetMouseButton(0))
        { 
            pos.x += PullSpeed * _delta;
        }
        // 경계 제한
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pointerRect.anchoredPosition = pos;
    }
    public void FocusOnOff(bool _isOn)
    {
        anim.enabled = _isOn;
    }
}
