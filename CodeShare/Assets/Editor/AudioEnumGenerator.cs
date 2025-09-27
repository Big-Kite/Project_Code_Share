using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AudioEnumGenerator : Editor
{
    private const string enumFilePath = "Assets/Scripts/System/SoundEnum.cs"; // ������ Enum ���� ���

    [MenuItem("Tools/Generate Audio Enum")]
    public static void GenerateAudioEnum()
    {
        // AudioManager ������Ʈ�� ���� ������Ʈ ã��
        SoundManager soundManager = FindObjectsByType<SoundManager>(FindObjectsSortMode.None).FirstOrDefault();
        if (soundManager == null || soundManager.bgms == null || soundManager.sfxs == null)
        {
            Debug.LogWarning("SoundManager�� splash�� �������� �ʰų� ����� Ŭ�� �迭���� ��� �ֽ��ϴ�.");
            return;
        }

        // BGM ���� ����
        string eumCode = "public enum BGM\n{\n";
        foreach (AudioClip clip in soundManager.bgms)
        {
            if (clip != null)
            {
                string safeName = clip.name.Replace(" ", "_"); // �̸��� ������ ������ _�� ��ü
                eumCode += $"\t{safeName},\n";
            }
        }
        eumCode += "\tBGM_MAX,\n}";
        // SFX ���� ����
        eumCode += "\n\npublic enum SFX\n{\n";
        foreach (AudioClip clip in soundManager.sfxs)
        {
            if (clip != null)
            {
                string safeName = clip.name.Replace(" ", "_"); // �̸��� ������ ������ _�� ��ü
                eumCode += $"\t{safeName},\n";
            }
        }
        eumCode += "\tSFX_MAX,\n}";

        // ���� ���� �� ����
        File.WriteAllText(enumFilePath, eumCode);
        AssetDatabase.Refresh(); // Unity ������ ����

        Debug.Log("AudioClipEnum ������ ���������� �����Ǿ����ϴ�.");
    }
}