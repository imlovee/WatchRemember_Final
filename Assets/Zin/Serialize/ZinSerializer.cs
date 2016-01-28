using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ZinSerializer
{
    public static BinaryFormatter bf = new BinaryFormatter();
    public static string Serialization(object serializableObject)
    {
        MemoryStream memoryStream = new MemoryStream();
        bf.Serialize(memoryStream, serializableObject);

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public static object Deserialization<T>(string serialzizableString)
    {
        if (string.IsNullOrEmpty(serialzizableString)) return null;

        MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(serialzizableString));
        T deserializedObject = (T)bf.Deserialize(memoryStream);
        return deserializedObject;
    }
}
