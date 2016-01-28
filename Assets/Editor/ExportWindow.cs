using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ExportWindow : EditorWindow
{
    string fileName = "Name";
    static string folderPath;
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    int noTrackIndex;
    int platformIndex;

    string[] noTrackNames = { "Track dependencies", "No dependency tracking" };
    string[] platforms = { "Mac", "Window", "Android", "IOS" };

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Export Assetbundle Window")]
    static void Init()
    {
        folderPath = Application.dataPath;

        // Get existing open window or if none, make a new one:
        ExportWindow window = (ExportWindow)EditorWindow.GetWindow(typeof(ExportWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Dependencies Setting");
        noTrackIndex = GUILayout.SelectionGrid(noTrackIndex, noTrackNames, noTrackNames.Length, "toggle");
        GUILayout.Space(10);

        GUILayout.Label("Platform Setting");
        platformIndex = GUILayout.SelectionGrid(platformIndex, platforms, platforms.Length, "toggle");
        GUILayout.Space(10);

        GUILayout.Label("File Setting");
        GUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Package Folder", folderPath);
        if (GUILayout.Button("Open"))
        {
            folderPath = EditorUtility.SaveFolderPanel("Select Folder", folderPath, "") + "/" + platforms[platformIndex];
        }
        GUILayout.EndHorizontal();
        

        fileName = EditorGUILayout.TextField("File Name", fileName);
        if (GUILayout.Button("Export"))
        {
            if (noTrackIndex == 0)
            {
                ExistDirectory(folderPath);
                ExportTrackDefend(folderPath + "/" + fileName + ".unity3d");
            }
            else
            {
                ExistDirectory(folderPath);
                ExportNoDefend(folderPath + "/" + fileName + ".unity3d");
            }
        }
    }

    /// <summary>
    /// 폴더 확인(없으면 생성)
    /// </summary>
    /// <param name="folderPath"></param>
    void ExistDirectory(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    void ExportTrackDefend(string filePath)
    {
        if (platformIndex == 0)
        {
			ExportAssetBundles.ExportResource(BuildTarget.StandaloneOSXIntel, false, filePath);
        }
		else if (platformIndex == 1)
		{
			ExportAssetBundles.ExportResource(BuildTarget.StandaloneWindows, false, filePath);
		}
        else if (platformIndex == 2)
        {
            ExportAssetBundles.ExportResource(BuildTarget.Android, false, filePath);
        }
        else if (platformIndex == 3)
        {
            ExportAssetBundles.ExportResource(BuildTarget.iOS, false, filePath);
        }
    }

    void ExportNoDefend(string filePath)
    {
        if (platformIndex == 0)
        {
			ExportAssetBundles.ExportResource(BuildTarget.StandaloneOSXIntel, true, filePath);
        }
        else if (platformIndex == 1)
        {
			ExportAssetBundles.ExportResource(BuildTarget.StandaloneWindows, true, filePath);
		} 
		else if (platformIndex == 2)
		{
			ExportAssetBundles.ExportResource(BuildTarget.Android, true, filePath);
		}
        else if (platformIndex == 3)
        {
            ExportAssetBundles.ExportResource(BuildTarget.iOS, true, filePath);
        }
    }
}
