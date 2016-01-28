using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;


public enum Item
{
    NONE = -1,
    NO_ADS = 0,
    PACK_ITEM = 1
}



public class GoogleIABManager : MonoBehaviour
{
    public delegate void OnBillingResultOK(Item item, string buyid);
    public static event OnBillingResultOK BillingResultOK;


    private static GoogleIABManager _instance;
    public string PublicKey;

    public PopupMessage m_message;
    public string[] m_itemIDs;

	#if UNITY_ANDROID

    public AndroidJavaObject curActivity;


    public static GoogleIABManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GoogleIABManager)) as GoogleIABManager;
                if (_instance == null)
                {
                    _instance = new GameObject("GoogleIABManager").AddComponent<GoogleIABManager>();
                }
            }

            return _instance;
        }
    }

    private List<Item> inappList = new List<Item>();


    void Awake()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        InAppInit(true);
    }

    public void OnDestroy()
    {
        if (inappList != null)
        {
            inappList.Clear();
            inappList.TrimExcess();
        }
    }

    //InAppBilling
    public void InAppInit(bool bDebug = false)
    {
        if (string.IsNullOrEmpty(PublicKey))
        {
            SetLog("Public Key Not Found");
            return;
        }

        curActivity.Call("InAppInit_U", PublicKey, bDebug);
    }

    /// <summary>
    /// 구입했었는지 여부 판단
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsBuy(Item item)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return inappList.Contains(item);
        }
        else
        {
            return GetInappStatus(item);
        }
    }

    public bool IsBuy(string id)
    {
        int itemIndex = FindIndex(id);
        return IsBuy((Item)itemIndex);
    }

    /// <summary>
    /// 로컬에 저장된 해당아이템 구매 여부 가져오기
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool GetInappStatus(Item item)
    {
        if (item == Item.NONE) return false;

        return PlayerPrefs.GetInt(m_itemIDs[(int)item], 0) == 0 ? false : true;
    }


    /// <summary>
    /// 구입 여부 로컬에 저장
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="buy"></param>
    public void SetInappStatus(string itemName, bool buy)
    {
        int status = buy ? 1 : 0;
        PlayerPrefs.SetInt(itemName, status);
        PlayerPrefs.Save();
    }



    private void InAppInitResult_J(string strResult)
    {
        SetLog("InApp Init " + strResult);
    }

 

    public void BuyItem(Item item)
    {
        int itemIndex = (int)item;

        if (itemIndex < 0 || itemIndex >= m_itemIDs.Length) return;

        InAppBuyItem(m_itemIDs[itemIndex]);
    }

	public void BuyItem(string id) {
		InAppBuyItem (id);
	}

	public void Buy(string id) {
		InAppBuyItem (id);
	}

    public void BuyNoAds()
    {
        BuyItem(Item.NO_ADS);
    }

    /*
     * // IAB Helper error codes
        public static final int IABHELPER_ERROR_BASE = -1000;
        public static final int IABHELPER_REMOTE_EXCEPTION = -1001;
        public static final int IABHELPER_BAD_RESPONSE = -1002;
        public static final int IABHELPER_VERIFICATION_FAILED = -1003;
        public static final int IABHELPER_SEND_INTENT_FAILED = -1004;
        public static final int IABHELPER_USER_CANCELLED = -1005;
        public static final int IABHELPER_UNKNOWN_PURCHASE_RESPONSE = -1006;
        public static final int IABHELPER_MISSING_TOKEN = -1007;
        public static final int IABHELPER_UNKNOWN_ERROR = -1008;
        public static final int IABHELPER_SUBSCRIPTIONS_NOT_AVAILABLE = -1009;
        public static final int IABHELPER_INVALID_CONSUMPTION = -1010;
     */
    /// <summary>
    /// 아이템 구매 결과 받기
    /// </summary>
    /// <param name="strResult"></param>
    private void InAppBuyItemResult_J(string strResult)
    {
        int iResult = System.Convert.ToInt32(strResult);
        switch (iResult)
        {
            case 0:
                SetLog("InAppBuyItem OK : " + strResult);
                break;
            case -1005:
                SetLog("InAppBuyItem Cancel : " + strResult);
                break;
            default:
                SetLog("InAppBuyItem Failed : " + strResult);
                break;
        }
    }

    private void SetLog(string message)
    {
        Debug.Log(message);
        //m_message.Show(message);
        Log.log(message);
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
                string strOrderId = jData["OrderId"].ToString();
                string strSku = jData["Sku"].ToString();
                string strPurchaseData = jData["purchaseData"].ToString();
                string strSignature = jData["signature"].ToString();
                SetLog(string.Format("InAppConsume Success.\n{0}\n{1}\n{2}\n{3}", strOrderId, strSku, strSignature, strPurchaseData));

                SetInappStatus(strSku, true);
                SendItemInfo(strSku);

                break;
            default:
                Log.log("InAppConsume Failed: " + iResult);
                break;
        }
    }

    /// <summary>
    /// 인앱 결제 창 종료 시점
    /// </summary>
    /// <param name="result"></param>
    private void InAppActivityResult(string result)
    {
        SetLog("InAppActivityResult: " + result);
    }

    /// <summary>
    /// 아이템 구매 정보 이벤트 등록
    /// </summary>
    /// <param name="sku"></param>
    private void SendItemInfo(string sku)
    {
        int index = FindIndex(sku);
        if (index >= 0)
        {
            BillingResultOK((Item)index, sku);
        }
    }

    /// <summary>
    /// 해당 아이디를 가진 아이템 인덱스 찾기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private int FindIndex(string id)
    {
        for (int i = 0; i < m_itemIDs.Length; i++)
        {
            if (id == m_itemIDs[i])
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 리스트에 결제 내역 추가
    /// </summary>
    /// <param name="sku"></param>
    private void AddInappList(string sku)
    {
        int index = FindIndex(sku);
        if (index >= 0)
        {
            inappList.Add((Item)index);
        }
    }

    /// <summary>
    /// 구입 리스트 콜백
    /// </summary>
    /// <param name="sku"></param>
    private void GetInappList(string sku)
    {
        SetLog("GetInappList : " + sku);

        AddInappList(sku);
    }

    public void InAppBuyItem(string strItemId)
    {
        SendItemInfo(strItemId);
        curActivity.Call("InAppBuyItem_U", strItemId);
    }
	#endif

}
