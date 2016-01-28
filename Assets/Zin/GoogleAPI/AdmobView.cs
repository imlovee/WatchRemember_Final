using UnityEngine;
using System.Collections;

using GoogleMobileAds.Api;

public class AdmobView : MonoBehaviour
{
    public string c_bannerView_addMobID;
    public string c_interstitial_addMobID;
    private AdSize m_type = AdSize.SmartBanner;
    private AdPosition m_postion = AdPosition.Bottom;

//    BannerView bannerView;
    InterstitialAd interstitial;
    AdRequest request;

    void Start()
    {
        if (!ControlAd.m_useAd)
        {
            return;
        }
		if (string.IsNullOrEmpty(c_interstitial_addMobID)) return;

        Debug.Log("Admob View Start");
//        bannerView = new BannerView(c_bannerView_addMobID, m_type, m_postion);
        request = new AdRequest.Builder().Build();
//        bannerView.LoadAd(request);

        interstitial = new InterstitialAd(c_interstitial_addMobID);
        interstitial.LoadAd(request);

//        bannerView.Show();

        interstitial.AdClosed += AdsClosed;

    }

    public void ShowView()
    {
        if (!ControlAd.m_useAd) return;

        if (GameState.m_playCount % 5 == 0)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
        }
    }

    void AdsClosed(object sender, System.EventArgs e)
    {
        if (interstitial != null)
        {
            interstitial.LoadAd(request);
        }
    }

    public void DestoryAdMob()
    {
//        if (bannerView != null)
//        {
//            bannerView.Hide();
//            bannerView.Destroy();
//        }

        if (interstitial != null)
        {
            interstitial.Destroy();
        }
    }

    void OnApplicationQuit()
    {
        DestoryAdMob();
    }
}
