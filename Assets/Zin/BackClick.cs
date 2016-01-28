using UnityEngine;
using System.Collections;

public class BackClick : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int prevLevel = Application.loadedLevel - 1;
            if (prevLevel < 0)
            {
                Application.Quit();
            }
            else
            {
                Application.LoadLevel(prevLevel);
            }
        }
    }
}
