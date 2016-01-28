using UnityEngine;
using System.Collections;

public class WRPageMainUp : WRPage
{
    public UIButton m_btn_record;
    public UIButton m_btn_setup;
    public UIButton m_btn_noads;
    public UIButton m_btn_sound;

    public NGUIScore m_bestScore;
    public NGUIScore m_cash;
    public UIButton m_btnCheatUp;
    public UILabel m_lastLevel;
    public UILabel m_bestStage;

    public override void CustomInit()
    {
#if UNITY_ANDROID
        GoogleIABManager.BillingResultOK += new GoogleIABManager.OnBillingResultOK(OnBillingResultOK);
#endif
        if (!ControlAd.m_useAd)
        {
            m_btn_noads.gameObject.SetActive(false);
        }

    }

    public void SetBtnRecord(MonoBehaviour behaviour, string functionName)
    {
        m_btn_record.onClick.Add(new EventDelegate(behaviour, functionName));
    }

    public void SetBtnCheatUp(MonoBehaviour behaviour, string functionName)
    {
        m_btnCheatUp.onClick.Add(new EventDelegate(behaviour, functionName));
    }

    public void SetLastLevel(int lastlevel)
    {
        m_lastLevel.text = string.Format("Lv{0}", lastlevel);
    }

    public void SetBestStage(int bestStage)
    {
        m_bestStage.text = string.Format("{0}P", bestStage);
    }

    public void SetCash(int cash)
    {
        m_cash.SetScore(cash);
    }

    public void SetBestScore(int score)
    {
        m_bestScore.SetScore(score);
    }

    public void SetBtnSetup(MonoBehaviour behaviour, string functionName)
    {
        m_btn_setup.onClick.Add(new EventDelegate(behaviour, functionName));
    }

    public void SetBtnNoAds(MonoBehaviour behaviour, string functionName)
    {
        if (m_btn_noads.gameObject.activeSelf)
        {
            m_btn_noads.onClick.Add(new EventDelegate(behaviour, functionName));
        }
    }

    public void SetBtnSound(MonoBehaviour behaviour, string functionName)
    {
        m_btn_sound.onClick.Add(new EventDelegate(behaviour, functionName));

        bool isSound = SoundManager.Instance.GetSoundValue();
        m_btn_sound.GetComponent<ChangeOnClick>().SetSprite(isSound);
    }

    public void OnBillingResultOK(Item item, string id)
    {
        if (item == Item.NO_ADS)
        {
            m_btn_noads.gameObject.SetActive(false);
        }
    }
}
