using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ZinWindow))]
public class WindowShortageCash : MonoBehaviour {
	private ZinWindow window;

	public ZinWindow PackageListWin;

	public UILabel lbl_cash;

	public string Cash {
		set {
			if (lbl_cash != null) {
				lbl_cash.text = value;
			}
		}
	}

	void Start () {
		this.window = GetComponent<ZinWindow> ();
	}

	public void Show(int shortageCash) {
		this.Cash = string.Format("-{0}", shortageCash.ToString ());

		this.window.Show (ZinWindow.OpendWindow);
	}
	
}
