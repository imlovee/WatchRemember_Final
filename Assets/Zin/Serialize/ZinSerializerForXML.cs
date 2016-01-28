using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public class ZinSerializerForXML
{

    public static bool Serialization<T>(object serializableObject, string filePath)
    {
        try
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(filePath);

            x.Serialize(writer, (T)serializableObject);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }

    public static T Deserialization<T>(string serialzizableString)
    {
        if (string.IsNullOrEmpty(serialzizableString)) return default(T);

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        T pack;
        try
        {
            StringReader sr = new StringReader(serialzizableString);
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreWhitespace = false;
            XmlReader reader = XmlReader.Create(sr, set);

            pack = (T)serializer.Deserialize(reader);
            sr.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Deserialization Error:" + e.ToString());
            Debug.LogError(serialzizableString);

            return default(T);
        }
        
        return pack;
    }
}
