using UnityEngine;
using System.Collections;

public class ShowPanelOnClick : MonoBehaviour
{
    public InitPopupWindowPanel m_panel;

    void Start()
    {

    }

    void OnClick()
    {
        if (m_panel == null) return;

        m_panel.SetPanel(!m_panel.m_show);

        //if (m_panel.m_show)
        //{
        //    m_panel.ShowPanel(false);
        //}
        //else
        //{
        //    m_panel.ShowPanel(true);
        //}
    }
}
