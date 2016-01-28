using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetLabelsOnClick : MonoBehaviour
{
    public SetLabelOnClick[] m_setLabelOnClicks;

    public string c_search_tag;                      // 찾을 이름
    public UILabel c_reference_label;

    void Start()
    {

    }

    public void GetScripts()
    {
        if (m_setLabelOnClicks == null || m_setLabelOnClicks.Length == 0)
        {
            List<SetLabelOnClick> list = new List<SetLabelOnClick>();

            m_setLabelOnClicks = GetComponentsInChildren<SetLabelOnClick>();
            for (int i = 0; i < m_setLabelOnClicks.Length; i++)
            {
                if (m_setLabelOnClicks[i].c_tag.Contains(c_search_tag))
                {
                    list.Add(m_setLabelOnClicks[i]);
                }
            }

            m_setLabelOnClicks = list.ToArray();
        }
    }

    public void SetReference(string[] texts)
    {
        if (m_setLabelOnClicks == null || m_setLabelOnClicks.Length == 0)
        {
            GetScripts();
        }

        for (int i = 0; i < m_setLabelOnClicks.Length; i++)
        {
            m_setLabelOnClicks[i].c_label = c_reference_label;
            if (i < texts.Length)
            {
                m_setLabelOnClicks[i].c_text = texts[i];
            }
            
        }
    }
}
