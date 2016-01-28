using UnityEngine;
using System.Collections;

public class PopupWindows : ObjectGroup
{
	public static PopupWindows Instance;

	public ZinWindow[] popupWindows;	
//    public ZinWindow Quit;
//    public ZinWindow Record;
//    public ZinWindow Credits;
//    public ZinWindow Message;
//    public ZinWindow Setup;

    void Awake()
    {
		PopupWindows.Instance = this;

		this.popupWindows = gameObject.GetComponentsInChildren<ZinWindow> ();
		if (this.popupWindows == null)
			return;
    }

	public void ShowWindow(string windowName) {
		ZinWindow window = GetWindow (windowName);
		window.Show ();
	}

	public ZinWindow GetWindow(string windowName) {
		for (int i = 0; i < this.popupWindows.Length; i++) {
			if (this.popupWindows [i].name.Contains (windowName)) {
				return this.popupWindows[i];
			}
		}
		return null;
	}
}
