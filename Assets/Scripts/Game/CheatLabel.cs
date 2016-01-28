using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class CheatLabel : MonoBehaviour
{
    private UILabel m_label;

    void Start()
    {
        m_label = GetComponent<UILabel>();
        if (ControlCheat.OnCheat)
        {
            m_label.gameObject.SetActive(true);
        }
        else
        {
            m_label.gameObject.SetActive(false);
        }
    }

    public void SetText(string text)
    {
        if (m_label == null) return;

        m_label.text = text;
    }
         
}
