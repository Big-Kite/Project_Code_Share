using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingController : Singleton<ChargingController>
{
    [Header("UI Elements")] // �ν����� ����
    [SerializeField] Button attackButton;
    [SerializeField] ChargingPointer pointer;
    [SerializeField] ChargingTargetRect targetRect;
    [SerializeField] Slider timer;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI attackCountText;
    // this
    RectTransform baseLine;
    // �ð� ��
    public float TimeLimit { get; private set; } = 0.0f;        // ��¡ �̴ϰ��� ���� �ð�
    public float ControllTime { get; private set; } = 0.0f;     // ��¡ �̴ϰ��� ���� �ð�
    public float CumulatedTime { get; private set; } = 0.0f;    // ��¡ ���� ���� �ð�
    public int AttackCount { get; private set; } = 0;           // ��¡���� ���� ��Ÿ Ƚ��
    // �÷���
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
            // todo: damage ���, �ļ� ó�� ��
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