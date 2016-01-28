using UnityEngine;
using System.Collections;

/// <summary>
/// </summary>
public class ShowWindowOnBack : MonoBehaviour
{
    public ZinWindow m_QuitWindow;

    void Start()
    {
        if (m_QuitWindow == null)
        {
            Debug.LogError("Quit Window Not Found");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ZinWindow.OpendWindow == null)
            {
                if (!m_QuitWindow.m_isShow)
                {
                    m_QuitWindow.Show();
                }
            }
            else
            {
                ZinWindow currentWindow = ZinWindow.OpendWindow;
                if (currentWindow.windowType == WindowType.POPUP)
                {
                    ZinWindow.OpendWindow.Hide();
                }
            }
        }
    }
}
