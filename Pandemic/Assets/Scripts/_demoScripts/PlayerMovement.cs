using UnityEngine;
using System.Linq;
using System.Collections.Generic;

//since we are going to relay info from the script directly inherit from Photon.MonoBehaviour
public class PlayerMovement : Photon.MonoBehaviour {

	private PhotonView PhotonView;

	// Positions
	public Vector3 TargetPosition; //position we want objects that are not our's to end up
	private Quaternion TargetRotation;
    public string TargetParent;
    public int TargetOffset;

    // Role Colour
	public string playerRole;

	// Player Hand
	public List<string> myHand;
	private int handSize = 0;

	// Actions
	public int actionsLeft;
	public int totalActions;
	public bool isCurrentPlayer = false;

	// Action-related buttons
	List<GameObject> actionButtons;
	GameObject endTurnButton;

	private void Awake(){
		PhotonView = GetComponent<PhotonView> ();
        TargetParent = "Atlanta";
  		TargetOffset = -1;
	}


	// Update is called once per frame
	void Update () {
		if (PhotonView.isMine) {	//if person controlling indeed owns this game object
			UpdateRole();
	//			CheckInput ();		//****** check on how & where back-end people did their checks
            RefreshActionPanel ();
		} 
		else {	// not my game object
			UpdateRole ();
			SmoothMove ();

			//Debug.Log ("Client " + PhotonNetwork.player.ID + " actionsLeft = " + actionsLeft);
			//Debug.Log ("Client " + PhotonNetwork.player.ID + " isCurrentPlayer = " + isCurrentPlayer);
		}
	}

	//called everytime you receive a packet, whether its for your object or someone's object
	//Only called if you are "observing " (PhotonView observe option) the PlayerMovement script (this script) 
	//This is not called if you are alone on the server
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting) {			//check if we are sending the data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(TargetOffset);
	        stream.SendNext(transform.parent.name);
	        stream.SendNext(playerRole);
	        stream.SendNext(handSize);
	        foreach (string card in myHand) {
	        	stream.SendNext(card);
	        }
			stream.SendNext (totalActions);
			stream.SendNext (actionsLeft);
			stream.SendNext (isCurrentPlayer);
		} 
		else {							//or we are reading the data
			TargetPosition = (Vector3) stream.ReceiveNext();				//order of casting is important, since you first send Vector3 then Quaternion
			TargetRotation = (Quaternion)stream.ReceiveNext ();
			TargetOffset = (int) stream.ReceiveNext();
	        TargetParent = (string) stream.ReceiveNext();
	        playerRole = (string) stream.ReceiveNext();
	        handSize = (int) stream.ReceiveNext();
	        myHand = new List<string> ();
	        for (int i = 0; i < handSize; i++) {
	        	myHand.Add((string) stream.ReceiveNext());
	        }
			totalActions = (int)stream.ReceiveNext ();
			actionsLeft = (int)stream.ReceiveNext ();
			isCurrentPlayer = (bool)stream.ReceiveNext ();
	    }
	}

	private void SmoothMove(){
        GameObject currentCity = GameObject.Find(TargetParent);
        transform.parent = currentCity.transform;
        transform.localScale = new Vector3(1f, 1f, 1f);
		transform.localPosition = new Vector3(((float) TargetOffset)*10f, 0, 0);
	}

	/* Called from PlayerNetwork
	* Assigns role and initializes Player Hand
	*/
	public void AssignMyRole(string myRole) {
		Debug.Log("AssignMyRole in Playermovement with role = " + myRole);
		playerRole = myRole;
		UpdateRole();
//		switch (myRole) {
//			case "Contingency Planner":
//				GetComponent<UISprite>().color = Color.cyan;
//				break;
//			case "Dispatcher":
//				GetComponent<UISprite>().color = Color.magenta;
//				break;
//			case "Medic":
//				GetComponent<UISprite>().color = Color.yellow;
//				break;
//			case "Operations Expert":
//				GetComponent<UISprite>().color = Color.grey;
//				break;
//			case "Quarantine Specialist":
//				GetComponent<UISprite>().color = Color.green;
//				break;
//			case "Researcher":
//				GetComponent<UISprite>().color = Color.red;
//				break;
//			case "Scientist":
//				GetComponent<UISprite>().color = Color.white;
//				break;
//			default:
//				break;
//		}
//		Update();
	}

	public void UpdateRole() {
		switch (playerRole) {
			case "Contingency Planner":
				GetComponent<UISprite>().color = Color.cyan;
				break;
			case "Dispatcher":
				GetComponent<UISprite>().color = Color.magenta;
				break;
			case "Medic":
				GetComponent<UISprite>().color = Color.yellow;
				break;
			case "Operations Expert":
				GetComponent<UISprite>().color = Color.grey;
				break;
			case "Quarantine Specialist":
				GetComponent<UISprite>().color = Color.green;
				break;
			case "Researcher":
				GetComponent<UISprite>().color = Color.red;
				break;
			case "Scientist":
				GetComponent<UISprite>().color = Color.white;
				break;
			default:
				break;
		}
	}			

	public void ShiftPawn( int offset ) {
		TargetOffset = offset;
		SmoothMove();
	}

	public void AssignCard( string card ) {
		Debug.Log("PlayerMovement : AssignCard with " + card);
		if (card.Contains("(")) {
			card = card.Substring(0, card.IndexOf('('));
		}
		myHand.Add(card);
		handSize++;

		UIGrid PlayerHandPanel = GameObject.Find("UI Root/BelowInfo/BelowPanel/PlayerHand/Scroll View/Grid").GetComponent<UIGrid>();
		GameObject cardObject = GameObject.Find(card);
		//PlayerHandPanel.AddChild(cardObject);
		GameObject go = NGUITools.AddChild(PlayerHandPanel.gameObject, cardObject);
		PlayerHandPanel.AddChild(go.transform);

	}
	

	// Called to remove a card from the Player Hand
	public void RemoveCard( string card ) {
		// Delete card from myHand List
		myHand.Remove(card.Substring(0, card.IndexOf('(')));
		handSize--;
		
		// Destroy the Card gameobject showing in Player Hand
		Destroy(GameObject.Find("PlayerHand/Scroll View/Grid/" + card));
		
		// Get photon view of NetworkManager to call RPC in PlayerNetwork
		PhotonView photonViewTemp = PhotonView.Get(GameObject.Find("_NetworkManager"));
		// Add the card to the respective discard pile
		if (card.Contains("City_")) {	// Card is a Player City Card
			photonViewTemp.RPC("RPC_MasterDiscardCard", PhotonTargets.MasterClient, "PlayerDiscardDeck", card);
			
		} else if (card.Contains("Infection_")) {	// Card is an Infection Card
			photonViewTemp.RPC("RPC_MasterDiscardCard", PhotonTargets.MasterClient, "InfectionDiscardDeck", card);
			
		} else { 						// Card is an Event Card
			photonViewTemp.RPC("RPC_MasterDiscardCard", PhotonTargets.MasterClient, "PlayerDiscardDeck", card);
			
		}
	}
	
	public int GetActionsLeft () {
		return actionsLeft;
	}

	public void SetTotalActions () {
		if (playerRole == "Generalist") {
			totalActions = 5;
		} else {
			totalActions = 4;
		}
	}

	public void IncActions () {
		actionsLeft++;
		PhotonView.RPC ("RPC_UpdateInfoLabelText", PhotonTargets.All, PhotonNetwork.player.NickName, actionsLeft);
	}

	public void DecActions () {
		actionsLeft--;
		PhotonView.RPC ("RPC_UpdateInfoLabelText", PhotonTargets.All, PhotonNetwork.player.NickName, actionsLeft);
	}

	public void ResetActions () {
		actionsLeft = totalActions;
	}

	public void StartTurn () {
		isCurrentPlayer = true;
		ResetActions ();
		RefreshActionPanel ();
		PhotonView.RPC ("RPC_UpdateInfoLabelText", PhotonTargets.All, PhotonNetwork.player.NickName, actionsLeft);	
	}

	public void EndTurn () {
		PhotonView.RPC ("RPC_EndTurn", PhotonTargets.All);
	}

	[PunRPC]
	public void RPC_EndTurn () {
		isCurrentPlayer = false;
		GameObject game = GameObject.Find ("GameManager");
		game.GetComponent<MainGame> ().EndTurn ();
	}

	/**
	 * Cache action and end turn buttons. Only use at init.
	 */
	public void FindActionButtons () {
		Debug.Log ("Client " + PhotonNetwork.player.ID + " PlayerMovement::FindActionButtons()");

		GameObject belowPanel = GameObject.Find ("UI Root/BelowInfo/BelowPanel");

		// find the action buttons
		actionButtons = belowPanel.transform.Cast<Transform> ()
								  .Where (c => c.gameObject.tag == "ActionButton")
								  .Select (c => c.gameObject)
								  .ToList ();

		// find end turn button
		endTurnButton = belowPanel.transform.Find ("SkipIcon").gameObject;

	}

	/**
	* Activates/deactivates action buttons and end turn buttons appropriately
	*/
	public void RefreshActionPanel () {
		
		if (isCurrentPlayer) {
			if (actionsLeft > 0) {
				EnableEndTurnButton ();
				EnableActionButtons ();
			} else {
				EnableEndTurnButton ();
				DisableActionButtons ();
			}
		} else {
			DisableEndTurnButton ();
			DisableActionButtons ();
		}
	}

	private void EnableEndTurnButton () {
		endTurnButton.GetComponent<UIButton> ().enabled = true;
	}

	private void DisableEndTurnButton () {
		endTurnButton.GetComponent<UIButton> ().enabled = false;
	}

	private void EnableActionButtons () {
		foreach (GameObject button in actionButtons) {
			button.GetComponent<UIButton> ().enabled = true;
		}
	}

	private void DisableActionButtons () {
		foreach (GameObject button in actionButtons) {
			button.GetComponent<UIButton> ().enabled = false;
		}	
	}

	/**
	* Update the info label text on all the clients
	*/
	[PunRPC]
	public void RPC_UpdateInfoLabelText (string name, int value) {
		GameObject infoLabel = GameObject.Find ("UI Root/BelowInfo/infoLabel");

		string info = string.Format ("{0}'s turn, {1} actions left", name, value);

		infoLabel.GetComponent<UILabel> ().text = info;	
	}
}