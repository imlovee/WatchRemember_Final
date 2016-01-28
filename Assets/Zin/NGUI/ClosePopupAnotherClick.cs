using UnityEngine;
using System.Collections;

/// <summary>
/// 다른 곳 클릭시에 창닫기 기능구현
/// UISprite와 같이 사용
/// </summary>
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(UIButton))]
public class ClosePopupAnotherClick : MonoBehaviour
{
    public GameObject c_popupGo;

    private UISprite m_sprite;

    void Start()
    {
        m_sprite = GetComponent<UISprite>();

        Vector3 size = new Vector3(Screen.width, Screen.height, 1);
        m_sprite.SetDimensions((int)size.x, (int)size.y);
    }

    void OnClick()
    {
        c_popupGo.SetActive(false);
    }
}
