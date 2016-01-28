using UnityEngine;
using System.Collections;


public enum QuestionPageType
{
    NONE = -1,
    QUESTION_UP = 0,
    QUESTION_DOWN,
    ANSWER_UP,
    ANSWER_DOWN,
    CORRECT_ANSWER_UP,
    CORRECT_ANSWER_DOWN,
    MAIN_UP,
    MAIN_DOWN
}

public enum QuestionPageIndex 
{
    NONE = -1,
    UP = 0,
    DOWN
}

[RequireComponent(typeof(UIPanel))]
public class WRPage : MonoBehaviour
{
    public GameObject m_notify;
    public QuestionPageType m_pageType = QuestionPageType.NONE;
    public QuestionPageIndex m_pageIndex = QuestionPageIndex.NONE;

    public UILabel m_lbl_pageNo;
    public bool m_isUse = true;

    private int m_pageNo = -1;

    void Awake()
    {
        CustomInit();
    }


    void Start()
    {
        if (m_isUse)
        {
            InitPageNo();

            //m_notify = SetStage.Instance.gameObject;
            m_notify = SetWRPage.Instance.gameObject;

            m_notify.SendMessage("SetPage", this);
        }
    }

    private void InitPageNo()
    {
        if (m_lbl_pageNo != null)
        {
            m_lbl_pageNo.color = Color.blue;
            m_lbl_pageNo.fontSize = 100;
            m_lbl_pageNo.SetDimensions(150, 150);

            switch (m_pageType)
            {
                case QuestionPageType.NONE:
                    break;
                case QuestionPageType.QUESTION_UP:
                    InitPagePosition(730, 1215);
                    break;
                case QuestionPageType.QUESTION_DOWN:
                    InitPagePosition(-730, -1215);
                    break;
                case QuestionPageType.ANSWER_UP:
                    InitPagePosition(730, 1215);
                    break;
                case QuestionPageType.ANSWER_DOWN:
                    InitPagePosition(-730, -1215);
                    break;
                case QuestionPageType.CORRECT_ANSWER_UP:
                    InitPagePosition(730, 1215);
                    break;
                case QuestionPageType.CORRECT_ANSWER_DOWN:
                    InitPagePosition(-730, -1215);
                    break;
                case QuestionPageType.MAIN_UP:
                    break;
                case QuestionPageType.MAIN_DOWN:
                    break;
                default:
                    break;
            }
        }
    }

    private void InitPagePosition(float w, float h)
    {
        m_lbl_pageNo.transform.localPosition = new Vector3(w, h, 0);
    }




    public void SetPageNumber(int pageNo)
    {
        m_pageNo = pageNo;

        if (m_lbl_pageNo != null)
        {
            m_lbl_pageNo.text = m_pageNo.ToString();
        }
    }

    public virtual void CustomInit() { }
}
