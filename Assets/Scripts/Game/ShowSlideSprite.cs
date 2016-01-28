using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenPosition))]
[RequireComponent(typeof(TweenAlpha))]
public class ShowSlideSprite : ZinBehaviour
{
    public Vector3 StartPos = Vector3.zero;
    public Vector3 EndPos = Vector3.zero;

    public float duration;
    public float startDelay;

    private TweenPosition ShowTween;
    private TweenAlpha ShowAlpha;

    //public bool isTest = false;

    void Awake()
    {
        ShowTween = GetComponent<TweenPosition>();
        ShowAlpha = GetComponent<TweenAlpha>();

        ShowTween.enabled = false;
        ShowAlpha.enabled = false;

    }


    void Start()
    {
    }

    public void Show()
    {
        transform.localPosition = StartPos;

        ShowTween.duration = duration;
        ShowTween.delay = startDelay;
        ShowTween.from = StartPos;
        ShowTween.to = EndPos;

        ShowAlpha.duration = duration;
        ShowAlpha.delay = startDelay;
        ShowAlpha.from = 0;
        ShowAlpha.to = 1;

        ShowTween.enabled = true;
        ShowTween.ResetToBeginning();

        ShowAlpha.enabled = true;
        ShowAlpha.ResetToBeginning();

        StartCoroutine(OnFinished());
    }

    IEnumerator OnFinished()
    {
        yield return new WaitForSeconds(startDelay + duration);
        Send();
    }

    //void Update()
    //{
    //    if (isTest)
    //    {
    //        Show();
    //        isTest = false;
    //    }
    //}

}
