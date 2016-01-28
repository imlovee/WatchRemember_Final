using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

/// <summary>
/// 번들 파일 타입
/// </summary>
public enum FileType
{
    Font,                           // 폰트
    BGSound,                        // 배경음악
    Common_pack,                    // 패키지 공통
    Effect,                         // 이펙트
    GameLevel,                      // 게임설정
    GamePopup,                      // 게임내 팝업
    Language,                       // 언어팩
    PageMain,                       // 메인 페이지
    PageQuestion,                   // 질답 페이지
    Picture,                        // 게임에 사용되는 사진들
    Popup,                          // 시스템 팝업
    Sound,                          // 사운드
    Tutorial                        // 튜토리얼
}


public class ZinBundle
{
    [XmlAttribute("BundleType")]
    public FileType BundleType;

    [XmlAttribute("BundleName")]
    public string BundleName;

    public ZinBundle() { }
    public ZinBundle(FileType type, string name)
    {
        this.BundleType = type;
        this.BundleName = name;
    }

    /// <summary>
    /// 번들 값을 가지고 있는 XML 파일 이름 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetBundleXML()
    {
        string[] split = BundleName.Split(new char[] { '.' });

        if (split.Length == 2)
        {
            return string.Format("{0}.xml", split[0]);
        }
        return null;
    }
}
