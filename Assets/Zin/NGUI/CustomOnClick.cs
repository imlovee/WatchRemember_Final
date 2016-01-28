using UnityEngine;
using System.Collections;

public class CustomOnClick : MonoBehaviour
{
    public UIPlaySound m_sound;

    void Awake()
    {
        m_sound = GetComponent<UIPlaySound>();
    }

    public IEnumerator WaitForEndOfSound()
    {
        if (m_sound != null && m_sound.audioClip != null)
        {
            yield return new WaitForSeconds(m_sound.audioClip.length);
        }
        yield return 0;
    }

}
