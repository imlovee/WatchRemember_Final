using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ExportAssetBundles
{
    public static void ExportResource(BuildTarget target, bool noTrack)
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");

        ExportResource(target, noTrack, path);
    }

    public static void ExportResource(BuildTarget target, bool noTrack, string path)
    {
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (noTrack)
            {
                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CompleteAssets, target);
            }
            else
            {
                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target);
            }
            
            Selection.objects = selection;
        }
    }

    public static void ExportResource2(BuildTarget target, Object[] objs, string folderPath)
    {
        string path = folderPath;
        string fileName = objs[0].name + ".unity3d";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundle(objs[0], objs, path + "/" + fileName, BuildAssetBundleOptions.CompleteAssets, target);
        Debug.Log("Build Complete: " + target);
        Debug.Log(path);
    }



    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies/Window")]
    public static void ExportResourceForWindow()
    {
        ExportResource(BuildTarget.StandaloneWindows, false);
    }

    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies/Android")]
    public static void ExportResourceForAndroid()
    {
        ExportResource(BuildTarget.Android, false);
    }

    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies/IPhone")]
    public static void ExportResourceForIPhone()
    {
        ExportResource(BuildTarget.iOS, false);
    }

    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking/Window")]
    public static void ExportResourceNoTrackForWindow()
    {
        ExportResource(BuildTarget.StandaloneWindows, true);
    }

    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking/Android")]
    public static void ExportResourceNoTrackForAndroid()
    {
        ExportResource(BuildTarget.Android, true);
    }

    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking/IPhone")]
    public static void ExportResourceNoTrackForIPhone()
    {
        ExportResource(BuildTarget.iOS, true);
    }


    //[MenuItem("Assets/Build AssetBundle From Selection Each - No dependency tracking")]
    //public static void ExportResourcesNoTrack()
    //{
    //    string path = EditorUtility.SaveFolderPanel("Save Resource", Application.dataPath, "");
    //    if (path.Length != 0)
    //    {
    //        Object[] objs = Selection.objects;

    //        if (Application.platform == RuntimePlatform.WindowsPlayer)
    //        {
    //            ExportObjects(objs, path, BuildTarget.StandaloneWindows);
    //        }
    //        else if (Application.platform == RuntimePlatform.Android)
    //        {
    //            ExportObjects(objs, path, BuildTarget.Android);
    //        }
    //        else if (Application.platform == RuntimePlatform.OSXEditor)
    //        {
    //            ExportObjects(objs, path, BuildTarget.StandaloneOSXIntel);
    //        }
    //        else if (Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            ExportObjects(objs, path, BuildTarget.iPhone);
    //        }

    //    }
    //}

    public static void ExportObjects(Object[] objs, string path, BuildTarget target)
    {
        Object[] objects = null;
        for (int i = 0; i < objs.Length; i++)
        {
            objects = new Object[1];
            objects[0] = objs[i];

            ExportResource2(target, objects, path);
        }
    }
}
