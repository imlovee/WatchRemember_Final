using UnityEngine;
using System.Collections;


public class ControlQuestionPage : MonoBehaviour
{
	public WRPageQuestionUp m_Question_up;
	public WRPageQuestionDown m_Qeustion_down;

	public ControlView m_controlView;
	public ClearMessage m_clearMessage;
	public TimeBonus m_timeBonus;
	public BonusScore m_levelupBonus;
	public ShowSlideSprite m_showLevelupEffect;
	public HideSlideSprite m_hideLevelupEffect;

	public BonusScore m_cashBonus;

	public ShowSlideSprite m_showComboEffect;
	public HideSlideSprite m_hideComboEffect;

	public int m_correctAnswer = -1;
	public Texture2D[] m_answerTextures;

	public int m_ClearScore = 100;
	public int m_ClearCashPoint = 0;
	public int m_StartCountDown = 5;
	//public readonly int ANSWER_TIME = 5;
	public int TIME_BONUS = -1;
	private int m_levelupBonusScore = 0;
	private int m_comboBonusScore = 0;

	private readonly float NEXT_QUESTION_DELAY = 0.5f;

	public delegate void OnSetPage (ControlQuestionPage conPage);

	public event OnSetPage onSetPage;

	/// <summary>
	/// 클리어 단계 개수
	/// </summary>
	private int ClearLevelCount = 5;


	void Awake ()
	{
		TutorialManager.Instance.playingTutorial += Instance_playingTutorial;
	}


	void Instance_playingTutorial (bool isPlaying)
	{
		if (this == null)
			return;

		if (!isPlaying) {
			StartCoroutine (NextQuestion (NEXT_QUESTION_DELAY));
		}
	}

	void Start ()
	{
		TIME_BONUS = m_ClearScore / m_StartCountDown;


	}

	void OnDestory ()
	{
		Debug.Log ("Destroy");
		TutorialManager.Instance.playingTutorial -= Instance_playingTutorial;
	}


	void EventOnSetPage ()
	{
		if (onSetPage != null) {
			onSetPage (this);
		}
	}

	public void SetPage (WRPageQuestionUp pageUp, WRPageQuestionDown pageDown)
	{
		m_Question_up = pageUp;
		m_Qeustion_down = pageDown;

		m_Question_up.SetCountdown (m_StartCountDown);

		EventOnSetPage ();
	}

	/// <summary>
	/// 시작 설정
	/// </summary>
	public void InitStartUI ()
	{
		//m_Question_up.SetBtnConfirm(m_Question_up.m_lbl_countDown, "Stop");
		m_Question_up.SetBtnSound (SoundManager.Instance, "ChangeSoundSetting");

		m_Question_up.SetDefaultUI (true);
		m_Qeustion_down.SetDefaultUI (true);

		m_Question_up.SetQuestionUI (false, m_StartCountDown);
		//m_Qeustion_down.SetButtons(this, "CheckAnswer");
		m_Qeustion_down.SetButtons (m_Question_up, "StopCountDown");
	}

	public void RunQuestionTween ()
	{
		//m_Question_up.RunTween();
		if (ControlView.CurrentStageType == StageType.FIRST || ControlView.CurrentStageType == StageType.CONTINUE_FIRST) {
			m_Question_up.RunReady ();
		} else {
			m_Question_up.StartQuesitionTween ();
		}
	}




	void EndQuestionTween ()
	{
		SetQuestionUI (true);

		//TutorialManager.Instance.SetAnswer(m_correctAnswer);
		Vector3 pos = m_Qeustion_down.GetAnswerPosition (m_correctAnswer);
		TutorialManager.Instance.SetAnswer (pos);
		TutorialManager.Instance.PlayTutorial ();
	}

	public void SetQuestionUI (bool isShow)
	{
		m_Question_up.SetQuestionUI (isShow, m_StartCountDown);
		m_Qeustion_down.SetQuestionUI (isShow);
	}


	public void CheckAnswer ()
	{
		//m_Question_up.SetBtnClicked(false);

		int confirmIndex = m_Qeustion_down.GetSelectAnswer ();

#if UNITY_EDITOR
		Debug.Log (string.Format ("Select: {0}, CorrectAnswer: {1}", confirmIndex, m_correctAnswer));
#endif
		if (confirmIndex == m_correctAnswer) {
			Debug.Log ("Clear");
			ClearStage ();
		} else {
			if (!TutorialManager.Instance.isPlaying) {
				Debug.Log ("GameOver");
				GameOver ();
			}
		}
	}

	/// <summary>
	/// 정답 맞췄을때
	/// </summary>
	private void ClearStage ()
	{

		if (TutorialManager.Instance.isPlaying) {
			TutorialManager.Instance.CloseWindow ();
		}

		//int clearTime = m_Question_up.GetClearTime();
		int time = m_Question_up.GetCount ();

		SoundFX.Instance.PlaySound (SoundType.STAGE_CLEAR);
		m_clearMessage.m_notify = gameObject;
		m_clearMessage.m_functionName = "EndClearMessage";
		m_clearMessage.ShowMessage (time / (m_StartCountDown / ClearLevelCount));

		//m_timeBonus.Show()

	}


	/// <summary>
	/// clear message show tween 종료시점
	/// </summary>
	private void EndClearMessage ()
	{

		if (m_clearMessage.perfectCount > 1) {
			StartCoroutine (CheckCombo (0.1f, m_clearMessage.perfectCount));
		} else {
			m_comboBonusScore = 0;
			StartCoroutine (CheckLevelUp (0.1f));
			StartCoroutine (m_clearMessage.HideMessage (1f));
		}
        
	}

	IEnumerator CheckCombo (float delay, int comboCount)
	{
		yield return new WaitForSeconds (delay);

		m_showComboEffect.SetNotify (gameObject, "EndShowComboEffect");
		m_hideComboEffect.SetNotify (gameObject, "EndHideComboEffect");

		int scoreCount = comboCount - 1;
		m_comboBonusScore = scoreCount * m_ClearScore;
		//m_comboBonus.Show(score, string.Format("{0} COMBO +", comboCount));

		m_showComboEffect.GetComponent<UILabel> ().text = string.Format ("{0} COMBO +{1}", comboCount, m_comboBonusScore);
		m_showComboEffect.Show ();
	}

	private void EndShowComboEffect ()
	{
		StartCoroutine (CheckLevelUp (0.1f));
		StartCoroutine (m_clearMessage.HideMessage (1f));

		m_hideComboEffect.startDelay = 1f;
		m_hideComboEffect.Hide ();
	}

	private void EndHideComboEffect ()
	{
	}

	/// <summary>
	/// 레벨업 여부확인
	/// </summary>
	/// <param name="delay"></param>
	/// <returns></returns>
	IEnumerator CheckLevelUp (float delay)
	{
		yield return new WaitForSeconds (delay);

		int levelCount = StageList.Instance.m_stageCount;
		int stageNo = SetWRPage.m_stageNo;
		if (stageNo % levelCount == 0) {
			m_levelupBonusScore = GetLevelUpBonus ();
			ShowLevelEffect ();
			ControlView.CurrentStageState = StageState.LEVEL_UP;
		} else {
			m_levelupBonusScore = 0;
			SetScores ();
		}
	}

	private void ShowLevelEffect ()
	{
		m_showLevelupEffect.m_notify = gameObject;
		m_showLevelupEffect.m_functionName = "EndShowLevelEffect";

		m_showLevelupEffect.Show ();

		StartCoroutine (PlayLevelUpSound (m_showLevelupEffect.startDelay));
	}

	IEnumerator PlayLevelUpSound (float delay)
	{
		yield return new WaitForSeconds (delay);
		SoundFX.Instance.PlaySound (SoundType.LEVEL_UP);
	}

	/// <summary>
	/// 레벨 업 효과 종료 시점
	/// </summary>
	private void EndHideLevelEffect ()
	{
		m_levelupBonus.Hide ();
		SetScores ();
	}

	/// <summary>
	/// 레벨 업 효과 가운데 위치 시점
	/// </summary>
	private void EndShowLevelEffect ()
	{
		m_levelupBonus.m_notify = gameObject;
		m_levelupBonus.m_functionName = "EndLevelupBonus";

		m_levelupBonus.Show (GetLevelUpBonus (), "+");
        
		// 캐쉬 포인트 추가
		m_ClearCashPoint = SetWRPage.m_gameLevel.ClearCashPoint;
		m_cashBonus.Show (m_ClearCashPoint, "+");
        
	}

	/// <summary>
	/// 레벨업 보너스 점수 증가 tween 종료 시점
	/// </summary>
	private void EndLevelupBonus ()
	{
		m_hideLevelupEffect.m_notify = gameObject;
		m_hideLevelupEffect.m_functionName = "EndHideLevelEffect";

		m_hideLevelupEffect.Hide ();

		m_cashBonus.Hide ();
	}

	private int GetLevelUpBonus ()
	{
		return SetWRPage.m_gameLevel.No * 100;
	}

	private void EndTimeBonus ()
	{
		int time = m_Question_up.GetCount ();

		SetScore (time);
		SetCashPoint ();

		SetAchievem ();
		SetBestStage ();
	}

	private void SetCashPoint ()
	{
		if (m_ClearCashPoint > 0) {
			int cashPoint = GameCash.CashPoint + m_ClearCashPoint;
			m_Question_up.SetCash (m_Question_up.GetCash () + m_ClearCashPoint);

			GameCash.CashPoint = cashPoint;
		}
	}


	/// <summary>
	/// 점수 계산
	/// </summary>
	private void SetScores ()
	{
		int time = m_Question_up.GetCount ();

		int timeBonus = time * TIME_BONUS;
		m_timeBonus.SetNotify (gameObject, "EndTimeBonus");
		m_timeBonus.Show (timeBonus);

	}

	private void SetBestStage ()
	{
		SocialManager.Instance.SetBestStage (SetWRPage.m_stageNo);
	}



	/// <summary>
	/// 틀렸을때
	/// </summary>
	private void GameOver ()
	{
		m_clearMessage.perfectCount = 0;
		m_controlView.ShowGameOver (m_Question_up.m_totalScore.Score);
	}

	private void SetScore (int remainTime)
	{
		int lastScore = m_Question_up.m_totalScore.Score;

		int timeBonus = remainTime * TIME_BONUS;

		int score = lastScore + m_ClearScore + timeBonus + m_levelupBonusScore + m_comboBonusScore;

		m_Question_up.SetScore (score, gameObject, "EndScoreAni");

		SocialManager.Instance.SetBestScore (score);
	}

	/// <summary>
	/// 업적 클리어 여부 판단
	/// </summary>
	private void SetAchievem ()
	{
		int levelCount = StageList.Instance.m_stageCount;
		//int lastStageNo = levelCount - 1;
		int stageNo = SetWRPage.m_stageNo;
		int stageLevel = SetWRPage.m_gameLevel.No;


		if (stageNo % levelCount == 0) {
			AchievemType aType = (AchievemType)stageLevel;
			SocialManager.Instance.SendAchievem (aType, 100);
			Debug.Log (string.Format ("Complete Mission: {0}", aType));
		}

	}


	void EndScoreAni ()
	{
		if (!TutorialManager.Instance.PlayTutorial ()) {
			StartCoroutine (NextQuestion (NEXT_QUESTION_DELAY));
		}
	}



	public void NextQuestion ()
	{
		m_timeBonus.Hide ();

		if (StageList.Instance.LastStage) {
			m_controlView.ShowGameClear(m_Question_up.m_totalScore.Score);

		} else {
			m_controlView.ShowQuestion (m_Question_up.m_totalScore.Score);
		}
	}

	IEnumerator NextQuestion (float delaySec)
	{
		yield return new WaitForSeconds (delaySec);
		//m_clearMessage.HideMessage();
		Debug.Log (delaySec);
		NextQuestion ();

	}
}
