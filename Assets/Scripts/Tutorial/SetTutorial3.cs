using UnityEngine;
using System.Collections;

public class SetTutorial3 : MonoBehaviour
{
    public GameObject target;
    public GameObject dummy;
    public int correct = -1;

    /// <summary>
    /// 기준이 될 손가락 이미지가 들어있는 panel
    /// </summary>
    public UIPanel mark;

    public ZinWindow window;


    void Start()
    {
        window = GetComponent<ZinWindow>();
        window.SetBackground(true);
        window.onShowWindow += window_onShowWindow;
    }

    void window_onShowWindow(bool isShow, bool isStart)
    {
        if (isShow)
        {
            if (dummy == null)
            {
                GameObject pageDown = ControlView.Instance.m_currentPageDown;
                if (pageDown == null) return;

                WRPageQuestionDown qDown = pageDown.GetComponent<WRPageQuestionDown>();
                target = qDown.m_defaultUI.gameObject;

                dummy = GameObject.Instantiate(target) as GameObject;
                dummy.transform.parent = transform;
                dummy.transform.localPosition = target.transform.localPosition;
                dummy.transform.localScale = target.transform.localScale;


                // 팝업 위에오도록 depth 조절
                UIPanel[] panels = dummy.GetComponentsInChildren<UIPanel>();
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].depth = 80;
                }

                GameObject pageUp = ControlView.Instance.m_currentPageUp;
                if (pageUp == null) return;

                //ControlQuestionPage qUp = pageUp.GetComponent<ControlQuestionPage>();

                // 정답인 collider 제거 
                InitToggle[] collider = dummy.GetComponentsInChildren<InitToggle>();
                for (int i = collider.Length - 1; i >= 0; i--)
                {
                    Destroy(collider[i]);
                    Destroy(collider[i].GetComponent<UIToggle>());
                    Destroy(collider[i].GetComponent<BoxCollider>());
                }
            }
        }
        else if (!isShow && !isStart)
        {
            if (dummy != null)
            {
                Destroy(dummy);
            }

        }
    }


    void OnDestroy()
    {
        window.onShowWindow -= window_onShowWindow;
    }


}
