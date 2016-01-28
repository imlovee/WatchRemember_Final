using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour
{
    public bool isShow = true;
    public UIPanel[] m_panels;

    void Awake()
    {
        m_panels = GameObject.FindObjectsOfType<UIPanel>();

        //m_panels = GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < m_panels.Length; i++)
        {
            m_panels[i].gameObject.SetActive(isShow);
        }
    }
}
