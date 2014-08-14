using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
	public GameObject stagePanel;
	public GameObject mapPanel;
	public UISprite MPbar;
	public Enemy enemy;
	public UIGrid cardList;
	public delegate void ChangeToMap();
	public ChangeToMap changePanel;
	public GameObject cardPrefab;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Card[] cards = stagePanel.GetComponentsInChildren<Card> ();
		initCard(5 - cards.Length);
	}

	public void initCard(int count) {
		for (int i = 0; i < count; i++) {
			GameObject newCard = NGUITools.AddChild (cardList.gameObject, cardPrefab);
			newCard.transform.position = new Vector3 (0, 0, -99f) * MainMapController.scaleSize;
			newCard.transform.RotateAround (newCard.transform.position, Vector3.up, 180);
			Card newCardField = newCard.GetComponent<Card>();
			newCardField.changeMP = changeMP;
			newCardField.ID = Random.Range(1, 1000);
			cardList.AddChild (newCard.transform);
		}
		if (count > 0) {
			cardList.Reposition ();
		}
	}

	public void attackEnemy() {
		Card[] cards = stagePanel.GetComponentsInChildren<Card> ();
		int allAttack = 0;
		for (int i = cards.Length - 1; i >= 0; i--) {
			if (cards[i].IsSelect) {
				allAttack += cards[i].Attack;
				cardList.RemoveChild(cards[i].transform);
				Destroy(cards[i].gameObject);
			}
		}
		enemy.hurt (allAttack);
		if (enemy.HP <= 0) {
			changePanel();
			enemy.HP = 100;
		}
		MPbar.width = 230;
	}

	public void changeMP(int np) {
		MPbar.width = Mathf.CeilToInt(230 * (100f - np) / 100f);
	}

}
