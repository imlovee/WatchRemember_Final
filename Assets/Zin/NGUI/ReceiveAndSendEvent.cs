using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// 받는 이벤트만 처리할 시에는 NGUI에서 바로 전달하면 됨.
/// 주는 오브젝트의 정보를 가지고 처리할 필요가 있을 때 이스크립트를 중간에서 사용
/// </summary>
public class ReceiveAndSendEvent : ZinBehaviour
{
    public ZinEventType m_ZinEventType = ZinEventType.None;

    void Awake()
    {
        switch (m_ZinEventType)
        {
            case ZinEventType.None:
                break;

            case ZinEventType.OnSubmit:
                UIEventListener.Get(gameObject).onSubmit += Receive;
                break;

            case ZinEventType.OnClick:
                UIEventListener.Get(gameObject).onClick += Receive;
                break;

            case ZinEventType.OnDoubleClick:
                UIEventListener.Get(gameObject).onDoubleClick += Receive;
                break;

            case ZinEventType.OnHover:
                UIEventListener.Get(gameObject).onHover += ReceiveState;
                break;

            case ZinEventType.OnPress:
                UIEventListener.Get(gameObject).onPress += ReceiveState;
                break;

            case ZinEventType.OnSelect:
                UIEventListener.Get(gameObject).onSelect += ReceiveSelect;
                break;

            case ZinEventType.OnScroll:
                UIEventListener.Get(gameObject).onScroll += ReceivePosition;
                break;

            case ZinEventType.OnDragStart:
                UIEventListener.Get(gameObject).onDragStart += Receive;
                break;

            case ZinEventType.OnDrag:
                UIEventListener.Get(gameObject).onDrag += ReceiveViewPosition;
                break;

            case ZinEventType.OnDragOver:
                UIEventListener.Get(gameObject).onDragOver += Receive;
                break;

            case ZinEventType.OnDragOut:
                UIEventListener.Get(gameObject).onDragOut += Receive;
                break;

            case ZinEventType.OnDragEnd:
                UIEventListener.Get(gameObject).onDragEnd += Receive;
                break;

            case ZinEventType.OnDrop:
                UIEventListener.Get(gameObject).onDrop += ReceiveDrop;
                break;

            case ZinEventType.OnKey:
                UIEventListener.Get(gameObject).onKey += ReceiveKey;
                break;
            default:
                break;
        }
    }

    private void ReceiveViewPosition(GameObject go, Vector2 delta)
    {
        Send(go);
        Send(delta);
    }

    private void ReceivePosition(GameObject go, float delta)
    {
        Send(go);
        Send(delta);
    }

    private void ReceiveState(GameObject go, bool state)
    {
        if (state)
        {
            Send(go);
        }
    }

    private void ReceiveSelect(GameObject go, bool state)
    {
        if (!state)
        {
            Send(go);
        }
    }


    private void ReceiveKey(GameObject go, KeyCode key)
    {
        Send(go);
        Send(key);
    }

    private void ReceiveDrop(GameObject go, GameObject obj)
    {
        Send(go);
        Send(obj);
    }

    private void Receive(GameObject go)
    {
        Send(go);
    }

}
