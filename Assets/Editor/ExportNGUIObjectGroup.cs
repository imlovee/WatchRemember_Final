using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExportNGUIObjectGroup
{
    [MenuItem("Assets/Export NGUIObject XML.")]
    static void SavePrefabGroup()
    {
        string path = EditorUtility.SaveFolderPanel("Export NGUIObject Group", Application.dataPath, "");

        if (path.Length != 0)
        {

            string filePath = string.Empty;
            Transform[] selection = Selection.transforms;
            if (selection.Length == 0)
            {
                Debug.LogWarning("Please Select Prefab.");
                return;
            }


            for (int i = 0; i < selection.Length; i++)
            {
                GameObject go = selection[i].gameObject;

                XMLObjectGroup grp = new XMLObjectGroup();
                grp.Name = go.name;

                UISprite[] sprites = go.GetComponentsInChildren<UISprite>();
                grp.AddRangeSprites(sprites);

                UILabel[] labels = go.GetComponentsInChildren<UILabel>();
                grp.AddRangeLabels(labels);

                filePath = path + "/" + go.name + ".xml";
                if (ZinSerializerForXML.Serialization<XMLObjectGroup>(grp, filePath))
                {
                    Debug.Log("Export file: " + filePath);
                }
            }

            Selection.objects = selection;
        }

    }

}
