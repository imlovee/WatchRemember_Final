using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenPosition))]
public class HideSlideSprite : ZinBehaviour
{
    public Vector3 StartPos = Vector3.zero;
    public Vector3 EndPos = Vector3.zero;

    public float duration;
    public float startDelay;

    private TweenPosition hideTween;
    private TweenAlpha hideAlpha;

    //public bool isTest = false;

    void Awake()
    {
        hideTween = GetComponent<TweenPosition>();
        hideAlpha = GetComponent<TweenAlpha>();

        hideTween.enabled = false;
    }

    public void Hide()
    {
        hideTween.from = StartPos;
        hideTween.to = EndPos;
        hideTween.duration = duration;
        hideTween.delay = startDelay;

        hideAlpha.duration = duration;
        hideAlpha.delay = startDelay;
        hideAlpha.from = 1;
        hideAlpha.to = 0;

        hideTween.enabled = true;
        hideTween.ResetToBeginning();

        hideAlpha.enabled = true;
        hideAlpha.ResetToBeginning();

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
    //        Hide();
    //        isTest = false;
    //    }
    //}
}
