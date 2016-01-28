using UnityEngine;
using System.Collections;

public class SendEmailOnClick : MonoBehaviour
{
    public string c_emailAddress;
    public string c_subject;
    public string c_body;

    void Start()
    {

    }

    void OnClick()
    {
        string subject = MyEscapeURL(c_subject);
        string body = MyEscapeURL(c_body);

        Application.OpenURL("mailto:" + c_emailAddress + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
