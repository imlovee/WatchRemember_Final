using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 다음버전엔 배열로 만들어야할듯. unity editor까지 덮으면 금상첨화
/// </summary>
public class ZinBehaviour : MonoBehaviour
{
    public GameObject m_notify;
    public string m_functionName;

    protected void Send(object obj = null)
    {
        if (m_notify == null) return;
        if (string.IsNullOrEmpty(m_functionName)) return;

        if (obj != null)
        {
            m_notify.SendMessage(m_functionName, obj);
        }
        else
        {
            m_notify.SendMessage(m_functionName);
        }
    }

    protected IEnumerator DelaySend(float delay, object obj = null)
    {
        yield return new WaitForSeconds(delay);
        Send(obj);
    }

    public void SetNotify(GameObject go, string functionName)
    {
        m_notify = go;
        m_functionName = functionName;
    }
}
