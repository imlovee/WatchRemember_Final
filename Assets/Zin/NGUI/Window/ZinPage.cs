using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//[RequireComponent(typeof(TweenPosition))]
public class ZinPage : MonoBehaviour
{
    public static int CurrentPage = 0;
    public static List<ZinPage> PageList = new List<ZinPage>();

    private float m_screenWidth = 0;

    //private UIPanel m_panel;
    public int PageNo = 0;

    public readonly int START_PAGE_NO = 0;

    void Awake()
    {
        //m_panel = GetComponent<UIPanel>();

        //tweenPosition = GetComponent<TweenPosition>();
        TouchEvent.onTouchEvent += OnTouchEvent;
    }

    private void OnTouchEvent(TouchPhase type, int id, Vector2 pos, Vector2 deltaPos)
    {
        switch (type)
        {
            case TouchPhase.Began:
                break;

            case TouchPhase.Moved:
                break;

            case TouchPhase.Ended:
                break;

            default:
                break;
        }
    }


    void Start()
    {
        UIRoot root = transform.root.GetComponent<UIRoot>();
        m_screenWidth = root.manualWidth;
        PageList.Add(this);

        SetPagePosition(START_PAGE_NO);
    }

    void OnDestory()
    {
        if (PageList != null)
        {
            PageList.Clear();
            PageList.TrimExcess();
        }
    }

    private Vector3 GetPosition()
    {
        return new Vector3(m_screenWidth * (PageNo - ZinPage.CurrentPage), 0, 0);
    }


    private void SetPagePosition(int pageNo)
    {
        ZinPage.CurrentPage = pageNo;
        transform.localPosition = GetPosition();
    }

    public void NextPage()
    {
        int pageNo = ZinPage.CurrentPage + 1;

        MovePageAll(pageNo);
    }

    public void PrevPage()
    {
        int pageNo = ZinPage.CurrentPage - 1;

        MovePageAll(pageNo);
    }

    private void MovePageAll(int pageNo)
    {
        if (pageNo < ZinPage.PageList.Count)
        {
            for (int i = 0; i < PageList.Count; i++)
            {
                PageList[i].SetPagePosition(pageNo);
            }
        }
    }

    private void InitMovePage()
    {
        MovePageAll(START_PAGE_NO);
    }
}
