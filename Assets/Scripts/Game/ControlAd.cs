using UnityEngine;
using System.Collections;



public class ControlAd : MonoBehaviour
{
	public static bool m_useAd = true;
	public UIPanel m_totalPanel;
	public AdmobView m_admob;

	public Vector3 m_useADPosition;
	public Vector3 m_useADScale;

	public Vector3 m_defaultScale = new Vector3 (0.95f, 0.95f, 0);

	//public bool Test = false;

	void Awake ()
	{
		#if UNITY_ANDROID 
        GoogleIABManager.BillingResultOK += new GoogleIABManager.OnBillingResultOK(OnBillingResultOK);
//        m_useAd = GoogleIABManager.Instance.IsBuy(Item.NO_ADS);
		m_useAd = GoogleIABManager.Instance.GetInappStatus(Item.NO_ADS);
		#elif UNITY_IOS
		m_useAd = PlayerPrefs.GetInt (NameManager.PREF_PACKAGE_BUY_HEADER + NameManager.PREF_BUY_NOADS, 0) == 0 ? true : false;
		#else
        m_useAd = true;
		#endif
       
		//m_useAd = false;
	}

	void Start ()
	{
		SetAds ();

	}

	private void OnBillingResultOK (Item item, string id)
	{
		Debug.Log ("OnBillingResultOK: " + item.ToString ());
		Log.log ("OnBillingResultOK: " + item.ToString ());
		if (item == Item.NO_ADS) {
			//PlayerPrefs.SetInt(PrefNameManager.PREF_INAPP_NOADS, 0);
			//PlayerPrefs.Save();

			if (m_useAd) {
				SetAds ();
			} else {
				SetAds ();
			}
		}
	}

	//void Update()
	//{
	//    if (Test)
	//    {
	//        Test = false;
	//        OnBillingResultOK(Item.NO_ADS);
	//        m_useAd = true;
	//    }
	//}

	public void SetScreen ()
	{
		if (m_useAd) {
			m_totalPanel.transform.localPosition = m_useADPosition;
			//m_totalPanel.transform.localScale = m_useADScale;


		} else {
			m_totalPanel.transform.localPosition = new Vector3 (150, 0, 0);
			//m_totalPanel.transform.localScale = m_defaultScale;
		}
	}

	private void SetAds ()
	{
//		SetScreen ();

		if (!m_useAd) {
			m_admob.DestoryAdMob ();
		}
	}


}


