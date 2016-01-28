using UnityEngine;
using System.Collections;

public class SetTutorialWindow : MonoBehaviour
{
    public UILabel[] labels;
    public UIPanel[] panels;
    public UIButton[] buttons;

    public UITweener[] tweens;


    void Awake()
    {
        tweens = GetComponentsInChildren<UITweener>();
    }

    void Start()
    {

    }

    void OpenedWindow(ZinWindow w)
    {
        ShowTweens(true);
    }

    void ClosedWindow(ZinWindow w)
    {
        ShowTweens(false);
    }

    public void ShowTweens(bool isShow)
    {
        if (tweens == null) return;

        for (int i = 0; i < tweens.Length; i++)
        {
            if (tweens[i].transform != transform)
            {
                tweens[i].enabled = isShow;
            }
        }
    }

    public void SetText(int index, string text)
    {
        if (index < 0 || index >= labels.Length) return;

        labels[index].text = text;
    }

    public void SetPanel(int index, Vector3 pos)
    {
        if (index < 0 || index >= panels.Length) return;

        panels[index].transform.localPosition = pos;
    }

    public void SetButton(int index, MonoBehaviour script, string functionName)
    {
        if (index < 0 || index >= buttons.Length) return;

        buttons[index].onClick.Add(new EventDelegate(script, functionName));

    }
}
