using UnityEngine;
using System.Collections;

public class Common : MonoBehaviour {
	public static Common Instance;

	public UIAtlas CommonAtlas;

	void Awake () {
		Instance = this;
	}
}
