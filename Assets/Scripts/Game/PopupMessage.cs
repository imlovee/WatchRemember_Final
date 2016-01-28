using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ZinWindow))]
public class PopupMessage : MonoBehaviour
{
    public ZinWindow m_zinWindow;
    public UILabel m_label;
    public string m_message;

    void Awake()
    {
        m_zinWindow = GetComponent<ZinWindow>();
        m_label = GetComponentInChildren<UILabel>();

    }
        
    void Start()
    {
        Hide();
    }

    public void Show()
    {
        if (m_label == null) return;
        if (m_zinWindow == null) return;

        m_label.text = m_message;
        m_zinWindow.Show();
        GameState.m_state = PlayState.PAUSE;
    }

    public void Hide()
    {
        if (m_zinWindow == null) return;
        m_zinWindow.Hide();
        GameState.m_state = PlayState.PLAY;
    }

    public void Show(string message)
    {
        if (m_label == null) return;

        m_message = message;
        Show();
    }
}
