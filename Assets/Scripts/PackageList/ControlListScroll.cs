using UnityEngine;
using System.Collections;

public class ControlListScroll : MonoBehaviour
{
	public UIScrollView scrollView;

	void Awake ()
	{
		

	}

	void ControlView_Instance_changePage (PageType page)
	{
		if (page == PageType.MAIN) {
			this.scrollView.enabled = true;
		} else {
			this.scrollView.enabled = false;
		}
	}

	void Start ()
	{
		ControlView.Instance.changePage += ControlView_Instance_changePage;
	}


}
