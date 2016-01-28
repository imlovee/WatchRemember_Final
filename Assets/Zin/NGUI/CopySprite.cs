using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// sprite 프리팹 복사 스크립트
/// 동적으로 나열되는 스프라이트를 사용할때 적용
/// </summary>
public class CopySprite : MonoBehaviour
{
    public GameObject c_spritePrefab;                   // 복사할 sprite 프리팹
    public int m_count = 0;
    public Vector3 c_position = Vector3.zero;           // 시작 위치
    public float c_spacing = 0;                         // sprite 간 간격
    public Color color = Color.white;                   // sprite color
    public int c_limit = 5;                             // 최대값
    public Direction c_direction = Direction.RIGHT;     // UI 방향

    public List<UISprite> m_sprites;
    private float m_spriteWidth;
    private float m_spriteHeight;

    // test code;
    //public bool plus = false;
    //public bool minus = false;
    //public bool clear = false;

    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    void Awake()
    {
        m_spriteWidth = c_spritePrefab.GetComponent<UIWidget>().localSize.x;
        m_spriteHeight = c_spritePrefab.GetComponent<UIWidget>().localSize.y;

        m_sprites = new List<UISprite>();
    }

    //void Update()
    //{
    //    // test code
    //    if (plus)
    //    {
    //        AddSprite();
    //        plus = false;
    //    }

    //    if (minus)
    //    {
    //        DestroySprite();
    //        minus = false;
    //    }

    //    if (clear)
    //    {
    //        DestroySprites();
    //        clear = false;
    //    }
    //}

    void OnDestroy()
    {
        Clear();
    }

    void Clear()
    {
        if (m_sprites != null)
        {
            m_sprites.Clear();
            m_sprites.TrimExcess();
        }
    }


    public void SetSprites(int count)
    {
        if (m_sprites.Count > 0)
        {
            DestroySprites();
        }

        m_count = count;

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < m_count; i++)
        {
            if (i >= c_limit) return;

            SetPosition(ref pos);
            UISprite sprite = AddSprite(pos);
            m_sprites.Add(sprite);
        }
        
    }

    public UISprite AddSprite(string spriteName = null)
    {
        m_count++;
        if (m_sprites.Count >= c_limit) return null;

        Vector3 pos = Vector3.zero;
        if (m_sprites.Count > 0)
        {
            pos = m_sprites[m_sprites.Count - 1].transform.localPosition - c_position;
        }

        SetPosition(ref pos);

        UISprite sprite = AddSprite(pos);
        if (!string.IsNullOrEmpty(spriteName))
        {
            sprite.spriteName = spriteName;
        }

        m_sprites.Add(sprite);

        return sprite;
    }

    /// <summary>
    /// 해당 스프라이트 이름을 가진 스프라이트가 있는지 검색
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public bool Contain(string spriteName)
    {
        UISprite findSprite = GetSprite(spriteName);

        if (findSprite != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public UISprite GetSprite(string spriteName)
    {
        return m_sprites.Find(delegate(UISprite sprite)
        {
            return sprite.spriteName.Contains(spriteName);
        });
    }

    private void SetPosition(ref Vector3 pos)
    {
        switch (c_direction)
        {
            case Direction.LEFT:
                pos.x -= c_spacing + m_spriteWidth;
                break;
            case Direction.RIGHT:
                pos.x += c_spacing + m_spriteWidth;
                break;
            case Direction.UP:
                pos.y += c_spacing + m_spriteHeight;
                break;
            case Direction.DOWN:
                pos.y -= c_spacing + m_spriteHeight;
                break;
            default:
                break;
        }
    }


    public void DestroySprite()
    {
        if (m_sprites == null || m_sprites.Count == 0) return;

        UISprite sprite = m_sprites[m_sprites.Count - 1];
        DestroySprite(sprite);
    }

    public void DestroySprite(UISprite sprite)
    {
        m_count--;
        if (m_count >= c_limit) return;

        m_sprites.Remove(sprite);

        GameObject.Destroy(sprite.gameObject);
        SetPositions();
    }

    public void DestroySprite(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DestroySprite();
        }
    }

    void DestroySprites()
    {
        for (int i = m_sprites.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(m_sprites[i].gameObject);
        }
        m_count = 0;
        Clear();
    }

    private void SetPositions()
    {
        Vector3 pos = c_position;
        for (int i = 0; i < m_sprites.Count; i++)
        {
            SetPosition(ref pos);
            m_sprites[i].transform.localPosition = pos;
        }
    }

    UISprite AddSprite(Vector3 pos)
    {
        GameObject spriteGo = GameObject.Instantiate(c_spritePrefab) as GameObject;
        spriteGo.transform.parent = transform;
        spriteGo.transform.localScale = Vector3.one;
        spriteGo.transform.localEulerAngles = Vector3.zero;

        UISprite sprite = spriteGo.GetComponent<UISprite>();
        sprite.color = color;

        spriteGo.transform.localPosition = c_position + pos;

        return sprite;
    }
}
