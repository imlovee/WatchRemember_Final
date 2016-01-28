using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenPosition))]
public class ZinTweenPosition : ZinTween
{
    void Awake()
    {
        m_teener = AddTween<TweenPosition>();
    }

    void Start()
    {
        InitTween();
    }

    public override void ShowTween()
    {
        TweenPosition tweenScale = (TweenPosition)m_teener;

        tweenScale.enabled = true;
        tweenScale.ResetToBeginning();

        m_isShow = true;
    }


    public override void HideTween()
    {
        TweenPosition tweenScale = (TweenPosition)m_teener;

        tweenScale.enabled = true;
        tweenScale.ResetToBeginning();

        m_isShow = false;
    }
}
