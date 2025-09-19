using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [Header("Disposable Popup")]
    [SerializeField] GameObject[] popupPrefab; // 일반 팝업 프리팹

    [Header("Attach Popup")]
    [SerializeField] GameObject sceneLoadingPopup;
    [SerializeField] GameObject loadingPopup;
    [SerializeField] GameObject battleResultPopup;

    Stack<GameObject> popupStack = new Stack<GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public GameObject OpenPopup(PopupType _type)
    {
        GameObject newPopup = Instantiate(popupPrefab[(int)_type], transform);
        newPopup.SetActive(true);

        Popup popupFunc = newPopup.GetComponent<Popup>();
        popupFunc ??= newPopup.AddComponent<Popup>();
        popupFunc.Open();

        popupStack.Push(newPopup);
        return newPopup;
    }
    public void ClosePopup()
    {
        if (popupStack.Count <= 0)
            return;

        GameObject topPopup = popupStack.Pop();
        if (topPopup == null)
            return;

        topPopup.GetComponent<Popup>()?.Close();
    }
    public void CloseAllPopups()
    {
        // 어태치 먼저 끄고
        sceneLoadingPopup.SetActive(false);
        loadingPopup.SetActive(false);
        // 일회성 클로즈
        while (popupStack.Count > 0)
        {
            GameObject topPopup = popupStack.Pop();

            if (topPopup != null)
                topPopup.GetComponent<Popup>()?.Close();
        }
        popupStack.Clear();
    }
    public void OpenBattleResult()
    {
        battleResultPopup.SetActive(true);

        Popup popupFunc = battleResultPopup.GetComponent<Popup>();
        popupFunc ??= battleResultPopup.AddComponent<Popup>();

        popupFunc.Open();
    }
    public void CloseBattleResult()
    {
        Popup popupFunc = battleResultPopup.GetComponent<Popup>();
        popupFunc ??= battleResultPopup.AddComponent<Popup>();

        popupFunc.Close(true);
    }
    public void OpenLoading()
    {
        loadingPopup.SetActive(true);
        PopupSort();

        Popup popupFunc = loadingPopup.GetComponent<Popup>();
        popupFunc ??= loadingPopup.AddComponent<Popup>();

        popupFunc.Open();
    }
    public void CloseLoading()
    {
        Popup popupFunc = loadingPopup.GetComponent<Popup>();
        popupFunc ??= loadingPopup.AddComponent<Popup>();

        popupFunc.Close(true);
    }
    public SceneLoading OpenSceneLoading()
    {
        sceneLoadingPopup.SetActive(true);
        PopupSort();

        return sceneLoadingPopup.GetComponent<SceneLoading>();
    }
    public void CloseSceneLoading()
    {
        sceneLoadingPopup.SetActive(false);
    }
    void PopupSort() // 나중엔 일회성 팝업을 널 디폴트매개변수로 받아서 널이 아니면 맨 위에 깔아주고 이어서 진행하게
    {
        loadingPopup.transform.SetAsLastSibling();
        sceneLoadingPopup.transform.SetAsLastSibling();
    }
}
