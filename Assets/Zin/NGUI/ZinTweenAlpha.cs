using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenAlpha))]
public class ZinTweenAlpha : ZinTween
{

    void Awake()
    {
        m_teener = AddTween<TweenAlpha>();
    }

    void Start()
    {
        InitTween();
    }

    public override void ShowTween()
    {
        TweenAlpha tweenAlpha = (TweenAlpha)m_teener;

        tweenAlpha.enabled = true;
        tweenAlpha.ResetToBeginning();

        m_isShow = true;
    }

    public void ShowTween(float from, float to)
    {
        TweenAlpha tweenAlpha = (TweenAlpha)m_teener;

        tweenAlpha.from = from;
        tweenAlpha.to = to;

        ShowTween();
    }


    public override void HideTween()
    {
        TweenAlpha tweenAlpha = (TweenAlpha)m_teener;

        tweenAlpha.enabled = true;
        tweenAlpha.ResetToBeginning();

        m_isShow = false;
    }
}
