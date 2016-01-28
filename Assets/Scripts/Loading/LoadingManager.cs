using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public float percent = 0;
    public SetLoadingScreen loadingScreen;

    void Start()
    {
        PackageManager.Instance.onLoaded += Instance_onLoaded;
        PackageManager.Instance.onLoadedBundle += Instance_onLoadedBundle;
        PackageListManager.Instance.onListLoaded += Instance_onListLoaded;

        loadingScreen.SetProgress(0);

    }

    void Instance_onLoadedBundle(int totalCount, int loadCount, string name)
    {
        percent += 1.0f / totalCount;
        
        loadingScreen.SetProgress(Mathf.Round(percent / .01f) * .01f);

        //Debug.Log(loadCount + " - Loading Complete: " + name);
    }

    /// <summary>
    /// 패키지 리스트 로딩 완료시점
    /// </summary>
    /// <param name="loaded"></param>
    void Instance_onListLoaded(bool loaded)
    {
        if (loaded)
        {
            //percent += 1.0f / totalCount;
            //loadingScreen.SetProgress(percent);

            Debug.Log("PacakageList Load Complete : " + percent);
        }
        else
        {
            Debug.LogError("Loading pacakageList Error");
        }
    }


    /// <summary>
    /// 패키지 로딩 완료 시점
    /// </summary>
    /// <param name="loaded"></param>
    void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
            //percent += 80;
            //percent = 100;
            //loadingScreen.SetProgress(percent);
        }
        else
        {
            Debug.LogError("Loading Pakage Error");
        }
    }

}
