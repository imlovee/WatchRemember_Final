using UnityEngine;
using System.Collections;

public class ZinMath
{
    /// <summary>
    /// 두배열이 같은 값을 가지고 있는지 비교
    /// </summary>
    /// <param name="arr1"></param>
    /// <param name="arr2"></param>
    /// <returns></returns>
    public static bool IsEqual(int[,] arr1, int[,] arr2)
    {
        if (arr1 == null || arr2 == null) return false;
        if (arr1.Length != arr2.Length) return false;

        for (int i = 0; i < arr1.GetLength(0); i++)
        {
            for (int k = 0; k < arr1.GetLength(1); k++)
            {
                if (arr1[i, k] != arr2[i, k])
                {
                    return false;
                }
            }
        }
        return true;
    }

}