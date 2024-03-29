﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StageData {
	public int maxScore = 3;
	public int score = 0;
	public bool enable;
	public string name;
	public int level;
	public float x;
	public float y;
}

public class Stage : MonoBehaviour {
	public StageData myStage;
	public Star star;
	public UIGrid grid;
	public UISprite stageBackground;
	public UISprite iconLocked;
	public delegate void ChangePanel();
	public MainMapController.ChangeToStage changePanel;

	public int MaxScore{
		get {return myStage.maxScore;}
		set {
			myStage.maxScore = value;
			Star[] stars = grid.GetComponentsInChildren<Star>();
			for (int i = stars.Length; i < value; i++) {
				NGUITools.AddChild (grid.gameObject, star.gameObject);
			}
			grid.Reposition ();
		}
	}

	public int Score{
		get {return myStage.score;}
		set {
			myStage.score = value;
			List<Transform> list = grid.GetChildList();
			foreach (Transform t in list) {
				UISprite[] s = t.GetComponentsInChildren<UISprite>();
				s[1].enabled = false;
			}
			for (int i = 0; i < value && i < myStage.maxScore; i++) {
				UISprite[] s = list[i].GetComponentsInChildren<UISprite>();
				s[1].enabled = true;
			}
		}
	}

	public bool Enable {
		get {return myStage.enable;}
		set {
			myStage.enable = value;
			if (value) {
				stageBackground.spriteName = "iconStage_01";
				iconLocked.enabled = false;
			} else {
				stageBackground.spriteName = "iconStage_02";
				iconLocked.enabled = true;
			}
		}
	}

	public string Name {
		get {return myStage.name;}
		set {
			myStage.name = value;
			UILabel stageName = GetComponentInChildren<UILabel>();
			stageName.text = myStage.name;
		}
	}

	public int Level {

		get {return myStage.level;}
		set {myStage.level = value;}
	}

	public float X {
		get {return myStage.x;}
		set {myStage.x = value;}
	}

	public float Y {
		get {return myStage.y;}
		set {myStage.y = value;}
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void loadLevel () {
		if (!Enable) {
			return;
		}
		changePanel ();
	}  

	public void mapJsonToObject (Dictionary<string, object> stageJson) {
		Dictionary<string, object> s = stageJson as Dictionary<string, object>;
		MaxScore = int.Parse(s["maxScore"].ToString());
		Score = int.Parse(s["score"].ToString());
		Name = s["name"].ToString();
		Enable = bool.Parse(s["enable"].ToString());
		Level = int.Parse(s["level"].ToString());
		X = float.Parse(s["x"].ToString());
		Y = float.Parse(s["y"].ToString());
	}
}
