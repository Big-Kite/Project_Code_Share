using UnityEngine;

public class TileView : MonoBehaviour
{
    private SpriteRenderer sr;
    private static readonly Color DefaultColor = new Color(0f, 0f, 0f, 0.5f); // ���� �Ǵ� �ſ� ��Ӱ�

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ClearHighlight(); // �⺻ �� ����
    }

    public void SetHighlight(Color color)
    {
        sr.color = color;
    }

    public void ClearHighlight()
    {
        sr.color = DefaultColor;
    }
}
