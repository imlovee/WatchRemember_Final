using UnityEngine;
using System.Collections;

public class SetTutorial4 : MonoBehaviour
{
    public UIButton btnNext;

    void Start()
    {
        SetButton();
    }

    private void SetButton()
    {
        btnNext.onClick.Add(new EventDelegate(TutorialManager.Instance, "SetEndPlaying"));
    }

}
