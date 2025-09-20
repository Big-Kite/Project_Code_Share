using UnityEngine;

public class ChargingPointer : MonoBehaviour
{
    RectTransform pointerRect;
    Animator anim;

    float fallSpeed = 0.0001f; // ���� �ӵ� // �̰ŵ� �����ΰ� �����ؼ� ������
    float PullSpeed = 0.0001f; // �̴� �ӵ�

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
        pos.x -= fallSpeed * _delta; // �⺻������ �������� �и�
        // �Է� �� ���������� �о���
        if (Input.GetMouseButton(0))
        { 
            pos.x += PullSpeed * _delta;
        }
        // ��� ����
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pointerRect.anchoredPosition = pos;
    }
    public void FocusOnOff(bool _isOn)
    {
        anim.enabled = _isOn;
    }
}
