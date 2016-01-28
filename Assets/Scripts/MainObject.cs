using UnityEngine;
using System.Collections;

public class MainObject : MonoBehaviour
{
    public GameObject[] gameObjects;

    void Start()
    {
        ControlView.Instance.changePage += Instance_changePage;
    }

    void Instance_changePage(PageType page)
    {
        if (page == PageType.MAIN)
        {
            Show(true);
        }
        else
        {
            Show(false);
        }
    }

    void Show(bool isShow)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(isShow);
        }
    }

}
