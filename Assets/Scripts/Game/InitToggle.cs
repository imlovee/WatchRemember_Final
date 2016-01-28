using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIToggle))]
public class InitToggle : MonoBehaviour
{
    private UIToggle m_toggle;
    public UISprite m_toggleSprite;


    void Awake()
    {
        m_toggle = GetComponent<UIToggle>();
    }

    public void SetToggle(int selectIndex)
    {
        if (selectIndex == 0)
        {
            m_toggle.group = 0;
            m_toggle.GetComponent<BoxCollider>().enabled = false;
        }
        else if (selectIndex > 0)
        {
            m_toggle.group = 1;
            m_toggle.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
