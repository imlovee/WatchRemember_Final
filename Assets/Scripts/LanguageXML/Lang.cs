using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class Lang
{
    [XmlAttribute("Country")]
    public SystemLanguage Country;

    public string value;

    public string GetValue()
    {
        // 공백처리
        return value.Replace("  ", "");
    }
}
