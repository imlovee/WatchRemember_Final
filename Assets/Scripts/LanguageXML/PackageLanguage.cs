using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class PackageLanguage
{
    [XmlAttribute("name")]
    public string name;

    public Text[] TextObjects;

    private SortedList<string, string> textList;


    /// <summary>
    /// 언어 설정
    /// </summary>
    /// <param name="lang"></param>
    public void InitLanguage(SystemLanguage lang)
    {
        if (textList == null)
        {
            textList = new SortedList<string, string>();
        }
        else
        {
            textList.Clear();
            textList.TrimExcess();
        }

        for (int i = 0; i < TextObjects.Length; i++)
        {
            string text = TextObjects[i].GetText(lang);
            textList.Add(TextObjects[i].id, text);
        }
    }

    /// <summary>
    /// 텍스트 가져오기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetText(string id)
    {
        if (textList == null) return null;

        if (textList.Keys.Contains(id))
        {
            return textList[id];
        }
        else
        {
            return null;
        }
    }

    //public Text FindText(string id)
    //{
    //    if (text == null) return;
    //}

}
