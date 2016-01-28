using UnityEngine;
using System.Collections;

public class WindowSetup : SetWindow
{
    void Start()
    {
        InitButtonLink();
    }

    /// <summary>
    /// 버튼 클릭시 연결 설정
    /// </summary>
    void InitButtonLink()
    {
        GameObject btnTuto = GetChildObj(gameObject, "Sprite_btn_tutorial");
        UIButton btn = btnTuto.GetComponent<UIButton>();
        btn.onClick.Add(new EventDelegate(TutorialManager.Instance, "RunTotorial"));

    }


}
