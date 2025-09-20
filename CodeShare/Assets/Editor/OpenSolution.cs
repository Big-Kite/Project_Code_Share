using UnityEditor;
using System.Diagnostics;

public class OpenSolution
{
    [MenuItem("Tools/Open Solution", priority = 0)]
    public static void OpenProjectSolution()
    {
        string projectPath = System.IO.Directory.GetCurrentDirectory(); // ���� ������Ʈ ��� ��������

        string[] solutionFiles = System.IO.Directory.GetFiles(projectPath, "*.sln", System.IO.SearchOption.TopDirectoryOnly);
        if (solutionFiles.Length == 0)
        {
            UnityEngine.Debug.LogError("No solution(.sln) file. found in the project directory.");
            return;
        }
        
        string externalEditorPath = EditorPrefs.GetString("kScriptsDefaultApp"); // External Script Editor ��� ��������
        if (string.IsNullOrEmpty(externalEditorPath))
        {
            UnityEngine.Debug.LogError("External Script Editor is not set in Unity Preferences.");
            return;
        }

        string solutionPath = solutionFiles[0];
        UnityEngine.Debug.Log($"Solution file found: {solutionPath}\nOpening solution with external editor: {externalEditorPath}");
        
        Process.Start(externalEditorPath, $"\"{solutionPath}\""); // ������ External Script Editor�� �ַ�� ����
    }
}