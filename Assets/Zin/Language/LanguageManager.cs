using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager instance;
    public static LanguageManager Instance
    {
        get {
            if (instance == null)
            {
                GameObject go = new GameObject("LanguageManager");
                instance = go.AddComponent<LanguageManager>();
            }
            return LanguageManager.instance; 
        }
    }


    public SystemLanguage currentLang = SystemLanguage.English;
    public string path;

    public PackageLanguage pack;

    void Awake()
    {
        //path = GameDataManager.Instance.GetPath(PackageManager.Instance.packageName, "language.xml");

        //Serialize(path);
        //pack = Desrialize(path);
        //pack.InitLanguage(currentLang);

    }


    public void SetPackLanguage(string text)
    {
        currentLang = Application.systemLanguage;


        pack = (PackageLanguage)ZinSerializerForXML.Deserialization<PackageLanguage>(text);
        if (pack != null)
        {
            Debug.Log("Set Package Language Complete ");
        }
        else
        {
            Debug.LogError("Set Package Language Error: " + text);
        }
        pack.InitLanguage(currentLang);
    }

    //private PackageLanguage Desrialize(string filePath)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(PackageLanguage));

    //    FileStream fs = new FileStream(filePath, FileMode.Open);
    //    XmlReader reader = XmlReader.Create(fs);

    //    PackageLanguage pack = (PackageLanguage)serializer.Deserialize(reader);
    //    fs.Close();

    //    return pack;
    //}

    //private PackageLanguage Deserialize(string text)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(PackageLanguage));

    //    StringReader sr = new StringReader(text);
    //    //FileStream fs = new FileStream(filePath, FileMode.Open);
    //    XmlReaderSettings set = new XmlReaderSettings();
    //    set.IgnoreWhitespace = false;
    //    XmlReader reader = XmlReader.Create(sr, set);

    //    PackageLanguage pack = (PackageLanguage)serializer.Deserialize(reader);
    //    sr.Close();

    //    return pack;
    //}
}
