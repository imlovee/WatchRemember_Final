using UnityEngine;
using System.Collections;

public class ShowDragBtn : MonoBehaviour {
	public enum BtnType
	{
		PREV,
		NEXT
	}

	public ItemClick itemClick;
	private PackageList packageList;
	public BtnType btnType = BtnType.NEXT;

	void Awake() {
	}

	void Start () {
		itemClick = GetComponentInParent<ItemClick> ();
		packageList = PackageListManager.Instance.packList;
	}

	public void InitBtn() {
		if (btnType == BtnType.NEXT) {
			if (itemClick.index != packageList.packages.Length) {
				gameObject.SetActive (true);
			} else {
				gameObject.SetActive (false);
			}
		} else {
			if (itemClick.index > 0) {
				gameObject.SetActive (true);
			} else {
				gameObject.SetActive (false);
			}
		}
	}
	
}
