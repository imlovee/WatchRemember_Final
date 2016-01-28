using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SetWRPage : MonoBehaviour
{
    public static SetWRPage Instance;

    public WRPage m_wrPage_up;
    public WRPage m_wrPage_down;

    public QuestionImages m_questionImages;
    public ClearMessage m_clearMessage;
    public TimeBonus m_timeBonus;
    public BonusScore m_levelUp;
    public BonusScore m_cash;
    public GoogleIABManager m_googleInapp;

    public LevelUp levelUpEffect;
    public Combo comboEffect;

    public ControlView m_controlView;

    public Texture2D m_correct;

    //private StageQuestion m_stage;
    public static GameLevel m_gameLevel;
    private StageList m_stageList;

    public static int m_stageNo = 0;
    //public static int LEVEL = -1;

    void Awake()
    {
        Instance = this;
#if UNITY_ANDROID
        m_googleInapp = GoogleIABManager.Instance;
#endif

        m_wrPage_up = null;
        m_wrPage_down = null;

        PackageManager.Instance.onLoaded += Instance_onLoaded;

    }

    void Instance_onLoaded(bool loaded)
    {
        if (loaded)
        {
        }
    }

    void Start()
    {
        m_stageList = StageList.Instance;

        InitStageNumber();
    }



    /// <summary>
    /// 최종 레벨 저장
    /// </summary>
    /// <param name="level"></param>
    private void SaveLastLevel(int level)
    {
        int oldLastLevel = PlayerPrefs.GetInt(NameManager.PREF_LAST_LEVEL, NameManager.PREF_LAST_LEVEL_DEFAULT);
        if (level > oldLastLevel)
        {
            PlayerPrefs.SetInt(NameManager.PREF_LAST_LEVEL, level);
            PlayerPrefs.Save();
        }
    }


    //private void SetLevel()
    //{
    //    Debug.Log(m_stageList.m_stageCount);
    //    LEVEL = m_stageList.m_stageCount < 0 ? 1 : m_stageNo / m_stageList.m_stageCount;
    //    //LEVEL = LEVEL >= m_stageList.m_stageCount ? m_stageList.m_stageCount - 1 : LEVEL;
    //}

    public IEnumerator SetQuestion()
    {
        //SetLevel();

        //m_stage = m_stageList.GetStage(LEVEL);
        
        m_gameLevel = m_stageList.GetLevel(m_stageNo);
        
        //LEVEL = m_gameLevel.No;
        Debug.Log("Lv : " + m_gameLevel.No);
        StageList.Instance.m_stageCount = m_gameLevel.StageCount;
        if (m_gameLevel != null)
        {
            WRPageQuestionUp pageQ_up = (WRPageQuestionUp)m_wrPage_up;
            WRPageQuestionDown pageQ_down = (WRPageQuestionDown)m_wrPage_down;

            pageQ_up.name = string.Format("Question_up_{0}", m_stageNo);
            pageQ_down.name = string.Format("Question_down_{0}", m_stageNo);

			pageQ_up.SetCash(GameCash.CashPoint);

            ControlQuestionPage controlQ = pageQ_up.gameObject.AddComponent<ControlQuestionPage>();
            controlQ.m_clearMessage = m_clearMessage;
            controlQ.m_StartCountDown = m_gameLevel.CountDown;
            controlQ.SetPage(pageQ_up, pageQ_down);
            controlQ.InitStartUI();
            controlQ.m_controlView = m_controlView;
            controlQ.m_timeBonus = m_timeBonus;
            controlQ.m_levelupBonus = m_levelUp;
            controlQ.m_showLevelupEffect = levelUpEffect.showSlide;
            controlQ.m_hideLevelupEffect = levelUpEffect.hideSlide;
            controlQ.m_showComboEffect = comboEffect.showSlide;
            controlQ.m_hideComboEffect = comboEffect.hideSlide;
            controlQ.m_ClearScore = m_gameLevel.ClearScore;
            //controlQ.m_ClearCashPoint = m_gameLevel.ClearCashPoint;
            controlQ.m_cashBonus = m_cash;

            m_questionImages.ClearQList();
            m_questionImages.GetExtraImages(m_gameLevel.ImageCount);

            pageQ_up.SetPageNo(m_stageNo + 1);
            pageQ_up.SetGameLevel(m_gameLevel.No);

            SaveLastLevel(m_gameLevel.No);

            List<Texture2D> list = new List<Texture2D>(m_questionImages.m_QImagesList);
            pageQ_up.m_controlView = m_controlView;
            pageQ_up.Image_Dealy_Time = m_gameLevel.DelayTime;

            int correctIndex = Random.Range(0, list.Count);
            m_correct = list[correctIndex];

            m_questionImages.ClearQList();
            pageQ_up.SetTextures(list.ToArray(), m_questionImages.m_EndTexture);

            // 대답입력

            List<Texture2D> answerList = m_questionImages.GetExtraImages(3, ref list);

            controlQ.m_correctAnswer = Random.Range(0, answerList.Count + 1);
            //pageA_up.m_cheatLabel.SetText((m_confirm.m_correctAnswer + 1).ToString());
            answerList.Insert(controlQ.m_correctAnswer, m_correct);

            if (ControlCheat.OnCheat)
            {
                pageQ_up.m_cheat.text = (controlQ.m_correctAnswer + 1).ToString();
            }

            pageQ_down.SetAnswerImages(answerList);

            list.Clear();
            list = null;

            answerList.Clear();
            answerList = null;


            //yield return StartCoroutine(LoadImage(ImageType.QUESTION_IMAGE, m_stage.Image_name, pageQ_up.m_questionImg));
        }

        SetStageCompleted(PageType.QUESTION);

        SetStageNumber();

        yield return null;
    }

    private IEnumerator SetMain()
    {
        WRPageMainUp pageM_up = (WRPageMainUp)m_wrPage_up;
        WRPageMainDown pageM_down = (WRPageMainDown)m_wrPage_down;

        pageM_down.InitStartBtn(m_controlView, "Reset");
        pageM_up.SetBtnRecord(m_controlView.m_recordWindow, "Show");
        pageM_up.SetBtnSetup(m_controlView.m_setupWindow, "Show");
        pageM_up.SetBtnSound(SoundManager.Instance, "ChangeSoundSetting");

		#if UNITY_ANDROID
//        pageM_up.SetBtnNoAds(m_googleInapp, "BuyNoAds");
		#elif UNITY_IOS 
//		pageM_up.SetBtnNoAds(IOSManager.Instance, "BuyNoAds");
		#endif

        //int lastLevel = GooglePlay.Instance.GetAchievem();
        int lastLevel = PlayerPrefs.GetInt(NameManager.PREF_LAST_LEVEL, NameManager.PREF_LAST_LEVEL_DEFAULT);
        pageM_up.SetLastLevel(lastLevel);

        int bestStage = PlayerPrefs.GetInt(NameManager.PREF_BEST_STAGE, NameManager.PREF_BEST_STAGE_DEFAULT);
        SocialManager.Instance.SetBestStage(bestStage);
        pageM_up.SetBestStage(bestStage);

		pageM_up.SetCash(GameCash.CashPoint);

        pageM_up.SetBtnCheatUp(ControlCheat.Instance, "Click1");
        pageM_down.SetBtnCheatDown(ControlCheat.Instance, "Click2");

        int bestScore = GetBestScore();
        pageM_up.SetBestScore(bestScore);
        SocialManager.Instance.SetBestScore(bestScore);

        //m_pageM_up.SetBtnAssessment(m_controlView.m_recordWindow, "Show");
        //m_pageM_up.SetBtnNoAds(m_controlView.m_recordWindow, "Show");

        yield return null;

        SetStageCompleted(PageType.MAIN);
    }

    /// <summary>
    /// 최고기록 가져오기
    /// </summary>
    /// <returns></returns>
    private int GetBestScore()
    {
        return PlayerPrefs.GetInt(NameManager.PREF_BEST_SCORE, NameManager.PREF_BEST_SCORE_DEFAULT);
    }

    private void SetStageCompleted(PageType pageType)
    {
        m_wrPage_up = null;
        m_wrPage_down = null;

        m_controlView.SetStaged(pageType);
    }

    /// <summary>
    /// WRPage 생성시 페이지 값 채우기
    /// </summary>
    /// <param name="page"></param>
    public void SetPage(WRPage page)
    {

        if (page.m_pageIndex == QuestionPageIndex.UP)
        {
            m_wrPage_up = page;
        }
        else if (page.m_pageIndex == QuestionPageIndex.DOWN)
        {
            m_wrPage_down = page;
        }

        if (m_wrPage_up != null && m_wrPage_down != null)
        {
            if (page.m_pageType == QuestionPageType.MAIN_UP || page.m_pageType == QuestionPageType.MAIN_DOWN)
            {
                StartCoroutine(SetMain());
            }
            else if (page.m_pageType == QuestionPageType.QUESTION_UP || page.m_pageType == QuestionPageType.QUESTION_DOWN)
            {
                StartCoroutine(SetQuestion());
            }
        }
    }


    /// <summary>
    /// 스테이지 번호 및 레벨 계산
    /// </summary>
    private void SetStageNumber()
    {
        m_stageNo++;
    }

    public void InitStageNumber()
    {
        m_stageNo = 0;
    }

    /// <summary>
    /// 계속하기시에 스테이지 번호 설정
    /// </summary>
    public void SetContinueStageNumber()
    {
        m_stageNo--;
    }

    void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
