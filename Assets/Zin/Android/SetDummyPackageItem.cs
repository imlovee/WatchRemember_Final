using UnityEngine;
using System.Collections;

public class SetDummyPackageItem : MonoBehaviour {
	public GameObject item;


	void Start () {
	
	}

	public void SetItem() {
		Destroy (ZinUtil.GetChildObj (item, "Price"));
	}
	
}
