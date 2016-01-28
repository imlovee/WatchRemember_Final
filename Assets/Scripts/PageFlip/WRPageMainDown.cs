using UnityEngine;
using System.Collections;

public class WRPageMainDown : WRPage
{
    public UIButton m_startBtn;
    public UIButton m_cheatDown;

    public void InitStartBtn(MonoBehaviour script, string functionName)
    {
        m_startBtn.onClick.Add(new EventDelegate(script, functionName));
    }

    public void SetBtnCheatDown(MonoBehaviour behaviour, string functionName)
    {
        m_cheatDown.onClick.Add(new EventDelegate(behaviour, functionName));
    }
}
