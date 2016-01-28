using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenRotation))]
public class ZinTweenRotation : ZinTween
{
    void Awake()
    {
        m_teener = AddTween<TweenRotation>();
    }

    void Start()
    {
        InitTween();
    }

    public override void InitTween()
    {
        m_teener.duration = m_showSpeed;
        m_teener.enabled = false;

        m_teener.transform.localEulerAngles = Vector3.zero;
    }

    public override void ShowTween()
    {
        TweenRotation tweenRotation = (TweenRotation)m_teener;

        tweenRotation.enabled = true;
        tweenRotation.ResetToBeginning();

        m_isShow = true;
        SoundFX.Instance.PlaySound(SoundType.SHOW_LABEL);
    }


    public void ShowTween(Vector3 from, Vector3 to)
    {
        TweenRotation tweenRotation = (TweenRotation)m_teener;

        tweenRotation.from = from;
        tweenRotation.to = to;

        ShowTween();
    }


    public override void HideTween()
    {
        TweenRotation tweenScale = (TweenRotation)m_teener;

        tweenScale.enabled = true;
        tweenScale.ResetToBeginning();

        m_isShow = false;
    }

}
