using UnityEngine;
using System.Collections;

public class SendMessageOnClick : CustomOnClick
{
    public GameObject[] c_notify;
    public string c_functionName;

    void Start()
    {

    }

    void OnClick()
    {
        if (c_notify == null) return;

        StartCoroutine(WaitForEndOfSound());

        for (int i = 0; i < c_notify.Length; i++)
        {
            c_notify[i].SendMessage(c_functionName);   
        }
    }
}
