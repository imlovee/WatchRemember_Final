using UnityEngine;
using System.Collections;

public class ZinGrid : MonoBehaviour
{
    public GameObject item;
    public Texture[] images;

    void Start()
    {
        InitItem();
    }

    void InitItem()
    {
        for (int i = 0; i < images.Length; i++)
        {
            GameObject obj = Instantiate(item, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            obj.transform.parent = this.transform;
            obj.transform.localScale = new Vector3(1f, 1f, 1f);

            UITexture texture = GetChildObj(obj, "Texture").GetComponent<UITexture>();
            UILabel label = GetChildObj(obj, "Label").GetComponent<UILabel>();

            texture.mainTexture = images[i];
            label.text = (i + 1).ToString();
        }

        GetComponent<UIGrid>().Reposition();
    }

    GameObject GetChildObj(GameObject source, string strName)
    {
        Transform[] AllData = source.GetComponentsInChildren<Transform>();
        GameObject target = null;

        foreach (Transform Obj in AllData)
        {
            if (Obj.name == strName)
            {
                target = Obj.gameObject;
                break;
            }
        }

        return target;
    }
}
