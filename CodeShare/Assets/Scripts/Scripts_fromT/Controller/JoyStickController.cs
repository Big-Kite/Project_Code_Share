using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoyStickController : MonoBehaviour
{
    [SerializeField] RectTransform body;
    [SerializeField] RectTransform handle;

    Canvas attachedCanvas;
    RectTransform canvasRT;
    Camera canvasCam;

    bool isTouch = false;
    Vector2 touchPos = Vector2.zero;
    Vector2 moveDir = Vector2.zero;

    bool initialized = false;

    void Start()
    {
        //var safeArea = FindObjectsByType<SafeAreaImage>(FindObjectsSortMode.None).FirstOrDefault();
        var canvas = GetComponentInParent<Canvas>();
        if(canvas != null)
        {
            attachedCanvas = canvas;
            canvasRT = attachedCanvas.GetComponent<RectTransform>();
            canvasCam = attachedCanvas.worldCamera;
            initialized = true;
        }
        body.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!initialized)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ReadTouchValue(out touchPos);

            body.anchoredPosition = touchPos;
            handle.anchoredPosition = Vector2.zero;
            body.gameObject.SetActive(true);
            isTouch = true;
        }
        else if (isTouch && Input.GetMouseButton(0))
        {
            ReadTouchValue(out Vector2 movedPos);

            moveDir = movedPos - touchPos;
            moveDir.Normalize();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            handle.anchoredPosition = Vector2.zero;
            body.gameObject.SetActive(false);

            moveDir = Vector2.zero;
            isTouch = false;
        }
    }
    void LateUpdate()
    {
        handle.anchoredPosition = Vector2.zero + moveDir * 50;
    }
    void ReadTouchValue(out Vector2 _vec)
    {
        Vector2 screenPos;

        if (Touchscreen.current == null)
            screenPos = Input.mousePosition;
        else
            screenPos = Touchscreen.current.primaryTouch.position.ReadValue(); // ¶Ç´Â Input.mousePosition

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, screenPos, canvasCam, out _vec);
    }
}
