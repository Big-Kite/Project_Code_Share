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
        // ������ ����� ��������
        Rect safeArea = Screen.safeArea;
        // ������ ������� �ǹ� (0.5, 0.0) ��ġ ���
        float centerX = safeArea.x + (safeArea.width * 0.5f);
        float bottomY = safeArea.y; // Safe Area�� �� �Ʒ�
        // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
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
        float rect1Height = 600.0f; // ��: 500
        float rect2Height = 700.0f; // ��: 500
        float testHeight = Screen.height - Screen.safeArea.height;

        float screenHeight = Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2.0f;

        // CanvasScaler ���� �ػ�
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        float match = canvasScaler.matchWidthOrHeight;

        // ĵ���� ���� ������ ���
        float logWidth = Mathf.Log(Screen.width / referenceResolution.x, 2);
        float logHeight = Mathf.Log(Screen.height / referenceResolution.y, 2);
        float scaleFactor = Mathf.Pow(2, Mathf.Lerp(logWidth, logHeight, match));
        // ������ worldPerPixel
        float worldPerPixel = cameraHeight / screenHeight * scaleFactor;
        // ���� ���� ũ��
        float rect1HeightInWorld = rect1Height * worldPerPixel;
        float rect2HeightInWorld = rect2Height * worldPerPixel;
        float testHeightInWorld = testHeight * worldPerPixel;

        // ���ҽ��� ���� �׷����� ���� ũ��
        spriteX = spriteRenderer.bounds.size.x;
        spriteY = spriteRenderer.bounds.size.y;
        // �׸��尡 �׷������� ���� ũ��
        float drawX = screenX * 0.95f;
        float drawY = Camera.main.orthographicSize * 2.0f;
        // �����ϸ� ��
        float fixX = drawX / spriteX;
        float fixY = drawY / spriteY;
        fixY -= (rect1HeightInWorld + rect2HeightInWorld + testHeightInWorld);
        // ����
        transform.localScale = new Vector3(fixX, fixY, 1.0f);
    }
    void Positioning()
    {
        float pixelHeight = 700.0f; // ��: 500
        float screenHeight = Screen.height;
        // ī�޶� Orthographic�� ��
        float cameraHeight = Camera.main.orthographicSize * 2f;
        // CanvasScaler ���� �ػ�
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        float match = canvasScaler.matchWidthOrHeight;

        // ĵ���� ���� ������ ���
        float logWidth = Mathf.Log(Screen.width / referenceResolution.x, 2);
        float logHeight = Mathf.Log(Screen.height / referenceResolution.y, 2);
        float scaleFactor = Mathf.Pow(2, Mathf.Lerp(logWidth, logHeight, match));
        // ������ worldPerPixel
        float worldPerPixel = cameraHeight / screenHeight * scaleFactor;
        // ���� ���� ũ��
        float heightInWorld = pixelHeight * worldPerPixel;

        // ���ҽ��� ���� �׷����� ���� ũ��
        spriteX = spriteRenderer.bounds.size.x;
        spriteY = spriteRenderer.bounds.size.y;
        // ȭ�� �ϴ� �� ��ġ ���� ���
        float screenHalfY = screenY * 0.5f;
        transform.position = new Vector3(0.0f, -screenHalfY + heightInWorld, 0.0f);
        // ȭ�� ������ ���� ��ŭ ���� �ø���
        transform.position = transform.position + new Vector3(0.0f, spriteY * 0.5f, 0.0f);
    }
}
