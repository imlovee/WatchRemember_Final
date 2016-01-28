using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class SendLabelOnClick : ZinBehaviour
{
    private UILabel m_label;
    
    void Start()
    {
        m_label = GetComponent<UILabel>();
    }

    void OnClick()
    {
        Send(m_label.text);
    }
}
