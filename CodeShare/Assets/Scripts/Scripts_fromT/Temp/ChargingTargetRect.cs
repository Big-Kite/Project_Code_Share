using UnityEngine;
using UnityEngine.UI;

public class ChargingTargetRect : MonoBehaviour
{
    [Header("UI Elements")] // 인스펙터 세팅
    [SerializeField] Image ChargingFrame;
    [SerializeField] Image ChargingValue;
    [SerializeField] GameObject dirLeft;
    [SerializeField] GameObject dirRight;
    
    RectTransform targetRect;
    //ChargingRectState state = ChargingRectState.Stay;

    Color onPoint = Color.white; // 컬러 값
    Color outPoint = new Color(1.0f, 0.48f, 0.0f, 0.8f);

    float baseHalfWidth = 0.0f;
    float maxX = 0.0f; // 범위 값
    
    int dir = 0; // -1 왼쪽 0 머뭄 1 오른쪽
    float moveSpeed = 250.0f; // 속도
    float stayTime = 0.0f; // 움직인 후 머무는 시간

    float maxWidth = 0.0001f; // 원래 디파인 값 있었는데 삭제해서 값 없음 
    float minWidth = 0.0001f;
    float height = 100.0f;

    float holdingTime = 0.0f;
    float onceAtkTime = 1.0f; // 임시 공속 1초/ 1초당 한대

    Animator anim;

    void Awake()
    {
        targetRect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }
    public void SetBounds(RectTransform _base)
    {
        targetRect.sizeDelta = new Vector2(maxWidth, height);

        baseHalfWidth = _base.rect.width * 0.5f;
        maxX = baseHalfWidth - (targetRect.rect.width * 0.5f);

        Vector3 pos = targetRect.anchoredPosition;
        pos.x = 0.0f;
        targetRect.anchoredPosition = pos;

        ChargingFrame.color = onPoint;
        ChargingValue.fillAmount = 0.0f;
    }
    public void HandleUpdate(float _delta)
    {
        SetMoveState(_delta);
        Move(_delta);
    }
    public void BlinkingOff()
    {
        dirLeft.SetActive(false);
        dirRight.SetActive(false);
    }
    void SwitchState()
    {
        //if (state == ChargingRectState.Stay)
        //{
        //    state = ChargingRectState.Moving;
        //    dirLeft.SetActive(false);
        //    dirRight.SetActive(false);
        //}
        //else
        //{
        //    state = ChargingRectState.Stay;
        //    dir = Random.Range(-1, 2); // 방향 설정
        //
        //    if (dir < 0)
        //        dirLeft.SetActive(true);
        //    else if (dir > 0)
        //        dirRight.SetActive(true);
        //}
        stayTime = Random.Range(0.3f, 1.0f);
    }
    void SetMoveState(float _delta)
    {
        stayTime -= _delta;

        if (stayTime <= 0.0f)
            SwitchState();
    }
    void Move(float _delta)
    {
        //if (state == ChargingRectState.Stay)
        //    return;

        Vector3 pos = targetRect.anchoredPosition;

        pos.x += (moveSpeed * _delta) * dir;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        targetRect.anchoredPosition = pos;

        stayTime -= _delta;
        if (stayTime <= 0.0f)
            SwitchState();
    }
    public bool IsPointerInTarget(float _x, float _delta)
    {
        float targetMin = targetRect.anchoredPosition.x - (targetRect.rect.width * 0.5f);
        float targetMax = targetRect.anchoredPosition.x + (targetRect.rect.width * 0.5f);

        bool isIn = _x >= targetMin && _x <= targetMax;
        ChargingFrame.color = isIn ? onPoint : outPoint;

        if (isIn)
        {
            holdingTime += _delta;
            float ratio = Mathf.Clamp01(holdingTime / onceAtkTime);

            ChargingValue.fillAmount = ratio;
            CheckHolding();
        }
        else
        {
            holdingTime = 0.0f;
            ChargingValue.fillAmount = 0.0f;
        }

        return isIn;
    }
    void CheckHolding()
    {
        if(holdingTime >= onceAtkTime)
        {
            holdingTime = 0.0f;
            ChargingValue.fillAmount = 0.0f;

            anim.Play("STACKPUSH", 0, 0.0f);
            ChargingController.Instance.StackUpAttack();
        }
    }
    public void SetScale()
    {
        float t = Mathf.Clamp01(ChargingController.Instance.CumulatedTime / ChargingController.Instance.TimeLimit); // 0~1로 정규화
        float newWidth = Mathf.Lerp(maxWidth, minWidth, t);

        targetRect.sizeDelta = new Vector2(newWidth, height);

        //Vector2 size = targetRect.sizeDelta;
        //size.x = newWidth;
        //targetRect.sizeDelta = size;

        maxX = baseHalfWidth - (targetRect.rect.width * 0.5f);
    }
}
