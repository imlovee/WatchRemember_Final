using UnityEngine;
using System.Collections;

public class ZinTween : MonoBehaviour
{
    public float m_showSpeed = 0.2f;
    public bool m_isShow = false;
    protected UITweener m_teener;
    public Vector3 m_startPos;

    protected T AddTween<T>() where T : UITweener
    {
        T tweener = GetComponent<T>();
        if (tweener == null)
        {
            tweener = gameObject.AddComponent<T>();
        }

        return tweener;
    }

    public virtual void InitTween()
    {
        m_teener.duration = m_showSpeed;
        m_teener.enabled = false;

        transform.localPosition = m_startPos;
    }

    public void RunTween(bool isRun)
    {
        if (isRun)
        {
            ShowTween();
        }
        else
        {
            HideTween();
        }
    }

    public virtual void ShowTween() { }
    public virtual void HideTween() { }
}
