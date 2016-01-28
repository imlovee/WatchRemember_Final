using UnityEngine;
using System.Collections;

public class SetSoundOnClick : MonoBehaviour
{
    private SoundManager m_sound;

    void Start()
    {
        m_sound = SoundManager.Instance;
    }

    void OnClick()
    {
        m_sound.ChangeSoundSetting();
    }
}
