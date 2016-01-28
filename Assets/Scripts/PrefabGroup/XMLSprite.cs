using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class XMLSprite : XMLObject
{
    [XmlAttribute("AtlasName")]
    public string AtlasName;

    [XmlAttribute("SpriteName")]
    public string SpriteName;

    [XmlAttribute("Type")]
    public UISprite.Type Type;

    [XmlAttribute("Flip")]
    public UISprite.Flip Flip;

	[XmlAttribute("Depth")]
	public int Depth;

	[XmlAttribute("Parent")]
	public string parentName;

    public CustomColor ColorTint;

    public XMLSprite() { }

    public XMLSprite(UISprite sprite)
    {
        SetValue(sprite);
    }

    /// <summary>
    /// 멤버 변수 값 입력
    /// </summary>
    /// <param name="widget"></param>
    public override void SetValue(UIWidget widget)
    {
        base.SetValue(widget);

        UISprite sprite = (UISprite)widget;

        AtlasName = sprite.atlas == null ? "null" : sprite.atlas.name;
        SpriteName = sprite.spriteName;
        Type = sprite.type;
        Flip = sprite.flip;
        ColorTint = new CustomColor(sprite.color);
    }


    /// <summary>
    /// 멤버 변수를 해당 위젯에 세팅
    /// </summary>
    /// <param name="widget"></param>
    public override void GetValue(UIWidget widget)
    {
        base.GetValue(widget);

        UISprite sprite = (UISprite)widget;

        if (sprite.atlas != null)
        {
            sprite.atlas.name = AtlasName;
        }
        sprite.spriteName = SpriteName;
        sprite.type = Type;
        sprite.flip = Flip;
        sprite.color = ColorTint.GetColor();
    }
}
