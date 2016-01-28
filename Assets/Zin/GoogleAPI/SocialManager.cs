using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif

using UnityEngine.SocialPlatforms;
using System.Diagnostics;
using System;


public enum LeaderBoardType
{
	BEST_SCORE = 0,
	BEST_STAGE
}

public enum AchievemType
{
	CLEAR_LEVEL_1 = 0,
	CLEAR_LEVEL_2,
	CLEAR_LEVEL_3,
	CLEAR_LEVEL_4,
	CLEAR_LEVEL_5,
	CLEAR_LEVEL_6,
	CLEAR_LEVEL_7,
	CLEAR_LEVEL_8,
	CLEAR_LEVEL_9,
	CLEAR_LEVEL_10
}


public class SocialManager : MonoBehaviour
{
	public static SocialManager Instance;

	#if UNITY_ANDROID
	public static PlayGamesClientConfiguration GPGConfig;
	#endif

	public bool m_login = false;
	public string[] m_leaderBoardIDsForAndroid;
	public string[] m_leaderBoardIDsForIOS;

	public string[] m_AchievemIDsForAndroid;
	public string[] m_AchievemIDsForIOS;

	//private static AndroidSaveSystem m_saveSystem;


	void Awake ()
	{
		Instance = this;
		//m_saveSystem = AndroidSaveSystem.Instance;

		DontDestroyOnLoad (this);
	}

	void Start ()
	{
		Init ();
	}

	private void Init ()
	{
		#if UNITY_ANDROID
		GPGConfig = new PlayGamesClientConfiguration.Builder ().EnableSavedGames ().Build ();
		PlayGamesPlatform.InitializeInstance (GPGConfig);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();

        print("Init GooglePlay");
		#endif


		Social.localUser.Authenticate (
			(bool success) => {
				print (" - Social:SignIn = " + success);

				if (success) {
					this.m_login = true;
				} else {
					this.m_login = false;
				}
			} 
		);
	}



	void OnApplicationFocus ()
	{
		if (!m_login) {
			Init ();
		}
	}
		

	// call this method form load button
	public void LoadGame ()
	{
		//// user is ILocalUser from Social.LocalUser - will work when authenticated
		//m_saveSystem.LoadSavedGame(Social.localUser, (SaveDataBundle saveBundle) =>
		//{
		//    Log.log("Load SaveGame...");
		//    // do whatever you need with save bundle
		//});
	}

	// call this method from save button
	public void Save (int bestScore, int lastLevel)
	{
		//if (m_saveSystem != null && m_saveSystem.CurrentSave != null)
		//{
		//    SaveDataBundle currentData = m_saveSystem.CurrentSave;
		//    if (bestScore > currentData.m_score || lastLevel > currentData.m_lastLevel)
		//    {
		//        currentData.m_score = bestScore;
		//        currentData.m_lastLevel = lastLevel;

		//        m_saveSystem.SaveGame(currentData, OnLevelSaved);
		//    }
		//}
	}


	private void OnLevelSaved (bool success)
	{
		//// do whatever you need when game has been successfully saved
		//if (success) {
		//    Log.log("SaveLevel");
		//}
	}

	public void SendBoardScore (LeaderBoardType lType, int score)
	{
		if (!m_login)
			return;

		#if UNITY_ANDROID
		string leaderBoardID = m_leaderBoardIDsForAndroid[(int)lType];

		#elif UNITY_IOS
		string leaderBoardID = m_leaderBoardIDsForIOS [(int)lType];

		#else
		string leaderBoardID = m_leaderBoardIDsForAndroid [(int)lType];

		#endif



		Social.ReportScore (score, leaderBoardID, (bool success) => {
			print ("send Score:" + score);
		});
	}

	public void SendAchievem (AchievemType aType, int progress)
	{
		if (!m_login)
			return;

		#if UNITY_ANDROID
		string achievemID = m_AchievemIDsForAndroid[(int)aType];

		#elif UNITY_IOS
		string achievemID = m_AchievemIDsForIOS [(int)aType];

		#else
		string achievemID = m_AchievemIDsForAndroid[(int)aType];

		#endif

		Social.ReportProgress (achievemID, progress, (bool success) => {
			print (string.Format ("{0} Progress {1}%", achievemID, progress));
		});
	}

	public void SetBestScore (int score)
	{
		int bestScore = PlayerPrefs.GetInt (NameManager.PREF_BEST_SCORE, NameManager.PREF_BEST_SCORE_DEFAULT);
		SendBoardScore (LeaderBoardType.BEST_SCORE, score);
		if (score > bestScore) {
			PlayerPrefs.SetInt (NameManager.PREF_BEST_SCORE, score);
			PlayerPrefs.Save ();

			Log.log (string.Format ("Best Score : {0}", score));
		}
	}

	public void SetBestStage (int stage)
	{
		int bestStage = PlayerPrefs.GetInt (NameManager.PREF_BEST_STAGE, NameManager.PREF_BEST_STAGE_DEFAULT);
		if (stage > bestStage) {
			PlayerPrefs.SetInt (NameManager.PREF_BEST_STAGE, stage);
			PlayerPrefs.Save ();

			SendBoardScore (LeaderBoardType.BEST_STAGE, stage);
			Log.log (string.Format ("Best Stage : {0}", stage));
		}
	}

	/// <summary>
	/// 로그인시 베스트 스코어 가져오기 구글 서버에서 값을 가져오도록 수정필요
	/// </summary>
	/// <param name="lType"></param>
	/// <returns></returns>
	public int GetBoardScore (LeaderBoardType lType)
	{
		if (!m_login) {
			return PlayerPrefs.GetInt (NameManager.PREF_BEST_SCORE, NameManager.PREF_BEST_SCORE_DEFAULT);
		} else {
			// 스코어 가져오는 로직 필요
			GetLoadScores (lType);

			return PlayerPrefs.GetInt (NameManager.PREF_BEST_SCORE, NameManager.PREF_BEST_SCORE_DEFAULT);
		}
	}

	/// <summary>
	/// 로그인시 최종 업적 구글에서 가져오기
	/// </summary>
	/// <param name="aType"></param>
	/// <returns></returns>
	public int GetAchievem ()
	{
		GetAchievemList ();

		return 1;
	}




	public void ShowLeaderBoard ()
	{
		print ("ShowLeaderBoard: " + m_login);
		if (!m_login) {
			return;
		}

		Social.ShowLeaderboardUI ();
	}

	public void ShowAchievem ()
	{
		print ("ShowAchievem: " + m_login);
		if (!m_login) {
			return;
		}

		Social.ShowAchievementsUI ();
	}

	private void Logout ()
	{
		if (m_login) {
			#if UNITY_ANDROID
            print("googlePlay logout");
            PlayGamesPlatform.Instance.SignOut();
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                m_login = false;
            }
			#elif UNITY_IOS
			print ("GAME CENTER Logout");

			#endif

		}
	}

	/// <summary>
	/// 업적 리스트 불러오기
	/// </summary>
	private void GetAchievemList ()
	{
		Social.LoadAchievements (achievements => {
			if (achievements.Length > 0) {
				Log.log ("Got " + achievements.Length + " achievement instances");
				string myAchievements = "My achievements:\n";
				foreach (IAchievement achievement in achievements) {
					myAchievements += "\t" +
					achievement.id + " " +
					achievement.percentCompleted + " " +
					achievement.completed + " " +
					achievement.lastReportedDate + "\n";
				}
				Log.log (myAchievements);
			} else {
				Log.log ("No achievements returned");
			}
		});
	}

	/// <summary>
	/// 리더보드 기록 가져오기
	/// </summary>
	private void GetLoadScores (LeaderBoardType lType)
	{
//        Social.LoadScores(m_leaderBoardIDs[(int)lType], scores =>
//        {
//            if (scores.Length > 0)
//            {
//                Log.log("Got " + scores.Length + " scores");
//                string myScores = "Leaderboard:\n";
//                foreach (IScore score in scores)
//                    myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
//                Log.log(myScores);
//            }
//            else
//                Log.log("No scores loaded");
//        });
	}

	void OnApplicationQuit ()
	{
		Logout ();
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        ProcessThreadCollection pt = Process.GetCurrentProcess().Threads;
        foreach (ProcessThread p in pt)
        {
            p.Dispose();
        }

        Process.GetCurrentProcess().Kill();
#endif
	}
}
