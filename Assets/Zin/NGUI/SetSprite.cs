using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class SetSprite : MonoBehaviour
{
    private UISprite m_sprite;
    public int m_width;
    public int m_height;


    void Start()
    {
        m_sprite = GetComponent<UISprite>();

        m_width = UISetup.Instance.m_background_width;
        m_height = UISetup.Instance.m_background_heght;

        m_sprite.SetDimensions(m_width, m_height);
    }
}
