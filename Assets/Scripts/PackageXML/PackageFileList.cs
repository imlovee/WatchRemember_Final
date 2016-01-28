using UnityEngine;
using System.Collections;

/// <summary>
/// 패키지에 필요한 파일 리스트
/// </summary>
public class PackageFileList : MonoBehaviour
{
    private static PackageFileList instance;
    public static PackageFileList Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PacakageFileList");
                instance = go.AddComponent<PackageFileList>();
            }

            return PackageFileList.instance;
        }
    }

    public string[] FileNames = {"Font.unity3d"
                                ,"BGSound.unity3d"
                                , "Common_pack.unity3d"
                                , "Effect.unity3d"
                                , "GameLevel.xml"
                                , "GamePopup.unity3d"
                                , "Language.xml"
                                , "PageMain.unity3d"
                                , "PageQuestion.unity3d"
                                , "Picture.unity3d"
                                , "Popup.unity3d"
                                , "Sound.unity3d"
                                , "Tutorial.unity3d"};

}
