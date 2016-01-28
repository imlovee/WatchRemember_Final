using UnityEngine;
using System.Collections;

public class ZinUtil
{

    /// <summary>
    /// 해당 이름을 가진 자식 오브젝트 가져오기
    /// </summary>
    /// <param name="source"></param>
    /// <param name="strName"></param>
    /// <returns></returns>
    public static GameObject GetChildObj(GameObject source, string strName)
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
