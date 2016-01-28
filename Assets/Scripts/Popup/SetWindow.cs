using UnityEngine;
using System.Collections;

public class SetWindow : MonoBehaviour
{
    protected GameObject GetChildObj(GameObject source, string strName)
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
