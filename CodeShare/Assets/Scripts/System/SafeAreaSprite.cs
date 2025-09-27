using UnityEngine;
using UnityEngine.UI;

public class SafeAreaSprite : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] CanvasScaler canvasScaler;

    float screenX = 0.0f;
    float screenY = 0.0f;
    float spriteX = 0.0f;
    float spriteY = 0.0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 세이프 에어리어 가져오기
        Rect safeArea = Screen.safeArea;
        // 세이프 에어리어의 피벗 (0.5, 0.0) 위치 계산
        float centerX = safeArea.x + (safeArea.width * 0.5f);
        float bottomY = safeArea.y; // Safe Area의 맨 아래
        // 스크린 좌표를 월드 좌표로 변환
        Vector3 screenPos = new Vector3(centerX, bottomY, 0.0f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        screenY = Camera.main.orthographicSize * 2.0f;
        screenX = screenY * Camera.main.aspect;
        screenY = Mathf.Abs(worldPos.y) * 2.0f;

        Scaling();
        Positioning();
    }
    void Scaling()
    {
        float rect1Height = 600.0f; // 예: 500
        float rect2Height = 700.0f; // 예: 500
        float testHeight = Screen.height - Screen.safeArea.height;

        float screenHeight = Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2.0f;

        // CanvasScaler 기준 해상도
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        float match = canvasScaler.matchWidthOrHeight;

        // 캔버스 보정 스케일 계산
        float logWidth = Mathf.Log(Screen.width / referenceResolution.x, 2);
        float logHeight = Mathf.Log(Screen.height / referenceResolution.y, 2);
        float scaleFactor = Mathf.Pow(2, Mathf.Lerp(logWidth, logHeight, match));
        // 보정된 worldPerPixel
        float worldPerPixel = cameraHeight / screenHeight * scaleFactor;
        // 실제 월드 크기
        float rect1HeightInWorld = rect1Height * worldPerPixel;
        float rect2HeightInWorld = rect2Height * worldPerPixel;
        float testHeightInWorld = testHeight * worldPerPixel;

        // 리소스가 실제 그려지는 렌더 크기
        spriteX = spriteRenderer.bounds.size.x;
        spriteY = spriteRenderer.bounds.size.y;
        // 그리드가 그려져야할 렌더 크기
        float drawX = screenX * 0.95f;
        float drawY = Camera.main.orthographicSize * 2.0f;
        // 스케일링 값
        float fixX = drawX / spriteX;
        float fixY = drawY / spriteY;
        fixY -= (rect1HeightInWorld + rect2HeightInWorld + testHeightInWorld);
        // 적용
        transform.localScale = new Vector3(fixX, fixY, 1.0f);
    }
    void Positioning()
    {
        float pixelHeight = 700.0f; // 예: 500
        float screenHeight = Screen.height;
        // 카메라가 Orthographic일 때
        float cameraHeight = Camera.main.orthographicSize * 2f;
        // CanvasScaler 기준 해상도
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        float match = canvasScaler.matchWidthOrHeight;

        // 캔버스 보정 스케일 계산
        float logWidth = Mathf.Log(Screen.width / referenceResolution.x, 2);
        float logHeight = Mathf.Log(Screen.height / referenceResolution.y, 2);
        float scaleFactor = Mathf.Pow(2, Mathf.Lerp(logWidth, logHeight, match));
        // 보정된 worldPerPixel
        float worldPerPixel = cameraHeight / screenHeight * scaleFactor;
        // 실제 월드 크기
        float heightInWorld = pixelHeight * worldPerPixel;

        // 리소스가 실제 그려지는 렌더 크기
        spriteX = spriteRenderer.bounds.size.x;
        spriteY = spriteRenderer.bounds.size.y;
        // 화면 하단 끝 위치 먼저 잡기
        float screenHalfY = screenY * 0.5f;
        transform.position = new Vector3(0.0f, -screenHalfY + heightInWorld, 0.0f);
        // 화면 밖으로 나간 만큼 끌어 올리기
        transform.position = transform.position + new Vector3(0.0f, spriteY * 0.5f, 0.0f);
    }
}
