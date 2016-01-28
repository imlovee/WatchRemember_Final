using UnityEngine;
using System.Collections;

public class GotoUrlOnClick : MonoBehaviour {

    public string m_url;

	void Start () {
	
	}

    void OnClick()
    {
        if (!string.IsNullOrEmpty(m_url)) Application.OpenURL(m_url);
    }

}
