using UnityEngine;
using System.Collections;

public class SetTutorial1 : MonoBehaviour
{
    public GameObject startBtn;
    public GameObject dummyBtn;

    public ZinWindow window;


    void Start()
    {
        window = GetComponent<ZinWindow>();
        window.onShowWindow += window_onShowWindow;
    }

    void window_onShowWindow(bool isShow, bool isStart)
    {
        if (isShow && isStart)
        {
            if (dummyBtn == null)
            {
                GameObject pageDown = ControlView.Instance.m_currentPageDown;
                if (pageDown == null) return;

                WRPageMainDown mainDown = pageDown.GetComponent<WRPageMainDown>();
                startBtn = mainDown.m_startBtn.gameObject;

                dummyBtn = GameObject.Instantiate(startBtn) as GameObject;
                dummyBtn.transform.parent = transform;
                dummyBtn.transform.localPosition = startBtn.transform.localPosition;
                dummyBtn.transform.localScale = startBtn.transform.localScale;
            }
        }
    }

    void OnDestroy()
    {
        window.onShowWindow -= window_onShowWindow;
    }

}
