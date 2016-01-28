using UnityEngine;
using System.Collections;
using LitJson;

public class GoogleInappManager : MonoBehaviour {
	private static GoogleInappManager instance;
	public static GoogleInappManager Instance {
		get {
			return GoogleInappManager.instance;
		}
	}


	public delegate void OnBillingResultOK(string buyid);
	public event OnBillingResultOK BillingResultOK;

	public string PublicKey;

	#if UNITY_ANDROID
	private AndroidJavaObject curActivity;

	void Awake() {
		if (GoogleInappManager.instance == null) {
			GoogleInappManager.instance = this;
		}
	}


	void Start () {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		InAppInit(true);
	}

	private void InAppInit(bool bDebug = false)
	{
		if (string.IsNullOrEmpty(PublicKey))
		{
			Debug.LogError ("PublicKey Not Found");
			return;
		}

		curActivity.Call("InAppInit_U", PublicKey, bDebug);
	}

	private void InAppInitResult_J(string strResult)
	{
		Debug.Log ("InApp Init " + strResult);
	}


	public void BuyItem(string itemId)
	{
		curActivity.Call("InAppBuyItem_U", itemId);
		Debug.Log ("Request BuyItem");
	}

	/*
     * // Billing response codes
	public static final int BILLING_RESPONSE_RESULT_OK = 0;
	public static final int BILLING_RESPONSE_RESULT_USER_CANCELED = 1;
	public static final int BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE = 3;
	public static final int BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE = 4;
	public static final int BILLING_RESPONSE_RESULT_DEVELOPER_ERROR = 5;
	public static final int BILLING_RESPONSE_RESULT_ERROR = 6;
	public static final int BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED = 7;
	public static final int BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED = 8;
	*/
	/// <summary>
	/// 아이템 구입 결과 받기
	/// </summary>
	/// <param name="strResult"></param>
	private void InAppConsumeResult_J(string strResult)
	{
		JsonData jData = JsonMapper.ToObject(strResult);

		int iResult = System.Convert.ToInt32(jData["Result"].ToString());
		switch (iResult)
		{
		case 0:
			string strOrderId = jData ["OrderId"].ToString ();
			string strSku = jData ["Sku"].ToString ();
			string strPurchaseData = jData ["purchaseData"].ToString ();
			string strSignature = jData ["signature"].ToString ();
			Debug.Log (string.Format ("InAppConsume Success.\n{0}\n{1}\n{2}\n{3}", strOrderId, strSku, strSignature, strPurchaseData));

//			SetInappStatus(strSku, true);
//			SendItemInfo(strSku);

			break;
		default:
			Log.log("InAppConsume Failed: " + iResult);
			break;
		}
	}



	public void GetBuyList() {
		
	
	}

	/// <summary>
	/// 결제 완료 이벤트 등록
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	private void SetEvent(string itemId) {
		if (this.BillingResultOK != null) {
			this.BillingResultOK (itemId);
		}
	}
	#endif
}
