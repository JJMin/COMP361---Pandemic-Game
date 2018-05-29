using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : Photon.MonoBehaviour {

	private PhotonView PhotonView;

	// Decks to synchronize
	public List<string> PlayerDeck;
	public List<string> PlayerDiscardDeck;
	public List<string> InfectionDeck;
	public List<string> InfectionDiscardDeck;
	public List<string> DeletedDeck;

	// Use this for initialization
	void Awake () {
		PhotonView = GetComponent<PhotonView> ();
		if (PhotonNetwork.isMasterClient) {
			Debug.Log("Creating Decks");
			var GameObjectPlayerDeck = new List<GameObject>(GameObject.FindGameObjectsWithTag("CityCard"));
			PlayerDiscardDeck = new List<string>();	
			var GameObjectInfectionDeck = new List<GameObject>(GameObject.FindGameObjectsWithTag("InfectionCityCard"));
			InfectionDiscardDeck = new List<string>();	

			foreach (GameObject card in GameObjectPlayerDeck) {
				Debug.Log("Name of card " + card.name);
				PlayerDeck.Add(card.name);
			}

			foreach (GameObject card in GameObjectInfectionDeck) {
				InfectionDeck.Add(card.name);
			}

			DeletedDeck = new List<string> ();
			Shuffle("PlayerDeck"); 
			Shuffle("InfectionDeck");
		}
	}
	
	// Update is called once per frame
//	void Update () {
//		if (PhotonView.isMine) {
//			
//		} else {
//
//		}
//	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
//		// send deck
//		if (stream.isWriting) {	// only send deck if we are the Master Client
//			if (PhotonNetwork.isMasterClient) {
//				stream.SendNext(PlayerDeck.ToArray());
//				stream.SendNext(PlayerDiscardDeck.ToArray());
//				stream.SendNext(InfectionDeck.ToArray());
//				stream.SendNext(InfectionDiscardDeck.ToArray());
//			}
//		} else {	// receive deck
//			string[] pDeck = stream.ReceiveNext();
//			string[] pDDeck = stream.ReceiveNext();
//			string[] iDeck = stream.ReceiveNext();
//			string[] iDDeck = stream.ReceiveNext();
//			PlayerDeck = new List<string> ( (string[]) stream.ReceiveNext() );
//			PlayerDiscardDeck = new List<string> ( (string[]) stream.ReceiveNext() );
//			InfectionDeck = new List<string> ( (string[]) stream.ReceiveNext() );
//			InfectionDiscardDeck = new List<string> ( (string[]) stream.ReceiveNext() );
//		}
	}

	public void Shuffle(string deck) {
		Debug.Log("Shuffling Deck");
		switch (deck) {
			case "PlayerDeck" :
				for (int i = 0; i < PlayerDeck.Count; i++) {
					string temp = PlayerDeck[i];
					int randomIndex = Random.Range(i, PlayerDeck.Count);
					PlayerDeck[i] = PlayerDeck[randomIndex];
					PlayerDeck[randomIndex] = temp;
				}
				break;
			case "PlayerDiscardDeck" :
				for (int i = 0; i < PlayerDiscardDeck.Count; i++) {
					string temp = PlayerDiscardDeck[i];
					int randomIndex = Random.Range(i, PlayerDiscardDeck.Count);
					PlayerDiscardDeck[i] = PlayerDiscardDeck[randomIndex];
					PlayerDiscardDeck[randomIndex] = temp;
				}
				break;
			case "InfectionDeck" :
				for (int i = 0; i < InfectionDeck.Count; i++) {
					string temp = InfectionDeck[i];
					int randomIndex = Random.Range(i, InfectionDeck.Count);
					InfectionDeck[i] = InfectionDeck[randomIndex];
					InfectionDeck[randomIndex] = temp;
				}
				break;
			case "InfectionDiscardDeck" :
				for (int i = 0; i < InfectionDiscardDeck.Count; i++) {
					string temp = InfectionDiscardDeck[i];
					int randomIndex = Random.Range(i, InfectionDiscardDeck.Count);
					InfectionDiscardDeck[i] = InfectionDiscardDeck[randomIndex];
					InfectionDiscardDeck[randomIndex] = temp;
				}
				break;
			default :
				break;
		}
	}

	public string DrawCard(string deck) {
		string card;
		switch (deck) {
			case "PlayerDeck" :
				card = PlayerDeck[0];
				PlayerDeck.RemoveAt(0);
				return card;
				break;
			case "InfectionDeck" :
				card = InfectionDeck[0];
				InfectionDeck.RemoveAt(0);
				return card;
				break;
			default :
				return null;
				break;
		}
	}
	
	// Called from Player to Master to add a card to the specified discard pile
	//[PunRPC]
	public void MasterDiscardCard(string discardDeck, string card) {
		Debug.Log("RPC_MasterDiscardCard called ");
		switch (discardDeck) {
			case "PlayerDiscardDeck" :
				// Add card to PlayerDiscardDeck
				PlayerDiscardDeck.Add(card);
				break;
			case "InfectionDiscardDeck" :
				// Add card to InfectionDiscardDeck
				InfectionDiscardDeck.Add(card);
				break;
			case "DeletedDeck" :
				// Add card to DeletedDeck
				DeletedDeck.Add(card);
				break;
			default :
				break;
		}
	}

	// Called from PlayerNetwork on MasterClient
	public void Master_AssignHand(PhotonPlayer player) {
		// Update Deck
		// Pass List of cards to player
		int numPlayers = PhotonNetwork.playerList.Length;
		List<string> handToAssign = new List<string> ();
		if (numPlayers == 2) {	// Draw 4 cards
			for (int i = 0; i < 4; i++) {
				handToAssign.Add(DrawCard("PlayerDeck"));
			}
		} else if (numPlayers == 3) {
			for (int i = 0; i < 3; i++) {
				handToAssign.Add(DrawCard("PlayerDeck"));
			}
		} else {
			handToAssign.Add(DrawCard("PlayerDeck"));
		}

		foreach (string cardName in handToAssign) {
			PhotonView.RPC("RPC_AssignCard", player, cardName);
		}
	}

	// Sent to each player
	[PunRPC]
	public void RPC_AssignCard(string cardName) {
		Debug.Log("Assigning card " + cardName);
		var pawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
		pawn.GetComponent<PlayerMovement> ().AssignCard(cardName);

	}

//	[PunRPC]
//	public void RPC_AssignMyHand(List<string> handToAssign) {
//		Debug.Log("Deck : RPC_AssignMyHand");
//		Debug.Log("handToAssign length = " + handToAssign.Count);
//		// get the pawn object and store my hand in PlayerMovement Script
//		var pawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork> ().myPawn;
//		pawn.GetComponent<PlayerMovement> ().AssignHand(handToAssign);
//
//	}

}
