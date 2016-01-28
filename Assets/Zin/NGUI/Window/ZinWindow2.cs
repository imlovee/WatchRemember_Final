using UnityEngine;
using System.Collections;

public class ZinWindow2 : MonoBehaviour
{
    public bool isShow = false;


    void Start()
    {
        if (isShow)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        transform.localScale = Vector3.one;
    }

    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }
}
