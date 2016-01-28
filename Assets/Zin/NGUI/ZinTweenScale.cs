using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenScale))]
public class ZinTweenScale : ZinTween
{

    public delegate void OnFinishedTween();
    public event OnFinishedTween onFinished;

    void Awake()
    {
        m_teener = AddTween<TweenScale>();
    }

    void Start()
    {
        InitTween();
    }


    public override void InitTween()
    {
        m_teener.duration = m_showSpeed;
        m_teener.enabled = false;

        m_teener.transform.localScale = Vector3.zero;
    }

    public override void ShowTween()
    {
        TweenScale tweenScale = (TweenScale)m_teener;

        tweenScale.from = Vector3.zero;
        tweenScale.to = Vector3.one;

        RunTween();
    }

    public void AddKey(Keyframe key)
    {
        TweenScale tweenScale = (TweenScale)m_teener;
        tweenScale.animationCurve.AddKey(key);
    }

    private void RunTween()
    {
        TweenScale tweenScale = (TweenScale)m_teener;

        tweenScale.enabled = true;
        tweenScale.ResetToBeginning();
		gameObject.GetComponentInParent<UIPanel> ().Update ();

        StartCoroutine(EndTween());

        m_isShow = true;
        SoundFX.Instance.PlaySound(SoundType.SHOW_LABEL);
    }

    IEnumerator EndTween()
    {
        yield return new WaitForSeconds(m_teener.delay + m_teener.duration);
        if (onFinished != null)
        {
            onFinished();
        }
    }


    public void ShowTween(Vector3 from, Vector3 to)
    {
        TweenScale tweenScale = (TweenScale)m_teener;

        tweenScale.from = from;
        tweenScale.to = to;

        RunTween();
    }


    public override void HideTween()
    {
        TweenScale tweenScale = (TweenScale)m_teener;

        tweenScale.from = Vector3.one;
        tweenScale.to = Vector3.zero;

        tweenScale.enabled = true;
        tweenScale.ResetToBeginning();

        m_isShow = false;
    }
}
