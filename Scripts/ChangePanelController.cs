using UnityEngine;
using System.Collections;

public class ChangePanelController : MonoBehaviour {
	public UIPanel mapPanel;
	public UIPanel stagePanel;
	private MainMapController mapCtrl;
	private StageController stageCtrl;
	// Use this for initialization
	void Start () {
		mapCtrl = GetComponent<MainMapController> ();
		mapCtrl.changePanel = changeToStage;
		Debug.Log (mapCtrl.changePanel);
		stageCtrl = GetComponent<StageController> ();
		stageCtrl.changePanel = changeToMap;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void changeToStage() {
		TweenPosition mapTween = mapPanel.GetComponent<TweenPosition>();
		mapTween.PlayForward();
		TweenPosition stageTween = stagePanel.GetComponent<TweenPosition>();
		stageTween.PlayForward();
	}

	public void changeToMap() {
		TweenPosition mapTween = mapPanel.GetComponent<TweenPosition>();
		mapTween.PlayReverse();
		TweenPosition stageTween = stagePanel.GetComponent<TweenPosition>();
		stageTween.PlayReverse();
		mapCtrl.passStage ();
	}
}
