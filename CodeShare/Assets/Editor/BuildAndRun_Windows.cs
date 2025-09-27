using System.Linq;
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
        string originalDefines = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone);
        string modifiedDefines = originalDefines;

        // QA 심볼 제거
        if (modifiedDefines.Contains("QA"))
        {
            var defineList = modifiedDefines.Split(';').ToList();
            defineList.RemoveAll(d => d == "QA");
            modifiedDefines = string.Join(";", defineList);
            PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, modifiedDefines);
            Debug.Log("[Build] QA 심볼 제거");
        }

        try
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

            for (int i = 1; i <= playerCount; i++)
            {
                BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win32/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + "_" + i.ToString() + ".exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
            }
        }
        finally
        {
            PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, originalDefines);
            Debug.Log("[Build] QA 심볼 복원");
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
    static void PerformMacBuild(int playerCount) // 안쓰지만 혹시 모르니 보존.
    {
        // 유니티 빌드세팅 API를 사용 // 빌드 타겟 설정 => 윈도우, 맥, 안드로이드, IOS 중 어떤걸로 설정할 지
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Standalone,
            BuildTarget.StandaloneWindows
        );

        // 실행할 클라이언트(플레이어) 갯수 만큼 반복문 실행
        for (int i = 1; i <= playerCount; i++)
        {
            // 씬의 경로를 추가 // 프로젝트 이름과 플레이어 번호를 사용해서 빌드 경로와 파일 설정
            BuildPipeline.BuildPlayer(GetScenePaths(),
                "Builds/Win64/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".app",
                BuildTarget.StandaloneOSX, BuildOptions.AutoRunPlayer
            );
        }
    }
}