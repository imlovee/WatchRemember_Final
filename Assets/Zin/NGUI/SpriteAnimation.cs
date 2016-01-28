using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class SpriteAnimation : MonoBehaviour
{
    private UISprite m_sprite;
	public GameObjects gameobjects;
    public string[] m_spriteNames;

    private int index = 0;
    private bool isRun = false;

    void Awake()
    {
        m_sprite = GetComponent<UISprite>();
		gameobjects.setWidget += Gameobjects_setWidget;
    }

    void Gameobjects_setWidget (bool isEnd)
    {
		if (isEnd) {
			string str = "01";
			for (int i = 0; i < m_spriteNames.Length; i++) {
				m_spriteNames [i] = m_sprite.spriteName.Replace (str, string.Format ("0{0}", i + 1));
			}
		}
    }

    public void ChangedSound(bool onOff)
    {
        if (onOff)
        {
            Show();
            StartCoroutine(Play());
        }
        else
        {
            isRun = false;
            Hide();
            //StopCoroutine(Play());
        }
    }


    void Start()
    {
        Init();
        StartCoroutine(Play());
    }

    void Init()
    {
        if (m_spriteNames == null) return;

        m_sprite.spriteName = m_spriteNames[0];
    }

    IEnumerator Play()
    {
        if (m_spriteNames == null) yield return 0;

        isRun = true;

        float delay = 0.1f;
        while (isRun)
        {
            m_sprite.spriteName = m_spriteNames[index];
            yield return new WaitForSeconds(delay);

            index = index >= m_spriteNames.Length - 1 ? 0 : index + 1;
        }
    }

    void Hide()
    {
        m_sprite.enabled = false;
    }

    void Show()
    {
        m_sprite.enabled = true;
    }
}
