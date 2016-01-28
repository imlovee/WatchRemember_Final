using UnityEngine;
using System.Collections;

public class LoadLevelOnBack : MonoBehaviour
{
    public string m_sceneName;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!string.IsNullOrEmpty(m_sceneName))
            {
                Application.LoadLevel(m_sceneName);
            }
        }
    }
}
