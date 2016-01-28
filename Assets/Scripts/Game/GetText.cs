using UnityEngine;
using System.Collections;

public class GetText : MonoBehaviour {

    public string text;

	// Use this for initialization
	void Start () {
        text = GetComponent<UILabel>().text;
	}
	
}
