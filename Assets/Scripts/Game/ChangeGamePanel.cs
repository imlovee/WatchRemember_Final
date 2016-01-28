using UnityEngine;
using System.Collections;

public class ChangeGamePanel : MonoBehaviour
{
	public GameObject Dummy;
	public GameObject GamePanel;
	public UIScrollView scrollView;
	public UIPanel uiPanel;

	public Transform[] children;

	public int startPos;
	public int gamePanelPos = int.MinValue;


	void Start ()
	{
		uiPanel = GetComponent<UIPanel> ();
		children = GamePanel.GetComponentsInChildren<Transform> ();

		scrollView.onStoppedMoving += OnStopMove;
		scrollView.onDragStarted += OnStart;
		scrollView.onMomentumMove += OnMove;

	}

	void OnStart ()
	{
		int posx = (int)transform.localPosition.x;
		if (gamePanelPos == int.MinValue) {
			gamePanelPos = posx;
		}
		startPos = posx;
		Debug.Log (startPos);
	}

	void OnMove() {
	}

	void OnStopMove ()
	{
		int posx = (int)transform.localPosition.x;
		if (gamePanelPos != posx && startPos != posx) {
			ShowDummy ();
		} else if (gamePanelPos == posx && startPos != posx) {
			ShowGamePanel ();
		}
		Debug.Log ("Scroll end");

	}

	public void ShowDummy ()
	{
		for (int i = 0; i < children.Length; i++) {
			if (children [i].gameObject != GamePanel) {
				children [i].gameObject.SetActive (false);
			}
		}
		Dummy.SetActive (true);
//		gameObject.transform.parent.localPosition = new Vector3 (-150, 0, 0);

	}

	public void ShowGamePanel ()
	{
		for (int i = 0; i < children.Length; i++) {
			if (children [i].gameObject != GamePanel) {
				children [i].gameObject.SetActive (true);
			}
		}

		Dummy.SetActive (false);
//		gameObject.transform.parent.localPosition = new Vector3 (0, 0, 0);
	}
	
}
