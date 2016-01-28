using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class XMLLabel : XMLObject
{
    [XmlAttribute("FontName")]
    public string FontName;

    [XmlAttribute("FontSize")]
    public string FontSize;

    [XmlAttribute("FontStyle")]
    public FontStyle FontStyle;

    [XmlAttribute("Align")]
    public NGUIText.Alignment Alignment;

    [XmlAttribute("UseGradient")]
    public bool IsGridient;

    public CustomColor GradientTop;

    public CustomColor GradientBottom;

    [XmlAttribute("EffStyle")]
    public UILabel.Effect EffectStyle;

    public CustomColor EffectColor;

    public CustomVector2 EffectDistance;

    [XmlAttribute("EffSpacingX")]
    public float EffectiveSpacingX;

    [XmlAttribute("EffSpacingY")]
    public float EffectiveSpacingY;

    public CustomColor ColorTint;



    public XMLLabel(UILabel label)
    {
        SetValue(label);
    }

    public XMLLabel() { }


    public override void SetValue(UIWidget widget)
    {
        base.SetValue(widget);

        UILabel label = (UILabel)widget;

        FontName = label.bitmapFont.name;
        FontStyle = label.fontStyle;
        Alignment = label.alignment;
        IsGridient = label.applyGradient;
        GradientTop = new CustomColor(label.gradientTop);
        GradientBottom = new CustomColor(label.gradientBottom);
        EffectStyle = label.effectStyle;
        EffectColor = new CustomColor(label.effectColor);
        EffectDistance = new CustomVector2(label.effectDistance);
        EffectiveSpacingX = label.effectiveSpacingX;
        EffectiveSpacingY = label.effectiveSpacingY;
        ColorTint = new CustomColor(label.color);
    }

    public override void GetValue(UIWidget widget)
    {
        base.GetValue(widget);

        UILabel label = (UILabel)widget;

        label.bitmapFont.name = FontName;
        label.fontStyle = FontStyle;
        label.alignment = Alignment;
        label.applyGradient = IsGridient;
        label.gradientTop = GradientTop.GetColor();
        label.gradientBottom = GradientBottom.GetColor();
        label.effectStyle = EffectStyle;
        label.effectColor = EffectColor.GetColor();
        label.effectDistance = EffectDistance.GetVector2();
        //label.effectiveSpacingX = EffectiveSpacingX;
        //label.effectiveSpacingY = EffectiveSpacingY;
        label.color = ColorTint.GetColor();
    }
}
