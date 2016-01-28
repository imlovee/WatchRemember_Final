using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddUIWidgets : ZinBehaviour
{
    public UIWidget m_widgetPrefab;
    public Vector2 m_offset;

    protected List<UIWidget> m_labels;
    public List<UIWidget> Labels { get { return m_labels; } }
    //protected Vector2 m_size;

    void Awake()
    {
        m_labels = new List<UIWidget>();
        //m_size = m_labelPrefab.localSize;
    }

    void OnDestroy()
    {
        if (m_labels != null)
        {
            m_labels.Clear();
            m_labels.TrimExcess();
            m_labels = null;
        }
    }

    protected GameObject InstantiateWidget()
    {
        Vector3 pos = Vector3.zero;
        pos.x += m_offset.x;
        pos.y += m_offset.y;
        //pos.y += m_size.y * m_labels.Count + m_offset.y;

        GameObject labelGo = GameObject.Instantiate(m_widgetPrefab.gameObject) as GameObject;
        labelGo.transform.parent = transform;
        labelGo.transform.localPosition = pos;
        labelGo.transform.localScale = Vector3.one;

        return labelGo;
    }
}
