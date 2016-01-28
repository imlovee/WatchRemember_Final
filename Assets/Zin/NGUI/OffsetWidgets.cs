using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class OffsetWidgets : MonoBehaviour
{
    public UIWidget[] m_widdgets;
    public Vector2 m_offset;

    void Awake()
    {
        
    }

    void Start()
    {
        SetOffset();
    }

    public void SetOffset()
    {
        m_widdgets = GetComponentsInChildren<UIWidget>();

        if (m_widdgets == null) return;

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < m_widdgets.Length; i++)
        {
            UIWidget widget = m_widdgets[i];
            pos = widget.transform.localPosition;
            pos.x += (widget.localSize.x * widget.transform.localScale.x) / 2 * m_offset.x;
            pos.y += (widget.localSize.y * widget.transform.localScale.y)/ 2 * m_offset.y;

            widget.transform.localPosition = pos;
        }
    }
}
