
using UnityEngine;
using System.Collections;
public class Title : MonoBehaviour
{

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Wait to 3 Second for Splash:   " + Time.timeSinceLevelLoad);
        AsyncOperation async = Application.LoadLevelAsync(1);
        Debug.Log("Loading 100% :" + async);
        yield return async;
    }
}