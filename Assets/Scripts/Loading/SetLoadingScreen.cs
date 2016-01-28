using UnityEngine;
using System.Collections;

public class SetLoadingScreen : MonoBehaviour
{
    public UILabel percent;
    public UISprite spirte;
    public string prefix;
    public string suffix;

    public UIProgressBar bar;
    private ZinWindow2 window;


    void Awake()
    {
        transform.localScale = Vector3.one;
        window = GetComponent<ZinWindow2>();
    }

    void Start()
    {
        ShowScreen();
    }

    public void ShowScreen()
    {
        window.Show();
    }

    public void HideScreen()
    {
        window.Hide();
    }

    public void SetProgress(float per)
    {
        bar.value = per;
        //percent.text = string.Format("{0}{1}{2}", prefix, per, suffix);
        if (per >= 1)
        {
            StartCoroutine(Hide(0.1f));
        }
    }

    IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideScreen();
    }
}
