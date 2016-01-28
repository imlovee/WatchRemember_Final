using UnityEngine;
using System.Collections;

public class ShowObject : MonoBehaviour {

	void Start () {
	
	}

	public void Show() {
		gameObject.SetActive (true);
	}

	public void Hide() {
		gameObject.SetActive (false);
	}
	
}
