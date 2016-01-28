using UnityEngine;
using System.Collections;
using System;

public enum ClearLevel
{
    NONE = -1,
    OK = 0,
    GOOD = 1,
    NICE = 2,
    BEST = 3,
    AMAZING = 4,
    PERPEFCT = 5,
}


public class ClearMessage : ZinBehaviour
{
    public UIPanel m_msgImg;
    public UISprite[] m_msgs;
    public ZinTweenScale[] m_tweens;

    private ClearLevel m_clearLv = ClearLevel.NONE;
    public int perfectCount = 0;

    void Awake()
    {
        m_msgImg = GetComponent<UIPanel>();
    }

    void Start()
    {
        InitMessages();
    }

    void InitMessages()
    {
        m_tweens = new ZinTweenScale[m_msgs.Length];
        for (int i = 0; i < m_msgs.Length; i++)
        {
            m_tweens[i] = m_msgs[i].GetComponent<ZinTweenScale>();
            if (m_tweens[i] == null)
            {
                m_tweens[i] = m_msgs[i].gameObject.AddComponent<ZinTweenScale>();
                m_tweens[i].m_showSpeed = 0.2f;
                m_tweens[i].AddKey(new Keyframe(0.7f, 1.1f, 0, 0));
                m_tweens[i].onFinished += OnFinished;
            }
        }
    }

    private void OnFinished()
    {
        Send();
    }

    /// <summary>
    /// 콤보 계산
    /// </summary>
    /// <param name="clearLv"></param>
    private void SetComboCount(ClearLevel clearLv)
    {
        if (clearLv == ClearLevel.AMAZING || clearLv == ClearLevel.PERPEFCT)
        {
            perfectCount++;
        }
        else
        {
            perfectCount = 0;
        }
    }

    public void ShowMessage(ClearLevel clearLv)
    {
        SetComboCount(clearLv);

        m_clearLv = clearLv;

        int index = GetIndex((int)clearLv);
        //int index = (int)m_clearLv >= m_msgs.Length ? m_msgs.Length - 1 : (int)m_clearLv;
        m_tweens[index].ShowTween();
    }

    private int GetIndex(int clearTime)
    {
        return clearTime >= m_msgs.Length ? m_msgs.Length - 1 : clearTime;
    }

    //public IEnumerator ShowMessageAndClose(ClearLevel clearLv)
    //{
    //    ShowMessage(clearLv);
    //    yield return new WaitForSeconds(0.5f);
    //    HideMessage(clearLv);
    //}

    public IEnumerator HideMessage(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        HideMessage();
    }

    public void HideMessage()
    {
        if (m_clearLv == ClearLevel.NONE) return;

        int index = (int)m_clearLv >= m_msgs.Length ? m_msgs.Length - 1 : (int)m_clearLv;

        if (m_tweens[index].m_isShow)
        {
            m_tweens[index].HideTween();
        }
        else
        {
            Debug.LogWarning("HideMessaged");
        }
    }

    public void ShowMessage(int clearTime)
    {
        ClearLevel clearLv = (ClearLevel)clearTime;
        ShowMessage(clearLv);
    }

}
