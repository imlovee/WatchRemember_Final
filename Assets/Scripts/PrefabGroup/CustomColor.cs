using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;

public class CustomColor
{
    [XmlAttribute("R")]
    public float R;

    [XmlAttribute("G")]
    public float G;

    [XmlAttribute("B")]
    public float B;

    [XmlAttribute("A")]
    public float A;

    public CustomColor() { }

    public CustomColor(Color color)
    {
        R = color.r;
        G = color.g;
        B = color.b;
        A = color.a;
    }

    public Color GetColor()
    {
        return new Color(R, G, B, A);
    }
}
