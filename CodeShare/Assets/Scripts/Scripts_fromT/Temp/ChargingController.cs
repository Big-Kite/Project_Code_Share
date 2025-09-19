using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingController : Singleton<ChargingController>
{
    [Header("UI Elements")] // 인스펙터 세팅
    [SerializeField] Button attackButton;
    [SerializeField] ChargingPointer pointer;
    [SerializeField] ChargingTargetRect targetRect;
    [SerializeField] Slider timer;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI attackCountText;
    // this
    RectTransform baseLine;
    // 시간 값
    public float TimeLimit { get; private set; } = 0.0f;        // 차징 미니게임 한판 시간
    public float ControllTime { get; private set; } = 0.0f;     // 차징 미니게임 진행 시간
    public float CumulatedTime { get; private set; } = 0.0f;    // 차징 성공 누적 시간
    public int AttackCount { get; private set; } = 0;           // 차징으로 쌓은 연타 횟수
    // 플래그
    bool controllOn = false;

    void Awake()
    {
        baseLine = GetComponent<RectTransform>();

        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(OnClickAttackButton);
    }
    void Start()
    {
        EnableAttackButton(false);
        InitChargingControll();
    }
    void Update()
    {
        if (!controllOn)
            return;

        float delta = Time.deltaTime;

        targetRect.HandleUpdate(delta);
        pointer.HandleInput(delta);
        CheckPointerInTarget(delta);
        UpdateChargingTime(delta);
    }
    void InitChargingControll()
    {
        pointer.SetBounds(baseLine);
        targetRect.SetBounds(baseLine);

        TimeLimit = 0.0001f;
        ControllTime = 0.0f;
        CumulatedTime = 0.0f;

        timer.value = 1.0f;

        AttackCount = 0;
        attackCountText.text = $"x{AttackCount}";

        controllOn = false;
    }
    public void EnableAttackButton(bool _enable)
    {
        InitChargingControll();

        attackButton.enabled = _enable;
        attackButton.gameObject.SetActive(_enable);
    }
    void OnClickAttackButton()
    {
        EnableAttackButton(false);
        controllOn = true;
        pointer.FocusOnOff(controllOn);
    }
    void CheckPointerInTarget(float _delta)
    {
        if (targetRect.IsPointerInTarget(pointer.GetPosX, _delta))
            CumulatedTime += _delta;
        else
            CumulatedTime = Mathf.Max(0.0f, CumulatedTime - (_delta * 0.5f));

        targetRect.SetScale();
    }
    void UpdateChargingTime(float _delta)
    {
        ControllTime += _delta;

        float remainTime = Mathf.Max(0.0f, TimeLimit - ControllTime);
        timeText.text = $"{remainTime: 0.0}sec";

        float timeRatio = Mathf.Clamp01(remainTime / TimeLimit);
        timer.value = timeRatio;

        if (remainTime <= 0.0f)
        {
            controllOn = false;
            // todo: damage 계산, 후속 처리 등
            pointer.FocusOnOff(controllOn);
            targetRect.BlinkingOff();
        }
    }
    public void StackUpAttack()
    {
        AttackCount++;
        attackCountText.text = $"x{AttackCount}";
    }
}