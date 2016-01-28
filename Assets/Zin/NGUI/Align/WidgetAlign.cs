using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIWidget))]
public class WidgetAlign : ZinBehaviour
{
    protected UIWidget m_widget;
    protected Vector2 m_size;
    public List<UIWidget> m_targets;

    public Align m_align = Align.NONE;

    void Start()
    {
        m_widget = GetComponent<UIWidget>();
        m_size = m_widget.localSize;

        SetAlignWidgets();
    }

    public void SetAlignWidgets()
    {
        GetWidgets();
        AlignWidgets();
    }

    void GetWidgets()
    {
        if (m_targets == null)
        {
            m_targets = new List<UIWidget>();
        }
        else
        {
            m_targets.Clear();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            UIWidget uiWidget = child.gameObject.GetComponent<UIWidget>();
            if (uiWidget != null)
            {
                if (uiWidget.gameObject.activeSelf && uiWidget.enabled)
                {
                    m_targets.Add(uiWidget);
                }
            }
        }
        //m_targets.Sort();
    }

    void OnDestroy()
    {
        if (m_targets != null)
        {
            m_targets.Clear();
            m_targets.TrimExcess();

            m_targets = null;
        }
    }

    public virtual void AlignWidgets() { }
}
