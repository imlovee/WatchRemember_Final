using UnityEngine;
using System.Collections;

public class WindowGameOver : SetWindow
{
    public NGUIScore m_totalScore;
    public NGUIScore m_bestScore;
    public NGUIScore m_cash;
    public UIPanel m_correct_img;
    public UISprite m_continue;


    private ControlView m_controlView;
    private SetButtons m_setButtons;
    //public AdmobView m_admob;
    private ZinWindow m_window;


    void Awake()
    {

        m_setButtons = GetComponent<SetButtons>();
        m_window = GetComponent<ZinWindow>();
    }

    void Start()
    {
        InitButtonLink();
		m_controlView = ControlView.Instance;
    }


    /// <summary>
    /// 버튼 클릭시 연결 설정
    /// </summary>
    void InitButtonLink()
    {
        GameObject btnContinue = GetChildObj(gameObject, "Sprite_btn_continue");
        UIButton btnC = btnContinue.GetComponent<UIButton>();
        btnC.onClick.Add(new EventDelegate(UnityAdsManager.Instance, "ShowAd"));


        GameObject btnRecord = GetChildObj(gameObject, "Sprite_btn_record");
        UIButton btnR = btnRecord.GetComponent<UIButton>();
        btnR.onClick.Add(new EventDelegate(SocialManager.Instance, "ShowLeaderBoard"));

        GameObject btnFacebook = GetChildObj(gameObject, "Sprite_btn_fb");
        UIButton btnF = btnFacebook.GetComponent<UIButton>();
        //todo
        btnF.onClick.Add(new EventDelegate(FBManager.Instance, "Share"));
    }


    void Update()
    {
        if (m_window.m_isShow)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MoveMain();
            }
        }
    }

    public void MoveMain()
    {
        m_window.Hide();
        m_controlView.ShowMain();
    }

    public void SetTotalScore(int score)
    {
        m_totalScore.SetScore(score);
    }

    public void SetBestScore(int score)
    {
        m_bestScore.SetScore(score);
    }

	public void SetCashPoint(int score)
	{
		m_cash.SetScore (score);
	}

    public void SetCorrectImg(Texture2D texture)
    {
        UITexture img = m_correct_img.GetComponentInChildren<UITexture>();
        img.mainTexture = texture;
    }

    public void ShowContinueBtn()
    {
        m_setButtons.SetStartPosition();
    }

    public void HideContinueBtn()
    {
        m_setButtons.SetMovePosition();
    }

    public void ContinueGame()
    {
        gameObject.GetComponent<ZinWindow>().Hide();

        SetWRPage.m_stageNo--;
        m_controlView.ShowQuestion(m_totalScore.Score);
    }

    void OpenedWindow(ZinWindow window)
    {
        //m_admob.ShowView();
    }

    void ClosedWindow(ZinWindow window)
    {

    }
}
