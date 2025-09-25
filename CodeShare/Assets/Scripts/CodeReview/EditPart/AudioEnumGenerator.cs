using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AudioEnumGenerator : Editor
{
    private const string enumFilePath = "Assets/Scripts/System/SoundEnum.cs"; // 생성될 Enum 파일 경로

    [MenuItem("Tools/Generate Audio Enum")]
    public static void GenerateAudioEnum()
    {
        // AudioManager 컴포넌트를 가진 오브젝트 찾기
        SoundManager soundManager = FindObjectsByType<SoundManager>(FindObjectsSortMode.None).FirstOrDefault();
        if (soundManager == null || soundManager.bgms == null || soundManager.sfxs == null)
        {
            Debug.LogWarning("SoundManager가 splash에 존재하지 않거나 오디오 클립 배열들이 비어 있습니다.");
            return;
        }

        // BGM 내용 생성
        string eumCode = "public enum BGM\n{\n";
        foreach (AudioClip clip in soundManager.bgms)
        {
            if (clip != null)
            {
                string safeName = clip.name.Replace(" ", "_"); // 이름에 공백이 있으면 _로 대체
                eumCode += $"\t{safeName},\n";
            }
        }
        eumCode += "\tBGM_MAX,\n}";
        // SFX 내용 생성
        eumCode += "\n\npublic enum SFX\n{\n";
        foreach (AudioClip clip in soundManager.sfxs)
        {
            if (clip != null)
            {
                string safeName = clip.name.Replace(" ", "_"); // 이름에 공백이 있으면 _로 대체
                eumCode += $"\t{safeName},\n";
            }
        }
        eumCode += "\tSFX_MAX,\n}";

        // 파일 생성 및 쓰기
        File.WriteAllText(enumFilePath, eumCode);
        AssetDatabase.Refresh(); // Unity 에디터 갱신

        Debug.Log("AudioClipEnum 파일이 성공적으로 생성되었습니다.");
    }
}