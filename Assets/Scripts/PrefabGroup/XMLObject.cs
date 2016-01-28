using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class XMLObject
{
    [XmlAttribute("Name")]
    public string Name;

    public CustomVector2 Position;

    public CustomVector2 Size;

    public virtual void SetValue(UIWidget widget)
    {
        Name = widget.name;
        Position = new CustomVector2(widget.transform.localPosition);
        Size = new CustomVector2(widget.localSize);
    }

    public virtual void GetValue(UIWidget widget)
    {
        widget.name = Name;
        widget.transform.localPosition = Position.GetVector2();
        widget.SetDimensions((int)Size.x, (int)Size.y);
    }
}
