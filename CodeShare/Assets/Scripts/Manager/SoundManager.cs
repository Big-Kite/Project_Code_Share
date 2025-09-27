using System;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    const string prefsBgmVolume = "BgmVolume";
    const string prefsSfxVolume = "SfxVolume";
    private const float MaxVolume = 1.0f;

    [Header("VOLUME")]
    [Range(0.0f, 1.0f)] public float bgmVolume;
    [Range(0.0f, 1.0f)] public float sfxVolume;

    [Header("BGM")]
    public AudioClip[] bgms;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxs;
    AudioSource[] sfxPlayers;
    int sfxChannels;
    int sfxIndex;

    // 마지막으로 재생한 SFX와 프레임을 추적
    private SFX lastPlayedSfx = SFX.SFX_MAX; // 초기값을 MAX로 설정하여 유효한 SFX가 아님을 표시
    private int lastPlayFrame = -1; // 마지막으로 SFX를 재생한 프레임
    private BGM lastPlayedBgm = BGM.BGM_MAX;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지되도록 설정
        Init();
    }

    void Init()
    {
        bgmVolume = PlayerPrefs.GetFloat(prefsBgmVolume, MaxVolume);
        sfxVolume = PlayerPrefs.GetFloat(prefsSfxVolume, MaxVolume);

        sfxChannels = sfxs.Length;

        // 배경음 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource();
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        // 효과음 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];
        for (int index = 0; index < sfxChannels; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].loop = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(BGM _bgm)
    {
        if (bgms.Length <= 0)
            return;

        if (lastPlayedBgm == _bgm)
            return;

        StopBgm();

        bgmPlayer.clip = bgms[(int)_bgm];
        bgmPlayer.Play();

        lastPlayedBgm = _bgm;
    }

    public void PlayStageBgm()
    {
        //int stageIndex = Utils.ExtractNumber(GameModeManager.Instance.Stage.Map);
        //switch (stageIndex)
        //{
        //    case 1:
        //        PlayBgm(BGM.BGM_Stage1);
        //        break;
        //    case 2:
        //        PlayBgm(BGM.BGM_Stage2);
        //        break;
        //    case 3:
        //        PlayBgm(BGM.BGM_Stage3);
        //        break;
        //    case 4:
        //        PlayBgm(BGM.BGM_Stage4);
        //        break;
        //}
    }

    public void PlayStopedBgm()
    {
        if (bgms.Length <= 0)
            return;

        StopBgm();

        bgmPlayer.clip = bgms[(int)lastPlayedBgm];
        bgmPlayer.Play();
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    public void PlaySfx(SFX _sfx)
    {
        if (sfxs.Length <= 0)
            return;

        // 현재 프레임에서 이미 같은 SFX가 재생된 경우 재생하지 않음
        if (lastPlayedSfx == _sfx && lastPlayFrame == Time.frameCount)
            return;

        for (int index = 0; index < sfxChannels; index++)
        {
            int curIndex = (index + sfxIndex) % sfxChannels;
            if (sfxPlayers[curIndex].isPlaying)
                continue;

            sfxPlayers[curIndex].clip = sfxs[(int)_sfx];
            sfxPlayers[curIndex].Play();

            // 마지막으로 재생한 SFX와 프레임 저장
            lastPlayedSfx = _sfx;
            lastPlayFrame = Time.frameCount;
            sfxIndex = curIndex;
            break;
        }
    }

    public void SetBgmVolume(float value)
    {
        bgmVolume = value;
        bgmPlayer.volume = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        for (int index = 0; index < sfxChannels; index++)
        {
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(prefsBgmVolume, bgmVolume);
        PlayerPrefs.SetFloat(prefsSfxVolume, sfxVolume);
    }
}