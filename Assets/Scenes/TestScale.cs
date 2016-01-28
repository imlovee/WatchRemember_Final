using UnityEngine;
using System.Collections;

public class TestScale : MonoBehaviour {

	public float speed = 0.01f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate (speed, 0, 0);
	
	}
}
