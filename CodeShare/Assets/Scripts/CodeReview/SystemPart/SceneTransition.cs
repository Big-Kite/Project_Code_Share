using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : Singleton<SceneTransition>
{
    // ������Ʈ ��Ȳ�� �°� �ε��� ���̽�ũ�ڵ��� �����ְ� ����� ������ �ֽ��ϴ�.
    // �ε����� �ڵ�� ���޻����� �ڵ��� �����ư��� ��巹���� �ε�� ���� �ε�/������ ó���մϴ�.
    // �ε��߿��� �ܺ� ȣ���� ���� ��ɰ� �ش� Ŭ�������� ���带 ����ִ� ���ҵ� �մϴ�.

    AsyncOperationHandle<SceneInstance> handleA;
    AsyncOperationHandle<SceneInstance> handleB;

    string targetScene = string.Empty;
    float duration = 1.0f;
    Color color = Color.black;
    LoadSceneMode loadMode = LoadSceneMode.Single;

    public bool Inaccessible { get; private set; } = false;
    bool sceneChanging = false;

    void Awake()
    {
        DontDestroyOnLoad(this);

        sceneChanging = false;
        Inaccessible = false;

        targetScene = string.Empty;
        duration = 2.0f;
        color = Color.black;
        loadMode = LoadSceneMode.Single;
    }
    public bool PerformTransition(string _sceneName)
    {
        if (Inaccessible)
            return false;

        var curScene = SceneManager.GetActiveScene();
        if (curScene.name == _sceneName)
            return false;

        targetScene = _sceneName;
        StartCoroutine(CoSceneTransition());
        return true;
    }
    IEnumerator CoSceneTransition()
    {
        Inaccessible = true;

        GameObject blackCanvas = new GameObject("TransitionFade");
        var canvas = blackCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        DontDestroyOnLoad(blackCanvas.gameObject);

        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, color);
        bgTex.Apply();

        var rect = new Rect(0.0f, 0.0f, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1.0f);

        GameObject blackOverlay = new GameObject();
        var image = blackOverlay.AddComponent<Image>();
        image.material.mainTexture = bgTex;
        image.sprite = sprite;
        image.canvasRenderer.SetAlpha(0.0f);

        blackOverlay.transform.localScale = new Vector3(1, 1, 1);
        blackOverlay.GetComponent<RectTransform>().sizeDelta = blackCanvas.GetComponent<RectTransform>().sizeDelta;
        blackOverlay.transform.SetParent(blackCanvas.transform, false);
        blackOverlay.transform.SetAsFirstSibling();

        float time = 0.0f, halfDuration = duration * 0.5f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }
        image.canvasRenderer.SetAlpha(1.0f);

        SceneLoadStart(targetScene, loadMode);
        yield return new WaitUntil(() => sceneChanging == false);

        time = 0.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }
        image.canvasRenderer.SetAlpha(0.0f);

        Destroy(blackCanvas);
        Inaccessible = false;
    }
    void SceneLoadStart(string _sceneName, LoadSceneMode _loadMode)
    {
        sceneChanging = true;

        var handle = handleA;
        string scenePath = $"Scene/{_sceneName}.unity"; // �ε��� �� Ǯ����

        if (handleA.IsValid()) // a�� �������̸� a ���� b ����
        {
            Addressables.UnloadSceneAsync(handleA, true);
            handle = handleB;
        }
        else if (handleB.IsValid()) // b�� �������̸� b ���� a ����
        {
            Addressables.UnloadSceneAsync(handleB, true);
        }

        StartCoroutine(CoSceneLoad(handle, scenePath));
    }
    IEnumerator CoSceneLoad(AsyncOperationHandle<SceneInstance> _handle, string _scenePath)
    {
        PopupManager.Instance.CloseAllPopups();
        var loadingPopup = PopupManager.Instance.OpenSceneLoading();

        _handle = new AsyncOperationHandle<SceneInstance>();
        _handle = Addressables.LoadSceneAsync(_scenePath, LoadSceneMode.Single, false);

        float per = 0.0f;
        while (true)
        {
            if (_handle.Status == AsyncOperationStatus.Succeeded && _handle.PercentComplete >= 0.9f)
                break;

            per = _handle.PercentComplete;
            loadingPopup.SetPercent(per);
            yield return null;
        }
        yield return null;
        yield return _handle.Result.ActivateAsync();

        float du = 2.0f;
        float timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > du)
                break;

            float clamp = Mathf.Clamp01(timer / du);
            float testper = Mathf.Lerp(per, 1.0f, clamp);
            loadingPopup.SetPercent(testper);
            yield return null;
        }
        yield return null;

        Scene loadedScene = _handle.Result.Scene;
        SceneManager.SetActiveScene(loadedScene);

        PopupManager.Instance.CloseSceneLoading();
        sceneChanging = false;
    }
}
