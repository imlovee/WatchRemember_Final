using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;

public class CustomVector2
{
    [XmlAttribute("X")]
    public float x;

    [XmlAttribute("Y")]
    public float y;

    public CustomVector2() { }

    public CustomVector2(Vector2 vec)
    {
        x = vec.x;
        y = vec.y;
    }

    public Vector2 GetVector2()
    {
        return new Vector2(x, y);
    }
}
