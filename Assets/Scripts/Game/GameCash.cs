using UnityEngine;
using System.Collections;

public class GameCash : MonoBehaviour {

	public static int CashPoint
	{
		get {
			return PlayerPrefs.GetInt (NameManager.PREF_CASH_POINT, 0);			
		}

		set {
			PlayerPrefs.SetInt (NameManager.PREF_CASH_POINT, value);
			PlayerPrefs.Save ();
		}
	}

	void Start () {
	}
	
}
