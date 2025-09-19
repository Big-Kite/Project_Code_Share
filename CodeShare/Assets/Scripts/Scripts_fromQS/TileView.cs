using UnityEngine;

public class TileView : MonoBehaviour
{
    private SpriteRenderer sr;
    private static readonly Color DefaultColor = new Color(0f, 0f, 0f, 0.5f); // 투명 또는 매우 어둡게

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ClearHighlight(); // 기본 색 적용
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
