using UnityEditor;
using System.Diagnostics;

public class OpenSolution
{
    [MenuItem("Tools/Open Solution", priority = 0)]
    public static void OpenProjectSolution()
    {
        string projectPath = System.IO.Directory.GetCurrentDirectory(); // 현재 프로젝트 경로 가져오기

        string[] solutionFiles = System.IO.Directory.GetFiles(projectPath, "*.sln", System.IO.SearchOption.TopDirectoryOnly);
        if (solutionFiles.Length == 0)
        {
            UnityEngine.Debug.LogError("No solution(.sln) file. found in the project directory.");
            return;
        }
        
        string externalEditorPath = EditorPrefs.GetString("kScriptsDefaultApp"); // External Script Editor 경로 가져오기
        if (string.IsNullOrEmpty(externalEditorPath))
        {
            UnityEngine.Debug.LogError("External Script Editor is not set in Unity Preferences.");
            return;
        }

        string solutionPath = solutionFiles[0];
        UnityEngine.Debug.Log($"Solution file found: {solutionPath}\nOpening solution with external editor: {externalEditorPath}");
        
        Process.Start(externalEditorPath, $"\"{solutionPath}\""); // 설정된 External Script Editor로 솔루션 열기
    }
}