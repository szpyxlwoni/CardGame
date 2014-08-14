using UnityEngine;
using System.Collections;

[System.Serializable]
public class CardData {
	public int NP = 30;
	public int attack = 10;
	public bool isSelect = false;
	public int id;
	public string category;
}

public class Card : MonoBehaviour {
	public CardData myCard;
	public delegate void ChangeMP(int mp);
	public ChangeMP changeMP;
	public bool IsSelect{
		get {return myCard.isSelect;}
		set {myCard.isSelect = value;}
	}
	public int NP{
		get {return myCard.NP;}
		set {myCard.NP = value;}
	}
	public int Attack{
		get {return myCard.attack;}
		set {myCard.attack = value;}
	}
	public int ID{
		get {return myCard.id;}
		set {myCard.id = value;}
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void selected() {
		Card[] cards = transform.parent.GetComponentsInChildren<Card> ();
		int allNP = 0;
		for (int i = 0; i < cards.Length; i++) {
			if (cards[i].IsSelect && cards[i].ID != ID) {
				allNP += cards[i].NP;
			}
		}
		if (!IsSelect && NP + allNP <= 100) {
			allNP += NP;
			moveCard();
		} else if (IsSelect) {
			moveCard();
		}
		changeMP (allNP);
	}

	private void moveCard() {
		Hashtable forwardhash = new Hashtable ();
		forwardhash.Add ("y", 1.6f);
		forwardhash.Add ("islocal", true);
		Hashtable reservehash = new Hashtable ();
		reservehash.Add ("y", 0);
		reservehash.Add ("islocal", true);
		if (IsSelect) {
			iTween.MoveTo(gameObject, reservehash);
			IsSelect = false;
		} else {
			iTween.MoveTo(gameObject, forwardhash);
			IsSelect = true;
		}
	}

	void OnDestroy() {
		UIGrid cardList = transform.parent.GetComponent<UIGrid> ();
		cardList.Reposition ();
	}
}
