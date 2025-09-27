using UnityEngine;
using UnityEngine.UI;

public class SafeAreaImage : MonoBehaviour
{
    RectTransform Panel; // 자기자신
    //CanvasScaler CanvasScaler; // 캔버스의 스케일러

    Rect CurSafeArea = new Rect(0, 0, 0, 0);
    Vector2Int curScreenSize = new Vector2Int(0, 0);
    ScreenOrientation curOrientation = ScreenOrientation.AutoRotation;

    RectTransform x_pos;
    LayoutGroup x_layout;

    void Awake()
    {
        Panel = GetComponent<RectTransform>();
        //CanvasScaler = GetComponentInParent<CanvasScaler>();
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        Rect safeArea = Screen.safeArea;
        if (safeArea != CurSafeArea || Screen.width != curScreenSize.x || Screen.height != curScreenSize.y || Screen.orientation != curOrientation)
        {
            curScreenSize.x = Screen.width;
            curScreenSize.y = Screen.height;
            curOrientation = Screen.orientation;
            CurSafeArea = safeArea;

            float widthRate = (float)Screen.height / (float)Screen.width;

            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Rect r = Screen.safeArea;
        if (Screen.width > 0 && Screen.height > 0)
        {
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
            {
                Panel.anchorMin = anchorMin;
                Panel.anchorMax = anchorMax;
            }
        }

        if (x_pos != null)
        {
            x_pos.anchoredPosition = new Vector2(-r.x, 0);
        }

        if (x_layout != null)
        {
            if ((int)r.x == 0)
            {
                x_layout.padding.left = (int)r.x + 100;
            }
            else
            {
                x_layout.padding.left = (int)r.x + 20;
            }
            x_layout.CalculateLayoutInputHorizontal();
            x_layout.SetLayoutHorizontal();
        }
    }
}