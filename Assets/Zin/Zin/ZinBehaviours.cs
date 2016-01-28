using UnityEngine;
using System.Collections;


/// <summary>
/// 메세지가 복수일때 에디터를 통해서 배열 개수가 동일하도록 조절해야함.
/// </summary>
public class ZinBehaviours : MonoBehaviour
{
    public GameObject[] m_notify;
    public string[] m_functionName;

    private void Send(GameObject go, string functionName, object obj = null)
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

    protected void Send(params object[] obj)
    {
        if (obj.Length != m_notify.Length || obj.Length != m_functionName.Length)
        {
            Debug.LogError("obj.Length != m_notify.Length || obj.Length != m_functionName.Length");
            return;
        }

        for (int i = 0; i < m_notify.Length; i++)
        {
            Send(m_notify[i], m_functionName[i], obj[i]);
        }
    }

    protected void Send()
    {
        for (int i = 0; i < m_notify.Length; i++)
        {
            Send(m_notify[i], m_functionName[i], null);
        }
    }

    protected void Send(object obj)
    {
        for (int i = 0; i < m_notify.Length; i++)
        {
            Send(m_notify[i], m_functionName[i], obj);
        }
    }
}
