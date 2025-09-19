using UnityEngine;
using UnityEngine.UI;

public class LobbyHome : LobbyBase
{
    [SerializeField] Button playButton;

    void Awake()
    {
        playButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(OnClickPlayButton);
    }
    void OnClickPlayButton()
    {
        StageManager.Instance.SetCurStage(1);

        if (SceneTransition.Instance.PerformTransition(Define.FieldScene))
        {
            playButton.enabled = false;
            StageManager.Instance.EnterStage();
        }
    }
}
