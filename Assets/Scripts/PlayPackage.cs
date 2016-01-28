using UnityEngine;
using System.Collections;

public class PlayPackage : MonoBehaviour
{
    public string PackageId;

    void Start()
    {

    }

    public void Play()
    {
        PlayerPrefs.SetString(NameManager.PREF_PLAY_PACKAGE, PackageId);
        PlayerPrefs.Save();

        Application.LoadLevel(1);
    }

}
