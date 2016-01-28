using UnityEngine;
using System.Collections;

public enum SoundType
{
    NONE = -1,
    COUNT = 0,
    PAGE_FLIP,
    STAGE_CLEAR,
    GAME_OVER,
    SHOW_POPUP,
    SHOW_LABEL,
    COUNT_NUMBER,
    GAME_CLEAR,
    LEVEL_UP,
    START_DELAY
}


public class SoundFX : MonoBehaviour
{
    public static SoundFX Instance;

    public AudioClip[] m_audioClips;

    public delegate void EventSoundState(bool isEnd, SoundType soundType);
    public static event EventSoundState eventSoundState;

    void Awake()
    {
        Instance = this;

        PackageManager.Instance.onLoaded += Instance_onLoaded;
    }

    void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
            m_audioClips = PackageManager.Instance.SoundClips;
        }
    }

    public void PlaySound(int index)
    {
        if (m_audioClips == null) return;

        if (index < 0 || index >= m_audioClips.Length) return;
        if (SoundManager.m_isOn)
        {
            SoundType type = (SoundType)index;
            eventSoundState(false, type);
            NGUITools.PlaySound(m_audioClips[index], 1, 1);

            StartCoroutine(EndSound(type, m_audioClips[index].length));
        }
    }



    /// <summary>
    /// 사운드 종료시점 생성
    /// </summary>
    /// <param name="type"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator EndSound(SoundType type, float delay)
    {
        yield return new WaitForSeconds(delay);
        eventSoundState(true, type);
    }

    public void PlaySound(SoundType sType)
    {
        PlaySound((int)sType);
    }
}
