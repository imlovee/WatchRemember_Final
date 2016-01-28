using UnityEngine;
using System.Collections;

#if !UNITY_EDITOR
using GoogleMobileAds.Api;


public class AdmobViewMain : ZinBehaviour
{
    public string c_addMobID;
    public AdSize m_type = AdSize.MediumRectangle;
    public AdPosition m_postion = AdPosition.Top;

    private BannerView bannerView;

    void Start()
    {
        if (string.IsNullOrEmpty(c_addMobID)) return;

        bannerView = new BannerView(c_addMobID, m_type, m_postion);

        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);

        HideView();
    }


    void OpenedWindow(ZinWindow window)
    {
        ShowView();
        Send(true);
    }

    void ClosedWindow(ZinWindow window)
    {
        Send(false);
    }

    public void ShowView()
    {
        bannerView.Show();
    }

    public void HideView()
    {
        bannerView.Hide();
    }

    public void DestoryAdMob()
    {
        HideView();
        bannerView.Destroy();
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