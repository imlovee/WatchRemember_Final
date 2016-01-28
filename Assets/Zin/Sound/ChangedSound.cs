using UnityEngine;
using System.Collections;

public class ChangedSound : ZinBehaviour
{
    void Awake()
    {
        SoundManager.Instance.ChangedSound += SetChangedSound;
    }

    void Start()
    {
        SetChangedSound(SoundManager.m_isOn);
    }

    private void SetChangedSound(bool onOff)
    {
        Send(onOff);
    }

}
