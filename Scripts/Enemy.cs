using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyData {
	public int HP = 100;
	public int power = 10;
}

public class Enemy : MonoBehaviour {
	public EnemyData myEnemy;
	public UISlider hpBar;
	public int HP{
		get {return myEnemy.HP;}
		set {myEnemy.HP = value;}
	}
	public int Power{
		get {return myEnemy.power;}
		set {myEnemy.power = value;}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void hurt(int damage) {

		HP -= damage;
		hpBar.value = 1 - (100f - HP) / 100f;
		if (HP <= 0) {
			hpBar.value = 1;
		}
	}
}
