using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class Text
{
    [XmlAttribute("id")]
    public string id;

    public Lang[] Languages;


    public string GetText(SystemLanguage lang)
    {
        if (Languages == null) return null;

        for (int i = 0; i < Languages.Length; i++)
        {
            if (lang == Languages[i].Country)
            {
                return Languages[i].GetValue();
            }
        }

        return null;
    }
}
