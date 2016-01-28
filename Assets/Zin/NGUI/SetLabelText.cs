using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class SetLabelText : MonoBehaviour
{
    public UILabel label;
    public string txt;

    void Awake()
    {
        label = GetComponent<UILabel>();
        Hide();
    }

    private void SetText(string text)
    {
        txt = text;
        label.text = text;
    }


    public IEnumerator SetText(string text, float delay)
    {
        Show();
        SetText(text);
        yield return new WaitForSeconds(delay);
    }

    public void Show()
    {
        ShowText(true);
    }

    private void ShowText(bool isShow)
    {
        if (label.enabled != isShow)
        {
            label.enabled = isShow;
        }
    }

    public void Hide()
    {
        ShowText(false);
    }

}
