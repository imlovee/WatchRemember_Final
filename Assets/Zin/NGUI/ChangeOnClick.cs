using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
[RequireComponent(typeof(UISprite))]
public class ChangeOnClick : MonoBehaviour
{
    private UISprite m_sprite;
    private UIButton m_button;

    public bool m_on = false;

    public string m_offSpriteName;
    public string m_onSpriteName;

	public GameObjects gameObjects;

    void Awake()
    {
        m_sprite = GetComponent<UISprite>();
        m_button = GetComponent<UIButton>();

		gameObjects.setWidget += GameObjects_setWidget;
    }

    void GameObjects_setWidget (bool isEnd)
    {
		if (isEnd) {
			m_onSpriteName = m_sprite.spriteName;
			m_offSpriteName = m_sprite.spriteName.Replace ("_on", "_off");



			SetSprite(false);
		}
    }

    void Start()
    {
    }

    void OnClick()
    {
        m_on = !m_on;
        ChangeSprite();
    }

    public void SetSprite(bool isOn)
    {
        m_on = isOn;
        ChangeSprite();
    }

    void ChangeSprite()
    {
        if (m_on)
        {
            if (!string.IsNullOrEmpty(m_onSpriteName))
            {
                m_sprite.spriteName = m_onSpriteName;
                m_button.normalSprite = m_onSpriteName;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(m_offSpriteName))
            {
                m_sprite.spriteName = m_offSpriteName;
                m_button.normalSprite = m_offSpriteName;

            }
        }
    }
}
