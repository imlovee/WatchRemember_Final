using UnityEngine;
using System.Collections;


/// <summary>
/// 구입 완료 메세지
/// </summary>
[RequireComponent(typeof(ZinWindow))]
public class WindowMessageOK : MonoBehaviour {

	private ZinWindow window;
	public ZinWindow PackageListWin;

//	public UISprite header;
//	public UISprite footer;
	public UILabel lblMsg;
	public UIButton btnOk;

	public string message {
		get {
			return this.lblMsg.text;
		}

		set {
			this.lblMsg.text = value;
		}
	}


	void Awake() {
		this.window = GetComponent<ZinWindow> ();
	}

	void Start () {
	
	}

//	public void SetPosition() {
//		if (this.lblMsg == null)
//			return;
//
//		Vector3[] corners = this.lblMsg.localCorners;
//
//		if (this.header != null) {
//			
//		}
//
//	}


	public void Show(string packageId) {

		PlayerPrefs.SetInt (NameManager.PREF_PACKAGE_BUY_HEADER + packageId, 1);
		PlayerPrefs.Save ();

		this.window.Show (ZinWindow.OpendWindow);
	}

	public void Hide() {
		this.window.Hide (PackageListWin);
	}
}
