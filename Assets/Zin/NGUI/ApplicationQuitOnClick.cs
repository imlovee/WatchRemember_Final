using UnityEngine;
using System.Collections;

public class ApplicationQuitOnClick : MonoBehaviour
{

    void Start()
    {

    }

    void OnClick()
    {
        Application.Quit();
    }
}
