using UnityEngine;
using System.Collections;

public class PageFlip : MonoBehaviour
{
    public UIPanel m_pageUp;
    public UIPanel m_newPageUp;

    public UIPanel m_pageDown;
    public UIPanel m_newPageDown;

    public UIPanel m_parentUp;
    public UIPanel m_parentDown;

    public float m_flipSpeed = 0.2f;
    public int m_pageNo = -1;

    /// <summary>
    /// 페이지 종료시점에 메세지 날려줄 오브젝트
    /// </summary>
    public GameObject m_notify;
	public static PageType m_pageType = PageType.NONE;


    private readonly string END_FUNCTION_NAME = "EndFlip";

    private readonly string BACKGROUND_NAME = "background";
    private readonly string BTN_NAME = "btn";
    private readonly string UI_NAME = "UI";
    private readonly string TOGGLE_NAME = "toggle";
    private readonly string QUESTION = "answer_image";
    
    //  페이지에서 사용할 depth 한계값
    private readonly int LIMIT_DEPTH = 20;

    void Awake()
    {
        //InitTweens();
        m_pageNo = 0;

        m_pageType = PageType.MAIN;
        PackageManager.Instance.onLoaded += Instance_onLoaded;
    }

    void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
            //GameObjects go = PackageManager.Instance.gameObjects;
            //GamePages pages = go.gamePages;

        }
    }

    private void DestroyPage(UIPanel go)
    {
        if (go != null)
        {
            GameObject.Destroy(go.gameObject);
        }
    }

    /// <summary>
    /// 새로운 페이지가 생성되는 시점에 실행
    /// 기존 newPage가 이전페이지로 변경됨
    /// </summary>
    /// <param name="pageUp"></param>
    /// <param name="pageDown"></param>
    public void SetPrevPage()
    {
        int downDepth = m_pageDown == null ? 0 : m_pageDown.depth;
        int upDepth = m_pageUp == null ? 0 : m_pageUp.depth;

        DestroyPage(m_pageDown);
        DestroyPage(m_pageUp);

        m_pageDown = m_newPageDown;
        SetDepth(m_pageDown, downDepth);

        m_pageUp = m_newPageUp;
        SetDepth(m_pageUp, upDepth);
    }


    void SetTween(GameObject go, Vector3 from, Vector3 to, string onFinishedFunctionName = null)
    {
        TweenScale tweenScale = go.GetComponent<TweenScale>();
        tweenScale.from = from;
        tweenScale.to = to;

        tweenScale.style = UITweener.Style.Once;
        tweenScale.duration = m_flipSpeed;
        tweenScale.enabled = false;

        go.transform.localScale = from;

        if (!string.IsNullOrEmpty(onFinishedFunctionName))
        {
            tweenScale.onFinished.Add(new EventDelegate(this, onFinishedFunctionName));
        }
    }

    bool StartTween(UIPanel go, Vector3 from, Vector3 to)
    {
        if (go == null) return false;

        TweenScale tweenScale = go.GetComponent<TweenScale>();
        if (tweenScale != null)
        {
            tweenScale.from = from;
            tweenScale.to = to;
            tweenScale.ResetToBeginning();
            tweenScale.enabled = true;
        }
        return true;
    }

    /// <summary>
    /// 페이지 전환
    /// </summary>
    /// <param name="pageUp"></param>
    /// <param name="pageDown"></param>
    public void Flip(PageType pageType, GameObject pageUp, GameObject pageDown)
    {
        m_pageType = pageType;

        InitTweens(pageUp, pageDown);
        StartFlip();

    }

    void InitTweens(GameObject goUp, GameObject goDown)
    {
        int prevUpDepth = -1;
        if (m_pageUp != null)
        {
            if (m_pageNo == 0)
            {
                m_pageUp.depth = m_pageNo;
            }

            prevUpDepth = m_pageUp.depth;
            SetTween(m_pageUp.gameObject, Vector3.one, Vector3.one);
        }

        m_newPageUp = goUp.GetComponent<UIPanel>();

        SetTween(m_newPageUp.gameObject, new Vector3(1, 0, 1), Vector3.one, END_FUNCTION_NAME);
        m_newPageUp.depth = prevUpDepth + LIMIT_DEPTH;
        SetDepth(m_newPageUp, m_newPageUp.depth);

        int prevDownDepth = -1;
        if (m_pageDown != null)
        {
            if (m_pageNo == 0)
            {
                m_pageDown.depth = prevUpDepth + 1;
            }

            prevDownDepth = m_pageDown.depth;
            SetTween(m_pageDown.gameObject, Vector3.one, new Vector3(1, 0, 1), "TopFlip");
        }

        m_newPageDown = goDown.GetComponent<UIPanel>();

        SetTween(m_newPageDown.gameObject, Vector3.one, Vector3.one);
        m_newPageDown.depth = prevDownDepth - LIMIT_DEPTH;
        SetDepth(m_newPageDown, m_newPageDown.depth);
    }

    void SetDepth(UIPanel panel, int depth)
    {
        if (panel == null) return;

        UIPanel[] sprites = panel.GetComponentsInChildren<UIPanel>();

        string widgetName = null;
        int questionCount = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            UIPanel widget = sprites[i];
            widgetName = widget.name;
            if (widgetName.Contains(BACKGROUND_NAME))
            {
                widget.depth = depth - 2;
            }
            else if (widgetName.Contains(BTN_NAME))
            {
                widget.depth = depth - 1;
            }
            else if (widgetName.Contains(TOGGLE_NAME) || widgetName.Contains(UI_NAME))
            {
                widget.depth = depth + 1;
            }
            else if (widget.name.Contains(QUESTION))
            {
                widget.depth = depth + questionCount;
                questionCount++;
            }
            else 
            {
                widget.depth = depth;
            }
        }
    }

    /// <summary>
    /// 페이지 넘김 효과 시작
    /// </summary>
    void StartFlip()
    {
        bool isStart = StartTween(m_pageDown, Vector3.one, new Vector3(1, 0, 1));
        if (isStart)
        {
            SoundFX.Instance.PlaySound(SoundType.PAGE_FLIP);
        }
        else
        {
            m_newPageUp.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 하단 페이지 효과 끝난뒤 상단 효과 시작
    /// </summary>
    public void TopFlip()
    {
        StartTween(m_newPageUp, new Vector3(1, 0, 1), Vector3.one);
    }


    /// <summary>
    /// 페이지 넘김효과 종료
    /// </summary>
    public void EndFlip()
    {
        if (m_pageType == PageType.MAIN)
        {
            m_pageNo = 0;
        }
        else
        {
            m_pageNo++;
        }

        if (m_notify != null)
        {
            m_notify.SendMessage(END_FUNCTION_NAME, m_pageType);
        }
    }

}

