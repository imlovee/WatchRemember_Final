using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이 정보를 담는 클래스
/// </summary>
public class PlayInfo : MonoBehaviour
{
    /// <summary>
    /// 게임 플레이 횟수
    /// </summary>
    private static int playCount = 0;
    public static int PlayCount
    {
        get { return playCount; }
    }

    void Awake()
    {
        //playCount = PlayerPrefs.GetInt(PrefNameManager.PREF_PLAY_COUNT, PrefNameManager.PREF_PLAY_COUNT_DEFAULT);
        playCount = 0;


        playCount++;
        PlayerPrefs.SetInt(NameManager.PREF_PLAY_COUNT, playCount);
        PlayerPrefs.Save();
    }
}
