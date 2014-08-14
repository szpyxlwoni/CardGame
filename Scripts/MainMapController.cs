using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMapController : MonoBehaviour {
	public UISprite wayPoint;
	public static float scaleSize = 0.001760563f;
	public Stage stage;
	public TextAsset wayPointsJson;
	public TextAsset stagesJson;
	public UILabel scoreBar;
	public GameObject mapPanel;
	public delegate void ChangeToStage();
	public ChangeToStage changePanel;

	private void initWayPoint ()
	{
		Dictionary<string, object> jsonW = MiniJSON.Json.Deserialize (wayPointsJson.text) as Dictionary<string, object>;
		List<object> wayPoints = jsonW ["arr"] as List<object>;
		for (int i = 0; i < wayPoints.Count; i++) {
			GameObject go = NGUITools.AddChild (mapPanel, wayPoint.gameObject);
			Dictionary<string, object> point = wayPoints [i] as Dictionary<string, object>;
			int x = int.Parse (point ["x"].ToString ());
			int y = int.Parse (point ["y"].ToString ());
			go.transform.position = new Vector3 (x, -y, 0) * scaleSize;
			UIWidget widget = go.GetComponent<UIWidget>();
			widget.depth = 2;
		}
	}

	private List<object> getDataFromJson() {
		string data = PlayerPrefs.GetString ("data");
		Dictionary<string, object> jsonS;
		if (data == null || data == "") {
			jsonS = MiniJSON.Json.Deserialize(stagesJson.text) as Dictionary<string, object>;
		} else {
			jsonS = MiniJSON.Json.Deserialize(data) as Dictionary<string, object>;
		}
		return jsonS["arr"] as List<object>;
	}

	private void enableNextStage(int level) {
		Stage[] stageArray = mapPanel.GetComponentsInChildren<Stage> ();
		for (int i = 0; i < stageArray.Length; i++) {
			if ((level + 1) == stageArray[i].myStage.level) {
				stageArray[i].Enable = true;
			}
		}
	}

	// Use this for initialization
	void Start () {
		int allScore = 0;
		int allMaxScore = 0;
		initWayPoint ();
		List<object> stages = getDataFromJson ();
		for (int i = 0; i < stages.Count; i++) {
			GameObject stageGo = NGUITools.AddChild (mapPanel, stage.gameObject);
			Stage newStage = stageGo.GetComponent<Stage> ();
			newStage.mapJsonToObject(stages[i] as Dictionary<string, object>);
			stageGo.transform.position = new Vector3 (newStage.X, -newStage.Y, 0) * scaleSize;
			newStage.changePanel = changePanel;

			allScore += newStage.Score;
			allMaxScore += newStage.MaxScore;
		}
		scoreBar.text = allScore + "/" + allMaxScore;
		
		PlayerPrefs.DeleteKey("isSuccess");
	}

	public void passStage() {
		Stage[] stages = mapPanel.GetComponentsInChildren<Stage> ();
		for (int i = 0; i < stages.Length; i++) {
			UIToggle toggle = stages[i].GetComponent<UIToggle>();
			if (toggle.value) {
				stages[i].Score = Mathf.Min(new int[]{stages[i].MaxScore, stages[i].Score + 1});
				if (stages[i].MaxScore <= stages[i].Score) {
					enableNextStage(stages[i].Level);
				}
				return;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	}

	void OnDisable() {
		Stage[] stages = mapPanel.GetComponentsInChildren<Stage> ();
		List<Dictionary<string, object>> stageList = new List<Dictionary<string, object>> ();
		for (int i = 0; i < stages.Length; i++) {
			Dictionary<string, object> stageData = new Dictionary<string, object> ();
			stageData.Add("maxScore", stages[i].MaxScore);
			stageData.Add("score", stages[i].Score);
			stageData.Add("enable", stages[i].Enable);
			stageData.Add("name", stages[i].Name);
			stageData.Add("level", stages[i].myStage.level);
			stageData.Add("x", stages[i].myStage.x);
			stageData.Add("y", stages[i].myStage.y);
			stageList.Add(stageData);
		}
		Dictionary<string, List<Dictionary<string, object>>> d = new Dictionary<string, List<Dictionary<string, object>>> ();
		d.Add ("arr", stageList);
		string json = MiniJSON.Json.Serialize(d);
		PlayerPrefs.DeleteKey ("data");
		PlayerPrefs.SetString ("data", json);
	}
}
