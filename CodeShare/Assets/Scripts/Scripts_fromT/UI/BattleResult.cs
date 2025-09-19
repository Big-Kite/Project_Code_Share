using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResult : Singleton<BattleResult>
{
    [SerializeField] Button continueButton;

    [SerializeField] GameObject victoryBanner;
    [SerializeField] GameObject defeatBanner;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI gainExpText;

    [SerializeField] Transform slotParent;
    [SerializeField] GameObject slotPrefab;

    IEnumerator CoWaitButton()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.gameObject.SetActive(false);

        yield return YieldCache.WaitForSeconds(2.0f);

        continueButton.onClick.AddListener(OnClickContinue);
        continueButton.gameObject.SetActive(true);
        continueButton.enabled = true;
    }
    public void Init(bool _win)
    {
        StartCoroutine(CoWaitButton());

        victoryBanner.SetActive(_win);
        defeatBanner.SetActive(!_win);

        // »πµÊ ∞Ê«Ëƒ°
        // »πµÊ æ∆¿Ã≈€
    }
    public void OnClickContinue()
    {
        if (SceneTransition.Instance.PerformTransition(Define.FieldScene))
        {
            continueButton.enabled = false;
            PopupManager.Instance.CloseBattleResult();
        }
    }
}
