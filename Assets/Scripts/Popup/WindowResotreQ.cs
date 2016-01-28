using UnityEngine;
using System.Collections;

public class WindowResotreQ : MonoBehaviour {
	public UILabel msg; 
	public ZinWindow window;


	void Start () {
		window = GetComponent<ZinWindow> ();
	}

	public void Show() {
		msg.text = "Resotre Purchase?";
		window.Show ();
	}
	
}
