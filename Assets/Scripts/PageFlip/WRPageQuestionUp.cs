using UnityEngine;
using System.Collections;

public class WRPageQuestionUp : WRPage
{
    // default
    public UIPanel m_defaultUI;

    public UISprite m_main_img_background;
    public UILabel m_stageLevel;
    public UILabel m_gameLevel;
    public UIButton m_btn_sound;
    public NGUIScore m_totalScore;
    public NGUIScore m_cash;
    //public UIButton m_btn_confirm;

    public UILabel m_cheat;

    public int m_correctAnswer = -1;

    //question
    public UIPanel m_question;
    public UIPanel m_texturePrefab;
    private UITexture[] m_textures;
    public ControlView m_controlView;

    private int m_currentIndex = 0;
    public float Image_Dealy_Time;
    public float Show_Level_Delay_Time;
    public float Ready_Delay_Time;
    public float Go_Delay_Time;


    // answer
    public UIPanel m_answer;

    public CountDown m_lbl_countDown;
    public SetLabelText m_setLabel;


    public void Awake()
    {
        m_totalScore.weight = 30;
    }

    public void SetCountdown(int startCount)
    {
        m_lbl_countDown.m_startCount = startCount;
    }

    public void SetDefaultUI(bool isShow)
    {
        if (m_defaultUI == null) return;

        m_defaultUI.gameObject.SetActive(isShow);
        //m_btn_confirm.gameObject.SetActive(isShow);

        if (ControlCheat.OnCheat)
        {
            m_cheat.gameObject.SetActive(isShow);
        }
        else
        {
            m_cheat.gameObject.SetActive(false);
        }
    }

    public void SetScore(int score, GameObject receive, string functionName)
    {
        m_totalScore.SetScoreAndRun(score, receive, functionName);
    }

    public void SetScore(int score)
    {
        m_totalScore.SetScore(score);
    }

    public void SetCash(int cash)
    {
        m_cash.SetScore(cash);
    }

    public int GetCash()
    {
        return m_cash.Score;
    }

    public void SetPageNo(int pageNo)
    {
        m_stageLevel.text = string.Format("{0} P", pageNo);
    }

    public void SetGameLevel(int gameLv)
    {
        m_gameLevel.text = string.Format("Lv{0}", gameLv);
    }

    public int GetGameLevel()
    {
        int lastLevel = 0;
        if (!int.TryParse(m_gameLevel.text, out lastLevel))
        {
        }

        return lastLevel;
    }

    public void SetQuestionUI(bool isShow, int answerTime)
    {
        if (m_question == null) return;

        m_question.gameObject.SetActive(isShow);
        if (isShow)
        {
            m_lbl_countDown.Init(answerTime, gameObject, "CheckAnswer");
            m_lbl_countDown.Play();
        }
    }

    //public void SetBtnClicked(bool enable)
    //{
    //    m_btn_confirm.enabled = enable;
    //}

    public int GetClearTime()
    {
        return m_lbl_countDown.GetCountTime();
    }

    public int GetCount()
    {
        return m_lbl_countDown.m_currentCount;
    }

    public void StopCountDown()
    {
        Debug.Log("Stop");
        m_lbl_countDown.Stop();
    }

    public void SetTextures(Texture2D[] textures, Texture2D endTexture)
    {
        m_textures = new UITexture[textures.Length + 1];
        for (int i = 0; i < m_textures.Length; i++)
        {
            GameObject go = GameObject.Instantiate(m_texturePrefab.gameObject) as GameObject;
            go.transform.parent = m_answer.transform;
            go.transform.localScale = Vector3.one;
            go.name = string.Format("answer_image_{0}", i);


            UITexture uiTexture = go.GetComponentInChildren<UITexture>();
            if (i == m_textures.Length - 1)
            {
                uiTexture.mainTexture = endTexture;
            }
            else
            {
                uiTexture.mainTexture = textures[i];
            }

            go.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "EndTween"));

            m_textures[i] = uiTexture;
        }
    }


    /// <summary>
    /// 시작 전 대기 화면
    /// </summary>
    public void RunReady()
    {
        StartCoroutine(SetReady());
    }

    private IEnumerator SetReady()
    {
        yield return StartCoroutine(m_setLabel.SetText(string.Format("LEVEL {0}", SetWRPage.m_gameLevel.No), Show_Level_Delay_Time));

        SoundFX.Instance.PlaySound(SoundType.START_DELAY);
        yield return StartCoroutine(m_setLabel.SetText("READY...", Ready_Delay_Time));

        SoundFX.Instance.PlaySound(SoundType.START_DELAY);
        yield return StartCoroutine(m_setLabel.SetText("GO!!", Go_Delay_Time));

        m_setLabel.Hide();
        StartCoroutine(RunTween(0.1f));
    }


    /// <summary>
    /// 이미지 보여주기 tween
    /// </summary>
    private void RunTween()
    {
        if (m_textures == null) return;

        if (m_currentIndex < m_textures.Length)
        {
            Vector3 from = new Vector3(3, 3, 3);
            Vector3 to = Vector3.one;

            UITexture image = m_textures[m_currentIndex];
            UIPanel panel = image.GetComponentInParent<UIPanel>();
            panel.transform.localPosition = new Vector3(0, 667, 0);
            panel.depth = m_defaultUI.depth + m_currentIndex + 1;

            image.GetComponentInParent<ZinTweenScale>().ShowTween(from, to);
            image.GetComponentInParent<ZinTweenAlpha>().ShowTween(0, 1);

            float z = 0;
            if (m_currentIndex < m_textures.Length - 1)
            {
                z = m_currentIndex % 2 == 0 ? Random.Range(0, -5) : Random.Range(0, 5);
                m_question.depth = panel.depth + 1;
            }

            Vector3 rotTo = new Vector3(0, 0, z);
            image.GetComponentInParent<ZinTweenRotation>().ShowTween(Vector3.zero, rotTo);


            m_currentIndex++;
        }
        else
        {
            gameObject.SendMessage("EndQuestionTween");
            //StartCoroutine(EndQuestion(DELAY_TIME));
        }
    }

    void EndTween()
    {
        //Debug.Log("EndTween");
        StartCoroutine(RunTween(Image_Dealy_Time));
    }

    public void StartQuesitionTween()
    {
        m_setLabel.Hide();
        StartCoroutine(RunTween(Image_Dealy_Time));
    }

    IEnumerator RunTween(float sec)
    {
        yield return new WaitForSeconds(sec);
        RunTween();
    }


    //public void SetBtnConfirm(MonoBehaviour behaviour, string functionName)
    //{
    //    m_btn_confirm.onClick.Add(new EventDelegate(behaviour, functionName));
    //}


    public void SetBtnSound(MonoBehaviour behaviour, string functionName)
    {
        m_btn_sound.onClick.Add(new EventDelegate(behaviour, functionName));

        bool isSound = SoundManager.Instance.GetSoundValue();
        m_btn_sound.GetComponent<ChangeOnClick>().SetSprite(isSound);
    }

}
