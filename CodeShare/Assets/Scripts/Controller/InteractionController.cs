using UnityEngine;
using UnityEngine.UI;

public class InteractionController : Singleton<InteractionController>
{
    [SerializeField] Button interactionButton;

    void Awake()
    {
        interactionButton.onClick.RemoveAllListeners();
        interactionButton.onClick.AddListener(OnClickBattle);
    }
    void Start()
    {
        interactionButton.gameObject.SetActive(false);
    }
    public void Show()
    {
        interactionButton.gameObject.SetActive(true);
    }
    public void Hide()
    {
        interactionButton.gameObject.SetActive(false);
    }
    public void OnClickBattle()
    {
        if (SceneTransition.Instance.PerformTransition(Define.BattleScene))
        {
            SoundManager.Instance.PlaySfx(SFX.ButtonTouch);
            interactionButton.enabled = false;
            StageManager.Instance.HoldOnTarget();
            StageManager.Instance.SavePlayerPos();
        }
    }
}
