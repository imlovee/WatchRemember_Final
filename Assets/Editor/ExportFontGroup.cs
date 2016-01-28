using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ExportFontGroup  {
    [MenuItem("Assets/Export FontGroup XML.")]
    static void SavePrefabGroup()
    {
        string path = EditorUtility.SaveFolderPanel("Export Font Group", Application.dataPath, "");

        if (path.Length != 0)
        {

            string filePath = string.Empty;
            Transform[] selection = Selection.transforms;
            if (selection.Length == 0)
            {
                Debug.LogWarning("Please Select Prefab.");
                return;
            }

            XMLFontGroup grp = new XMLFontGroup();
            List<string> fontList = new List<string>();

            for (int i = 0; i < selection.Length; i++)
            {
                GameObject go = selection[i].gameObject;

                UIFont font = go.GetComponent<UIFont>();
                fontList.Add(font.name);
            }

            grp.FontNames = fontList.ToArray();

            filePath = path + "/Font.xml";
            if (ZinSerializerForXML.Serialization<XMLFontGroup>(grp, filePath))
            {
                Debug.Log("Export file: " + filePath);
            }

            Selection.objects = selection;
        }

    }
}
