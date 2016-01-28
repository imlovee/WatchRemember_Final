using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum WindowType
{
    POPUP = 0,              // 일반 메세지용 팝업(전체 컨트롤용)
    GAME_WINDOW             // 게임중에 사용되는 윈도우(게임로직과 연동되는 윈도우, 개별 콘트롤용)
}

[RequireComponent(typeof(UIPanel))]

public class ZinWindow : MonoBehaviour
{
    public static ZinWindow OpendWindow;            // 열려있는 현재 윈도우

    public WindowType windowType = WindowType.POPUP;
    public GameObject m_notify;
    public string m_functionName;
    private float m_showSpeed = 0.2f;
    private TweenScale m_tweenScale;
    private TweenAlpha m_tweenAlpha;

    public ZinWindow PrevWindow;                 // 이전 윈도우

    public bool m_isShow = false;
    public bool m_onSound = false;

    /// <summary>
    /// 윈도우 배경 사용유무(true면 뒷부분도 클릭 가능)
    /// </summary>
    public bool m_useBackground = false;

    private string openedWindowName = "OpenedWindow";
    private string closedWindowName = "ClosedWindow";

    /// <summary>
    /// 윈도우 show/hide 상태
    /// </summary>
    /// <param name="isShow">윈도우 상태(show/hide></param>
    /// <param name="isStart">애니메이션 시작/끝</param>
    public delegate void OnWindowShowState(bool isShow, bool isStart);
    public event OnWindowShowState onShowWindow;

    void Awake()
    {
        m_tweenScale = GetComponent<TweenScale>();
        if (m_tweenScale == null)
        {
            m_tweenScale = gameObject.AddComponent<TweenScale>();
        }

        m_tweenAlpha = GetComponent<TweenAlpha>();
        if (m_tweenAlpha == null)
        {
            m_tweenAlpha = gameObject.AddComponent<TweenAlpha>();
        }

        transform.localPosition = Vector3.zero;
        InitBackGround();
    }

    /// <summary>
    /// 백그라운드 사용설정
    /// </summary>
    /// <param name="useBackground"></param>
    public void SetBackground(bool useBackground)
    {
        m_useBackground = useBackground;

        InitBackGround();
    }


    /// <summary>
    /// 배경 이미지 설정
    /// </summary>
    private void InitBackGround()
    {
        GameObject go = ZinUtil.GetChildObj(gameObject, "Sprite_background");
		if (go == null)
			return;

        if (!m_useBackground)
        {
            BoxCollider box = go.GetComponent<BoxCollider>();
            if (go.GetComponent<BoxCollider>() == null)
            {
                box = go.AddComponent<BoxCollider>();
            }
            box.size = new Vector3(2000, 3000, 0);
        }
        else
        {
            BoxCollider box = go.GetComponent<BoxCollider>();
            if (box != null)
            {
                Destroy(box);
            }
        }
    }

    void Start()
    {
        InitTween();

		if (m_isShow) {
			Show ();
		}
    }

    public void Show(ZinWindow prevWindow = null)
    {
        if (ZinWindow.OpendWindow == null)
        {
            ShowTweenWindow();
            ZinWindow.OpendWindow = this;
        }
        else
        {
            if (prevWindow != null)
            {
                PrevWindow = prevWindow;

                PrevWindow.HideTweenWindow();
                ShowTweenWindow();
                ZinWindow.OpendWindow = this;
            }
        }

    }

    /// <summary>
    /// 이전 윈도우 열기
    /// </summary>
    //public void HideAndPrevShow()
    //{
    //    Hide();

    //    if (PrevWindow != null)
    //    {
    //        PrevWindow.Show();
    //    }
    //}

	public void Hide(ZinWindow openWindow = null)
    {
        HideTweenWindow();

        if (PrevWindow != null)
        {
            ZinWindow.OpendWindow = null;

			if (openWindow != null) {
				openWindow.Show ();
			} else {
				PrevWindow.Show ();
			}
            PrevWindow = null;
        }
        else
        {
            ZinWindow.OpendWindow = null;
            PrevWindow = null;
        }
    }




    private void InitTween()
    {
        m_tweenScale.animationCurve.AddKey(new Keyframe(0.7f, 1.1f, 0, 0));

        m_tweenScale.duration = m_showSpeed;
        m_tweenScale.enabled = false;
        m_tweenScale.AddOnFinished(new EventDelegate(this, "OnFinished"));

        m_tweenAlpha.duration = m_showSpeed / 2;
        m_tweenAlpha.enabled = false;

        transform.localScale = Vector3.zero;
    }

    private void OnFinished()
    {
        SendMessage();
        SetShowWindow(m_isShow, false);
    }

    /// <summary>
    /// 창 열리고 닫히는 이벤트시 메세지를 날릴 함수명 설정
    /// </summary>
    /// <param name="opendMethodName"></param>
    /// <param name="closedMethodName"></param>
    public void SetSendMethodName(string opendMethodName, string closedMethodName)
    {
        this.openedWindowName = opendMethodName;
        this.closedWindowName = closedMethodName;
    }


    void SetShowWindow(bool isShow, bool isStart)
    {
        if (onShowWindow != null)
        {
            onShowWindow(isShow, isStart);
        }
    }

    private void ShowTweenWindow()
    {

        m_tweenScale.from = Vector3.zero;
        m_tweenScale.to = Vector3.one;

        m_tweenScale.enabled = true;
        m_tweenScale.ResetToBeginning();
        m_isShow = true;

        SetShowWindow(m_isShow, true);

        m_tweenAlpha.from = 0;
        m_tweenAlpha.to = 1;
        m_tweenAlpha.enabled = true;
        m_tweenAlpha.ResetToBeginning();

        if (m_onSound)
        {
            SoundFX.Instance.PlaySound(SoundType.SHOW_POPUP);
        }
    }

    private void HideTweenWindow()
    {
        if (!m_isShow) return;

        m_tweenScale.delay = 0;
        m_tweenScale.from = Vector3.one;
        m_tweenScale.to = Vector3.zero;

        m_tweenScale.enabled = true;
        m_tweenScale.ResetToBeginning();
        m_isShow = false;

        SetShowWindow(m_isShow, true);

        m_tweenAlpha.delay = 0;
        m_tweenAlpha.from = 1;
        m_tweenAlpha.to = 0;
        m_tweenAlpha.enabled = true;
        m_tweenAlpha.ResetToBeginning();
    }

    private void SendMessage()
    {
        bool isNull = string.IsNullOrEmpty(m_functionName);

        if (m_notify != null && isNull)
        {
            if (m_isShow)
            {
                m_notify.SendMessage(this.openedWindowName, this);
            }
            else
            {
                m_notify.SendMessage(this.closedWindowName, this);
            }
        }
        else if (m_notify != null && !isNull)
        {
            m_notify.SendMessage(m_functionName);
        }
    }
}
