using UnityEngine;
using System.Collections;

public class BuyPackageAsMoney : MonoBehaviour {

	public static BuyPackageAsMoney Instance;

	public WindowMessageOK SuccessWindow;
	public WindowShortageCash FailWindow;

	public string buyId;

	public string packageId;

	void Awake() {
		BuyPackageAsMoney.Instance = this;
	}

	void Start () {
		#if UNITY_ANDROID
		GoogleIABManager.BillingResultOK += InappBillingOK;

		#elif UNITY_IOS

		#endif
	}

	#if UNITY_IOS
	#elif UNITY_ANDROID

	void InappBillingOK(Item item, string id) {
		this.SuccessWindow.Show (this.packageId);
	}
	#endif




}
