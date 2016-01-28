using UnityEngine;
using System.Collections;

public class ZinTools
{
    /// <summary>
    /// 게임 오브젝트 배열삭제
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    public static void DestroyGameObjects<T>(T[] objs) where T : MonoBehaviour
    {
        if (objs == null || objs.Length == 0) return;

        for (int i = objs.Length - 1; i >= 0; i--)
        {
            GameObject.Destroy(objs[i].gameObject);
        }
    }

    public static void SendMessage(GameObject go, string functionName, object obj = null)
    {
        if (go == null) return;
        if (string.IsNullOrEmpty(functionName)) return;

        if (obj != null)
        {
            go.SendMessage(functionName, obj);
        }
        else
        {
            go.SendMessage(functionName);
        }
    }
}
