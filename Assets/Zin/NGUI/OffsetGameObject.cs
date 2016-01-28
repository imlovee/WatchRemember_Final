using UnityEngine;
using System.Collections;

/// <summary>
/// 동일한 사이즈의 위젯이 자식들로 있을 경우 해당 위젯을 이용해 offset 적용
/// </summary>
public class OffsetGameObject : MonoBehaviour
{
    public UIWidget m_widget;
    public Vector2 m_offset;

    void Start()
    {
        SetOffset();
    }

    void SetOffset()
    {
        m_widget = GetComponentInChildren<UIWidget>();
        if (m_widget == null) return;

        Vector3 pos = Vector3.zero;

        pos = transform.localPosition;
        pos.x += Screen.width / 100 * m_offset.y;
        pos.y += Screen.height / 100 * m_offset.y;

        transform.localPosition = pos;
    }
}
