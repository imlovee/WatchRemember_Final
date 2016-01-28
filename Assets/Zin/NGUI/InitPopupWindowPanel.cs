using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class InitPopupWindowPanel : MonoBehaviour
{
    public UIPanel m_panel;
    public bool m_show = false;

    public Vector3 m_defaultLocalSize;

    // Use this for initialization
    void Awake()
    {
        m_panel = GetComponent<UIPanel>();

        ShowPanel();
    }

    private void ShowPanel()
    {
        if (m_show)
        {
            transform.localScale = m_defaultLocalSize;

        }
        else
        {
            if (transform.localScale != Vector3.zero)
            {
                m_defaultLocalSize = transform.localScale;
            }
            transform.localScale = Vector3.zero;
        }
    }

    public void SetPanel(bool isShow)
    {
        m_show = isShow;

        ShowPanel();
    }
}
