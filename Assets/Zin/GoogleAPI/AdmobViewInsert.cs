using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using GoogleMobileAds.Api;


public class AdmobViewInsert : MonoBehaviour
{
    public string c_addMobID;

    InterstitialAd interstitial;
    AdRequest request;

    void Start()
    {
        if (string.IsNullOrEmpty(c_addMobID)) return;

        interstitial = new InterstitialAd(c_addMobID);
        request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);

        interstitial.AdClosed += AdsClosed;
    }


    public void ShowView()
    {
        //if (GameState.m_playCount % 5 == 0)
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            interstitial.LoadAd(request);
        }
    }


    void AdsClosed(object sender, System.EventArgs e)
    {
        //SoundFX.Instance.PlaySound(SoundType.COUNT);
    }

    public void DestoryAdMob()
    {
        interstitial.Destroy();
    }

    void OnDestroy()
    {
        DestoryAdMob();
    }

    void OnApplicationQuit()
    {
        DestoryAdMob();
    }
}
#endif
