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
    void Start()
    {
        SoundManager.Instance.SetBgmVolume(1.0f);
        SoundManager.Instance.PlayBgm(BGM.BGM_Lobby);
    }
    void OnClickPlayButton()
    {
        StageManager.Instance.SetCurStage(1);

        if (SceneTransition.Instance.PerformTransition(Define.FieldScene))
        {
            SoundManager.Instance.PlaySfx(SFX.ButtonTouch);
            playButton.enabled = false;
            StageManager.Instance.EnterStage();
        }
    }
}
