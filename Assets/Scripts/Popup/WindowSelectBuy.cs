using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ZinWindow))]
public class WindowSelectBuy : MonoBehaviour {

	public string buyId;
	public string packageID;

	public UILabel lbl_usd;
	public UILabel lbl_cash;

	public UIButton btn_usd;
	public UIButton btn_cash;


	private BuyPackageAsCash buyPackage;
	private BuyPackageAsMoney buyPackageMoney;
	private readonly string usd_suffix = "$";

	public string USD {
		get {
			return this.lbl_usd.text;
		}
		set {
			this.lbl_usd.text = value + usd_suffix;
		}
	}

	private int cash = 0;
	public int Cash{
		get{
			return cash;
		}
		set {
			this.cash = value;
			this.lbl_cash.text = cash.ToString ();
		}
	}

	private ZinWindow window;


	void Awake() {
	}

	void Start () {
		this.window = GetComponent<ZinWindow> ();

		this.buyPackage = BuyPackageAsCash.Instance;
		this.buyPackageMoney = BuyPackageAsMoney.Instance;
	}

	public void Show(string packageName, string buy_id, string USD, int cash, Purchaser p) {
		this.packageID = packageName;
		this.buyId = buy_id;
		this.USD = USD;
		this.Cash = cash;

		// cash 이벤트 등록

		if (this.USD == null || this.USD == this.usd_suffix) {
			this.btn_usd.gameObject.SetActive (false);	
		} else {
			this.btn_usd.gameObject.SetActive (true);

			this.buyPackageMoney.buyId = this.buyId;

			EventDelegate e;

			// 유료 결제 이벤트 등록
//			#if UNITY_ANDROID
//			e = new EventDelegate(GoogleIABManager.Instance, "Buy");
//
//			#elif UNITY_IOS 
//			e = new EventDelegate(IOSManager.Instance, "BuyItem");
//
//			#endif

//			e = new EventDelegate(p, "BuyNonConsumable");

//			e.parameters [0].value = this.buyId;
//			this.btn_usd.onClick.Add (e);

			this.btn_usd.onClick.Add (new EventDelegate (p, "BuyNonConsumable"));
		}

		if (this.cash < 0) {
			this.btn_cash.gameObject.SetActive (false);
		} else {
			this.btn_cash.gameObject.SetActive(true);

			this.buyPackage.packageId = this.packageID;
			this.buyPackage.cash = this.cash;
		}

		this.window.Show (ZinWindow.OpendWindow);
	}

}
