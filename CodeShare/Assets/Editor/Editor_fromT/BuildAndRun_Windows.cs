using UnityEditor;
using UnityEngine;

public class BuildAndRun_Windows : EditorWindow
{
    public static BuildAndRun_Windows Instance;

    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        Instance = null;
    }
    [MenuItem("Tools/Run Multiplayer/Win/1 Players")]
    static void PerformWin64Build1()
    {
        PerformWin64Build(1);
    }
    [MenuItem("Tools/Run Multiplayer/Win/2 Players")]
    static void PerformWin64Build2()
    {
        PerformWin64Build(2);
    }
    [MenuItem("Tools/Run Multiplayer/Win/3 Players")]
    static void PerformWin64Build3()
    {
        PerformWin64Build(3);
    }
    [MenuItem("Tools/Run Multiplayer/Win/4 Players")]
    static void PerformWin64Build4()
    {
        PerformWin64Build(4);
    }
    static void PerformWin64Build(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for (int i = 1; i <= playerCount; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win32/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
        }
    }
    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }
    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
    static void PerformMacBuild(int playerCount) // �Ⱦ����� Ȥ�� �𸣴� ����.
    {
        // ����Ƽ ���弼�� API�� ��� // ���� Ÿ�� ���� => ������, ��, �ȵ���̵�, IOS �� ��ɷ� ������ ��
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Standalone,
            BuildTarget.StandaloneWindows
        );

        // ������ Ŭ���̾�Ʈ(�÷��̾�) ���� ��ŭ �ݺ��� ����
        for (int i = 1; i <= playerCount; i++)
        {
            // ���� ��θ� �߰� // ������Ʈ �̸��� �÷��̾� ��ȣ�� ����ؼ� ���� ��ο� ���� ����
            BuildPipeline.BuildPlayer(GetScenePaths(),
                "Builds/Win64/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".app",
                BuildTarget.StandaloneOSX, BuildOptions.AutoRunPlayer
            );
        }
    }
}