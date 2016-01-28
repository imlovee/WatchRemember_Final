using UnityEngine;
using System.Collections;


/// <summary>
/// Cash로 Package 구입 
/// </summary>
public class BuyPackageAsCash : MonoBehaviour {

	public static BuyPackageAsCash Instance;

	public ZinWindow ListWindow;
	public WindowMessageOK SuccessWindow;
	public WindowShortageCash FailWindow;

	public string packageId;
	public int cash = 0;

	void Awake() {
		BuyPackageAsCash.Instance = this;
	}

	void Start () {
		
	
	}

	public void Buy() {
		if (this.packageId == null)
			return;

		if (GameCash.CashPoint >= cash) {
			GameCash.CashPoint -= cash;

			this.SuccessWindow.Show (this.packageId);

		} else {
			int shortageCash = cash - GameCash.CashPoint;
			this.FailWindow.Show (shortageCash);			
		}
	}
	
}
