using UnityEngine;
using System.Collections;

public delegate void OnSound(bool onOff);

public class SoundManager : MonoBehaviour
{
    private readonly string SOUND_PREF_NAME = "OnSound";

    public event OnSound ChangedSound;

    private static SoundManager m_instance;
    public static SoundManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject go = new GameObject("SoundManager");
                m_instance = go.AddComponent<SoundManager>();

                DontDestroyOnLoad(go);
            }

            return m_instance;
        }
    }

    public static bool m_isOn = false;

    void Start()
    {
        GetSoundSetting();
    }

    private void GetSoundSetting()
    {
        m_isOn = GetSoundValue();
        Debug.Log("sound : " + m_isOn);

        ChangedSound(m_isOn);
    }

    public bool GetSoundValue()
    {
        int soundValue = PlayerPrefs.GetInt(SOUND_PREF_NAME, 1);
        return soundValue == 1 ? true : false;
    }


    public void ChangeSoundSetting()
    {
        m_isOn = !m_isOn;
        Debug.Log("sound : " + m_isOn);

        ChangedSound(m_isOn);
        SetSoundSetting();
    }

    private void SetSoundSetting()
    {
        PlayerPrefs.SetInt(SOUND_PREF_NAME, m_isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
