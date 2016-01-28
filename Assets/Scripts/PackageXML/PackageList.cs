using System.Collections;

public class PackageList 
{
    public Package[] packages;


    /// <summary>
    /// 해당 패키지 가져오기
    /// </summary>
    /// <param name="packId"></param>
    /// <returns></returns>
    public Package GetPackage(string packId)
    {
        if (packages == null) return null;

        for (int i = 0; i < packages.Length; i++)
        {
            if (packages[i].ID == packId)
            {
                return packages[i];
            }
        }

        return null;
    }
}
