using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [Header("Disposable Popup")]
    [SerializeField] GameObject[] popupPrefab; // �Ϲ� �˾� ������

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
        // ����ġ ���� ����
        sceneLoadingPopup.SetActive(false);
        loadingPopup.SetActive(false);
        // ��ȸ�� Ŭ����
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
    void PopupSort() // ���߿� ��ȸ�� �˾��� �� ����Ʈ�Ű������� �޾Ƽ� ���� �ƴϸ� �� ���� ����ְ� �̾ �����ϰ�
    {
        loadingPopup.transform.SetAsLastSibling();
        sceneLoadingPopup.transform.SetAsLastSibling();
    }
}
