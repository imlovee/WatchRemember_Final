using UnityEngine;
using System.Collections;


public class InitPlayerPrefs : MonoBehaviour {

	#if UNITY_EDITOR
	public bool Reset  = false;

	void Start () {
	
	}

	void Init() {
		Debug.Log ("Init PlayerPrefs");
		PlayerPrefs.DeleteAll ();
	
	}
	
	void Update () {
		if (this.Reset) {
			Init ();

			this.Reset = false;
		}
	}

	#endif
}
