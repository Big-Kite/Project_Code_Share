using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] Button enterButton;

    readonly string[] loadKeyMap = { "ScriptableData/DataRef.prefab", "UI/PopupManager.prefab" };
    bool isInit = false;

    void Awake()
    {
        isInit = false;

        Application.targetFrameRate = 120; // 앱 실행 첫 컨트롤러
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면 잠금 방지
    }
    void Start()
    {
        enterButton.onClick.RemoveAllListeners();
        enterButton.onClick.AddListener(OnClickEnterButton);

        LoginProcessor.Instance.LogInTry();
        LoadDataRef();

        PlayerData.Instance.Init();
    }
    public void OnClickEnterButton()
    {
        if (!isInit || !LoginProcessor.Instance.DidLogIn())
            return;

        // todo 로비씬이 만들어지면 씬 로드할때 해줄거임
        if (SceneTransition.Instance.PerformTransition(Define.LobbyScene))
            enterButton.enabled = false;
    }
    void LoadDataRef()
    {
        int objectsCount = loadKeyMap.Length, count = 0;
        List<AsyncOperationHandle<GameObject>> loadHandles = new List<AsyncOperationHandle<GameObject>>();

        foreach (string objectKey in loadKeyMap)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(objectKey);
            loadHandles.Add(handle);

            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    Instantiate(operation.Result, Vector3.zero, Quaternion.identity);
                    count++;

                    if (count >= objectsCount)
                        isInit = true;
                }
            };
        }
    }
}
