using UnityEngine;
using System.Collections;

/// <summary>
/// 클릭시에 연결되어 있는 label 값 설정
/// 
/// </summary>

[RequireComponent(typeof(UIButton))]
public class SetLabelOnClick : MonoBehaviour
{
    public string c_tag;                       // 구분자 겸 이름
    public UILabel c_label;
    public string c_text;

    void Start()
    {
        UIEventListener.Get(gameObject).onClick += OnMyClick;
    }

    private void OnMyClick(GameObject go)
    {
        SetText();
    }

    public void SetText(string text)
    {
        c_label.text = text;

        SetText();
    }

    private void SetText()
    {
        SetText(c_text, c_label);
    }

    private void SetText(string text, UILabel uiLabel)
    {
        if (uiLabel == null) return;

        if (!string.IsNullOrEmpty(text))
        {
            uiLabel.text = text;
        }
    }
}
