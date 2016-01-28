using UnityEngine;
using System.Collections;

public class InappRestore : MonoBehaviour {
	public Purchaser[] purchasers;
	public GameObject root;

	void Start () {
	}

	public void Restore() {

		purchasers = root.GetComponentsInChildren<Purchaser>();
		for (int i = 0; i < purchasers.Length; i++) {
			purchasers[i].RestorePurchases();
		}

		Application.LoadLevel(1);

	}
}
