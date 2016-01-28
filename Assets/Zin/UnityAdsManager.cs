using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsManager : ZinBehaviour
{
	public static UnityAdsManager Instance;
	//public static UnityAdsManager Instance
	//{
	//    get
	//    {
	//        if (instance == null)
	//        {
	//            GameObject go = new GameObject("UnityAdsManager");
	//            instance = go.AddComponent<UnityAdsManager>();
	//        }

	//        return UnityAdsManager.instance;
	//    }
	//}
	#if UNITY_ANDROID
	private string id = "61777";
	#elif UNITY_IOS
	private string id = "85555";
	#else
	private string id = "61777";
	#endif

	float DELAY_TIME = 0.1f;

	void Awake ()
	{
		Instance = this;
		if (Advertisement.isSupported) {
			Advertisement.Initialize (id);
		} else {
			Debug.Log ("Platform not supported");
			Log.log ("Platform not supported");
		}

	}

	void Start ()
	{
		PackageManager.Instance.onLoaded += Instance_onLoaded;
	}

	void Instance_onLoaded (bool loaded)
	{

		SetNotify (ControlView.Instance.gameObject, "ContinueQuestion");
	}


	public void ShowAd ()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable && ControlAd.m_useAd) {
			if (Advertisement.IsReady ()) {
				string zone = "defaultZone";
				//string zone = "rewardedVideoZone";

				ShowOptions options = new ShowOptions ();
				options.resultCallback = HandleShowResult;

				Advertisement.Show (zone, options);

				Log.log ("Show Unity Ads");
			}
		} else {
			StartCoroutine (DelaySend (DELAY_TIME));
		}
	}


	void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
                // 광고 본 보상 처리.
			StartCoroutine (DelaySend (DELAY_TIME));

			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			StartCoroutine (DelaySend (DELAY_TIME));
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			StartCoroutine (DelaySend (DELAY_TIME));
			break;
		}
	}
}
