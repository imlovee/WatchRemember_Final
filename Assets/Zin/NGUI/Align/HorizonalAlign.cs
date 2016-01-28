using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Align
{
    NONE = -1,
    LEFT = 0,
    RIGHT,
    CENTER,
    TOP,
    BOTTOM
}

/// <summary>
/// 부모(백그라운드이미지) 위젯 사이즈를 이용하여 자식 위젯 가로 정렬
/// 백그라운드 이미지 안에 자식 위젯들이 들어가도록 정렬
/// </summary>
public class HorizonalAlign : WidgetAlign
{
    public Align m_parentAlign = Align.NONE;
    public float m_offset;

    public override void AlignWidgets()
    {
        float pieceSize = (m_size.x) / m_targets.Count;
        float space = 0;

        Vector2 targetSize = Vector2.zero;
        for (int i = 0; i < m_targets.Count; i++)
        {
            UIWidget widget = m_targets[i];
            if (targetSize == Vector2.zero)
            {
                targetSize = widget.localSize;
                space = (m_size.x - targetSize.x * m_targets.Count) / m_targets.Count / 2;          // 앞쪽 여백 계산
            }

            float x = pieceSize * i + targetSize.x / 2 + space;

            SetAlign(widget, x);
        }
    }

    void SetAlign(UIWidget widget, float posX)
    {
        Vector2 pos = widget.transform.localPosition;
        switch (m_align)
        {
            case Align.NONE:
                break;

            case Align.LEFT:
                if (m_parentAlign == Align.RIGHT)
                {
                    pos.x = m_size.x * -1;
                }
                else
                {
                    pos.x = m_size.x * -1;
                }
                break;

            case Align.RIGHT:
                if (m_parentAlign == Align.RIGHT)
                {
                    pos.x = 0;
                }
                else
                {
                    pos.x = 0;
                }
                break;

            case Align.CENTER:
                if (m_parentAlign == Align.RIGHT)
                {
                    pos.x = m_size.x / 2 * -1;
                }
                else
                {
                    pos.x = m_size.x / 2 * -1;
                }
                break;

            default:
                break;
        }

        pos.x += posX;
        pos.y += m_offset;
        widget.transform.localPosition = pos;
    }
}
