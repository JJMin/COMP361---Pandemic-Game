using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ShareKnowledge : Photon.PunBehaviour {
	public GameObject prefabTakePanel;
	public GameObject prefabRequestPanel;
	PhotonView PhotonView;
	
	void Awake () {
		// Get photonview of this component, used for RPC calls
		PhotonView = GetComponent<PhotonView> ();
	
	}
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// GiveCard button clicked
	public void GiveCardClicked() {
		Debug.Log("GiveCardClicked");
		// Store the player's hand
        GameObject hand = GameObject.Find("PlayerHand/Scroll View/Grid");
        
        // Set up events for cards in hand (make them clickable)
        foreach (Transform card in hand.transform) {
            if (card.tag == "CityCard") {
                foreach (Transform sprite in card.transform) {
                    UIButton button = sprite.GetComponent<UIButton>();
                    EventDelegate onClick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShareKnowledge>(), "giveACard");
                    EventDelegate.Parameter param1 = new EventDelegate.Parameter();
                    EventDelegate.Parameter param2 = new EventDelegate.Parameter();
                    
                    param1.value = card.name;
                    param2.value = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn.GetComponent<PhotonView>().ownerId;
                    
                    param1.expectedType = card.GetComponent<CityCards>().getCity().GetType();
                    param2.expectedType = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn.GetComponent<PhotonView>().ownerId.GetType();
                    
                    onClick.parameters[0] = param1;	// Name of card to give
                    onClick.parameters[1] = param2;	// Name of giving player

                    EventDelegate.Add(button.onClick, onClick);
                }
            }
        }
	}
	
	// Card selected, now need to select player to give card to
	// Then add function to button to call another function "sendRequest" in giving player to target the receiver to ask permission
	private void giveACard(string card, int giving) {
		Debug.Log("giveACard with " + card + " " + giving);
		// Remove events on cards
		GameObject hand = GameObject.Find("PlayerHand/Scroll View/Grid");
		foreach (Transform c in hand.transform) {
            if (c.tag == "CityCard") {
                foreach (Transform sprite in c.transform) {
                	UIButton button = sprite.GetComponent<UIButton>();
                    button.onClick.Clear();
                }
            }
        }
		
		// Get all the pawns in game
		GameObject[] pawns = GameObject.FindGameObjectsWithTag("Pawn");
		
		GameObject myPawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;

		// Put events on them
		foreach (GameObject pawn in pawns) {
			if (pawn == myPawn) {
				Debug.Log("MyPawn");
				UIButton pawnButton = pawn.GetComponent<UIButton>();
				pawnButton.onClick.Clear();
				continue;
			}
			UIButton button = pawn.GetComponent<UIButton> ();
			EventDelegate onClick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShareKnowledge> (), "sendRequest");
			EventDelegate.Parameter param1 = new EventDelegate.Parameter();
			EventDelegate.Parameter param2 = new EventDelegate.Parameter();
			EventDelegate.Parameter param3 = new EventDelegate.Parameter();
			EventDelegate.Parameter param4 = new EventDelegate.Parameter();
			EventDelegate.Parameter param5 = new EventDelegate.Parameter();
			
			param1.value = card;
			param2.value = giving;
			param3.value = "Give";
			param4.value = pawn;
			param5.value = PhotonNetwork.player.ID;
			
			param1.expectedType = card.GetType();
			param2.expectedType = giving.GetType();
			param3.expectedType = ("Give").GetType();
			param4.expectedType = pawn.GetType();
			param5.expectedType = PhotonNetwork.player.ID.GetType();
			
			onClick.parameters[0] = param1;
			onClick.parameters[1] = param2;
			onClick.parameters[2] = param3;
			onClick.parameters[3] = param4;
			onClick.parameters[4] = param5;
			
			EventDelegate.Add(button.onClick, onClick);
		}
	}
	
	// TakeCard button clicked
	public void TakeCardClicked() {
		// Set up events for Pawn
		Debug.Log("TakeCardClicked");
		// Get all the pawns in game
		GameObject[] pawns = GameObject.FindGameObjectsWithTag("Pawn");
		// Get MY pawn
		GameObject myPawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
		
		// Put events on them
		foreach (GameObject pawn in pawns) {
			//if (pawn.GetPhotonView().ownerId == gameObject.GetPhotonView().ownerId) {
			if (pawn == myPawn) {
				Debug.Log("MyPawn");
				UIButton pawnButton = pawn.GetComponent<UIButton>();
				pawnButton.onClick.Clear();
				continue;
			}
			UIButton button = pawn.GetComponent<UIButton> ();
			EventDelegate onClick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShareKnowledge> (), "takeACard");
			EventDelegate.Parameter param1 = new EventDelegate.Parameter();
			EventDelegate.Parameter param2 = new EventDelegate.Parameter();
			
			// Send the player who wants to take
			// Send the player who needs to give
			param1.value = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn.GetComponent<PhotonView>().ownerId;
			param2.value = pawn;
			
			param1.expectedType = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn.GetComponent<PhotonView>().ownerId.GetType();
			param2.expectedType = pawn.GetType();
			
			onClick.parameters[0] = param1;
			onClick.parameters[1] = param2;
			
			EventDelegate.Add(button.onClick, onClick);
		}
	
	}
	
	// Need to show cards 
	private void takeACard(int playerRequesting, GameObject playerGivingPermission) {
		Debug.Log("takeACard with playerRequestion = " + playerRequesting + " playerGivingPermission = " + playerGivingPermission.GetPhotonView().ownerId);
		// Show the Panel 
		
		GameObject panel = Instantiate( prefabTakePanel );
		List<string> hand = playerGivingPermission.GetComponent<PlayerMovement> ().myHand;
		
		UIGrid grid = GameObject.Find("TakeCardPanel(Clone)/panel/Scroll View/Grid").GetComponent<UIGrid>();

		foreach (string card in hand) {
			Debug.Log("Player has " + card);
			GameObject cardObject = GameObject.Find(card);
			GameObject go = NGUITools.AddChild(grid.gameObject, cardObject);
			grid.AddChild(go.transform);
		}
		        
        // Set up events for cards in hand (make them clickable)
        foreach (Transform card in grid.transform) {
            if (card.tag == "CityCard") {
                foreach (Transform sprite in card.transform) {
                    UIButton button = sprite.GetComponent<UIButton>();
                    EventDelegate onClick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShareKnowledge>(), "sendRequest");
                    EventDelegate.Parameter param1 = new EventDelegate.Parameter();
                    EventDelegate.Parameter param2 = new EventDelegate.Parameter();
                    EventDelegate.Parameter param3 = new EventDelegate.Parameter();
					EventDelegate.Parameter param4 = new EventDelegate.Parameter();
					EventDelegate.Parameter param5 = new EventDelegate.Parameter();
			
                    param1.value = card.name;
					param2.value = playerRequesting;	// int
					param3.value = "Take";
					param4.value = playerGivingPermission;
					param5.value = PhotonNetwork.player.ID;
                    
                    param1.expectedType = card.GetType();
					param2.expectedType = playerRequesting.GetType();
					param3.expectedType = ("Take").GetType();
					param4.expectedType = playerGivingPermission.GetType();
					param5.expectedType = PhotonNetwork.player.ID.GetType();
					
					onClick.parameters[0] = param1;
					onClick.parameters[1] = param2;
					onClick.parameters[2] = param3;
					onClick.parameters[3] = param4;
					onClick.parameters[4] = param5;

                    EventDelegate.Add(button.onClick, onClick);
                }
            }
        }
		
		
	}
	
	private void sendRequest(string card, int playerRequesting, string giveOrTake, GameObject playerGivingPermission, int playerRequestingID) {
		Debug.Log("sendRequest with card = " + card + " playerRequesting = " + playerRequesting + " giveOrTake = " + giveOrTake + " playerGivingPersmission = " + playerGivingPermission.GetPhotonView().ownerId);
		
		switch (giveOrTake) {
			// Remove events on pawns if GIVE
			case "Give" :
				GameObject[] pawns = GameObject.FindGameObjectsWithTag("Pawn");
				GameObject myPawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
		
				// Remove events on pawn
				foreach (GameObject pawn in pawns) {
					//if (pawn.GetPhotonView().ownerId == gameObject.GetPhotonView().ownerId) {
					if (pawn == myPawn) {
						UIButton pawnButton = pawn.GetComponent<UIButton>();
						pawnButton.onClick.Clear();
					}
				}
				break;
				
			// Remove events on cards / destroy panel if TAKE
			case "Take" :
				GameObject panel = GameObject.Find("TakeCardPanel(Clone)");
				Destroy(panel);		
        
				break;
			default :
				break;
				
		}
		
		// RPC call to Player
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.ID == playerGivingPermission.GetPhotonView().ownerId) {		
				// Call playerGivingPermission RPC		
				PhotonView.RPC("RPC_Response", player, card, playerRequesting, giveOrTake, playerGivingPermission.GetPhotonView().ownerId, playerRequestingID);
			}
		}
	}
	
	// Sent to the target player for them to then send a response
	[PunRPC]
	private void RPC_Response(string card, int playerRequesting, string giveOrTake, int playerGivingPermission, int playerRequestingID) {
		// Show panel with buttons
		Debug.Log("RPC_Reponse called on playerGivingPermission");
		
		GameObject panel;
		
		
		//Instantiate();
		switch (giveOrTake) {
			case "Give" :
				// Pop up with 2 buttons , accept or reject
				panel = Instantiate( prefabRequestPanel );
				panel.GetComponentInChildren<UILabel>().text = "Player " + playerRequestingID + " wants to give " + card.Substring(0, card.IndexOf('(')) + " to you!";				
				break;
			case "Take" :
				// Pop up with 2 buttons, accept or reject
				panel = Instantiate( prefabRequestPanel );
				panel.GetComponentInChildren<UILabel>().text = "Player " + playerRequestingID + " wants to give " + card.Substring(0, card.IndexOf('(')) + " to you!";				
				break;
			default :
				break;
		}
		
		
		
		
			
			UIButton yes = GameObject.Find("RequestPermissionPanel(Clone)/BtnYes").GetComponent<UIButton> ();
			
			EventDelegate onClickYes = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShareKnowledge> (), "sendResponse");
			
			EventDelegate.Parameter param1 = new EventDelegate.Parameter();
			EventDelegate.Parameter param2 = new EventDelegate.Parameter();
			EventDelegate.Parameter param3 = new EventDelegate.Parameter();
			EventDelegate.Parameter param4 = new EventDelegate.Parameter();
			EventDelegate.Parameter param5 = new EventDelegate.Parameter();			
			EventDelegate.Parameter param6 = new EventDelegate.Parameter();
			
			param1.value = card;
			param2.value = playerRequesting;
			param3.value = giveOrTake;
			param4.value = playerGivingPermission;
			param5.value = playerRequestingID;
			param6.value = true;
			
			param1.expectedType = card.GetType();
			param2.expectedType = playerRequesting.GetType();
			param3.expectedType = giveOrTake.GetType();
			param4.expectedType = playerGivingPermission.GetType();
			param5.expectedType = playerRequestingID.GetType();
			param6.expectedType = (true).GetType();
			
			onClickYes.parameters[0] = param1;
			onClickYes.parameters[1] = param2;
			onClickYes.parameters[2] = param3;
			onClickYes.parameters[3] = param4;
			onClickYes.parameters[4] = param5;
			onClickYes.parameters[5] = param6;
			
			EventDelegate.Add(yes.onClick, onClickYes);
			
			UIButton no = GameObject.Find("RequestPermissionPanel(Clone)/BtnNo").GetComponent<UIButton> ();
			
			EventDelegate onClickNo = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShareKnowledge> (), "sendResponse");
			
			param1 = new EventDelegate.Parameter();
			param2 = new EventDelegate.Parameter();
			param3 = new EventDelegate.Parameter();
			param4 = new EventDelegate.Parameter();
			param5 = new EventDelegate.Parameter();			
			param6 = new EventDelegate.Parameter();
			
			param1.value = card;
			param2.value = playerRequesting;
			param3.value = giveOrTake;
			param4.value = playerGivingPermission;
			param5.value = playerRequestingID;
			param6.value = false;
			
			param1.expectedType = card.GetType();
			param2.expectedType = playerRequesting.GetType();
			param3.expectedType = giveOrTake.GetType();
			param4.expectedType = playerGivingPermission.GetType();
			param5.expectedType = playerRequestingID.GetType();
			param6.expectedType = (false).GetType();
			
			onClickNo.parameters[0] = param1;
			onClickNo.parameters[1] = param2;
			onClickNo.parameters[2] = param3;
			onClickNo.parameters[3] = param4;
			onClickNo.parameters[4] = param5;
			onClickNo.parameters[5] = param6;
			
			EventDelegate.Add(no.onClick, onClickNo);


	}
	
	// Player sends response to give permission
	private void sendResponse(string card, int playerRequesting, string giveOrTake, int playerGivingPermission, int playerRequestingID, bool response) {
		Debug.Log("sendResponse");
		
		// Destroy the request panel
		GameObject requestPanel = GameObject.Find("RequestPermissionPanel(Clone)");
		Destroy(requestPanel);
		
		// Put card in hand, or remove card
		if (response) {
			GameObject myPawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork> ().myPawn;
			switch (giveOrTake) {
				case "Give" :
					// Put card in hand
					myPawn.GetComponent<PlayerMovement>().AssignCard(card);
					
					break;
				case "Take" :
					// Remove card in hand
					myPawn.GetComponent<PlayerMovement>().RemoveCard(card);
					break;
					
				default :
					break;
			}
		} 
		
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.ID == playerRequestingID) {
				PhotonView.RPC("RPC_ReceiveResponse", player, card, playerRequesting, giveOrTake, playerGivingPermission, playerRequestingID, response);
			}
		}
	}
	
	// Tell the request player the response
	[PunRPC]
	private void RPC_ReceiveResponse (string card, int playerRequestion, string giveOrTake, int playerGivingPermission, int playerRequestingID, bool response) {
		Debug.Log("RPC_ReceiveResponse");
		if (response) {
			Debug.Log("Accepted");
			// Player accepted
			GameObject myPawn = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork> ().myPawn;
			// Decrement actions
			myPawn.GetComponent<PlayerMovement> ().DecActions();
			switch (giveOrTake) {
				case "Give" :
					// Remove card from this player
					myPawn.GetComponent<PlayerMovement> ().RemoveCard(card);
					break;
				case "Take" :
					myPawn.GetComponent<PlayerMovement> ().AssignCard(card);
					break;
				default :
					break;
			}
		} else {
			Debug.Log("Refuse");

			// Player refused
			// Show window with message
		}
	
	}
}
