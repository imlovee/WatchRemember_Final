using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerPrefsSerializer
{
    public static BinaryFormatter bf = new BinaryFormatter();
    public static void Save(string prefKey, object serializableObject)
    {
        MemoryStream memoryStream = new MemoryStream();
        bf.Serialize(memoryStream, serializableObject);

        string tmp = System.Convert.ToBase64String(memoryStream.ToArray());
        PlayerPrefs.SetString(prefKey, tmp);
    }

    public static object Load<T>(string prefKey)
    {
        string tmp = PlayerPrefs.GetString(prefKey, string.Empty);
        if (tmp == null) return null;
        MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(tmp));
        T deserializedObject = (T)bf.Deserialize(memoryStream);
        return deserializedObject;
    }
}
