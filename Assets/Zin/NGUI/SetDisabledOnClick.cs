using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
[RequireComponent(typeof(UISprite))]
public class SetDisabledOnClick : MonoBehaviour
{
    private UIButton m_button;
    public GameObject c_notify;
    public string c_functionName;
    public string c_onSpriteName;
    public string c_offSpriteName;

    public bool m_On = true;

    void Awake()
    {
    }

    void OnClick()
    {
        m_On = !m_On;
        Init(m_On);

        if (c_notify == null) return;
        c_notify.SendMessage(c_functionName, m_On);
    }

    void SetStatus(bool isOn)
    {
        m_On = isOn;
        Init(m_On);
    }

    void Init(bool isOn)
    {
        if (m_button == null)
        {
            m_button = GetComponent<UIButton>();
        }

        if (isOn)
        {
            if (!string.IsNullOrEmpty(c_onSpriteName))
            {
                m_button.normalSprite = c_onSpriteName;
                m_button.hoverSprite = c_onSpriteName;
                m_button.disabledSprite = c_onSpriteName;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(c_offSpriteName))
            {
                m_button.normalSprite = c_offSpriteName;
                m_button.hoverSprite = c_offSpriteName;
                m_button.disabledSprite = c_offSpriteName;
            }
        }
    }
}
