using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIScrollView))]
public class InitControlView : MonoBehaviour {
	private UIScrollView scrollView;


	// Use this for initialization
	void Start () {
		scrollView = GetComponent<UIScrollView> ();
	}
	
}
