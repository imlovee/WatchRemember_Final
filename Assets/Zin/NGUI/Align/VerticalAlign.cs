using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 부모 위젯 사이즈를 이용하여 자식 위젯 세로 정렬
/// 백그라운드 이미지 안에 자식 위젯들이 들어가도록 정렬
/// </summary>
public class VerticalAlign : WidgetAlign
{
    public override void AlignWidgets()
    {
        float pieceSize = m_size.y / m_targets.Count;
        float space = 0;
        float halfHeight = m_size.y / 2;

        Vector2 targetSize = Vector2.zero;
        for (int i = 0; i < m_targets.Count; i++)
        {
            UIWidget widget = m_targets[i];
            if (targetSize == Vector2.zero)
            {
                targetSize = widget.localSize;
                space = (m_size.y - targetSize.y * m_targets.Count) / m_targets.Count / 2;          // 앞쪽 여백 계산
            }

            float y = pieceSize * i + targetSize.y / 2 + space - halfHeight;
            SetAlign(widget, y);
        }
    }

    void SetAlign(UIWidget widget, float posY)
    {
        Vector2 pos = widget.transform.localPosition;
        switch (m_align)
        {
            case Align.NONE:
                break;

            case Align.CENTER:
                pos.y = 0;
                break;

            case Align.TOP:
                pos.y = m_size.y / 2;
                break;

            case Align.BOTTOM:
                pos.y = m_size.y / 2 * -1;
                break;

            default:
                break;
        }

        pos.y -= posY;
        widget.transform.localPosition = pos;
    }

}
