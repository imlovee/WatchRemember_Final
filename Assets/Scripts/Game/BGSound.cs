using UnityEngine;
using System.Collections;

public enum BGSoundType
{
    MAIN = 0
}


[RequireComponent(typeof(AudioSource))]
public class BGSound : MonoBehaviour
{
    public static BGSound Instance;

    private AudioSource m_audioSource;
    public AudioClip[] m_audioClips;

    void Awake()
    {
        Instance = this;

        m_audioSource = GetComponent<AudioSource>();

        InitAudioSource();

        SoundManager.Instance.ChangedSound += new OnSound(ChangeSound);
        
    }

    void Start()
    {
        PackageManager.Instance.onLoaded += Instance_onLoaded;
    }

    void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
            m_audioClips = PackageManager.Instance.BgSoundClips;
        }
    }

    private void ChangeSound(bool onOff)
    {
        if (onOff)
        {
            if (PageFlip.m_pageType == PageType.MAIN)
            {
                Pause();
            }
            else
            {
                PlaySound(BGSoundType.MAIN);
            }
        }
        else
        {
            Pause();
        }
    }


    private void InitAudioSource()
    {
        m_audioSource.loop = true;
        m_audioSource.playOnAwake = false;
    }


    public void PlaySound(int index)
    {
        if (m_audioClips == null) return;

        if (index < 0 || index >= m_audioClips.Length) return;
        if (SoundManager.m_isOn)
        {
            Stop();

            m_audioSource.clip = m_audioClips[index];
            m_audioSource.Play();
        }
    }

    public void PlaySound(BGSoundType bgType)
    {
        PlaySound((int)bgType);
    }

    public void PlaySound()
    {
        PlaySound(Random.Range(0, m_audioClips.Length));
    }

    public void Stop()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
    }

    public void Pause()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Pause();
        }
    }
}
