using UnityEngine;
using System.Collections;
using System.IO;

public class ZinString 
{
    /// <summary>
    /// 문자열 null 확인 후 오류메세지 출력
    /// </summary>
    /// <param name="str"></param>
    /// <param name="msg"></param>
    public static void IsNullOrEmpty(string str, MonoBehaviour name = null, string msg = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            if (string.IsNullOrEmpty(msg))
            {
                Debug.LogError(string.Format("{0} : string is null or empty", name));
            }
            else
            {
                Debug.LogError(string.Format("{0} : {1}", name, msg));
            }
        }
    }

    public static string[] ToArray(string str)
    {
        if (str == null) return null;

        return new string[] { str };
    }

    public static string[] ToArray(int val)
    {
        return ToArray(val.ToString());
    }

    public static string[] ToArray(params string[] param)
    {
        string[] result = new string[param.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = param[i];
        }

        return result;
    }

    public static string[] ToArray(params int[] param)
    {
        string[] result = new string[param.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = param[i].ToString();
        }

        return result;
    }

    public static int ConvertToInt(string str)
    {
        int number = -1;
        if (!int.TryParse(str, out number))
        {
            Debug.LogWarning("Convert Error : " + str);
        }
        return number;
    }

    /// <summary>
    /// 확장자 제거해서 파일 이름만 가져오기
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string GetFileName(string fullPath)
    {
        FileInfo f = new FileInfo(fullPath);
        string[] split = f.Name.Split(new char[] { '.' });
        string result = null;
        if (split.Length >= 2)
        {
            for (int i = 0; i < split.Length - 1; i++)
            {
                result += split[i];
            }
        }
        else
        {
            result = f.Name;
        }

        return result;
    }


}
