using UnityEngine;
using System.Collections;

public class SetTutorial2 : MonoBehaviour
{
    public UIButton btnNext;

    void Start()
    {
        SetButton();
    }

    private void SetButton()
    {
        btnNext.onClick.Add(new EventDelegate(ControlView.Instance, "RunQuestionTween"));
    }
}
