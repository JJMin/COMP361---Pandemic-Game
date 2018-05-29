using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviour {

	[SerializeField]
	private GameObject _playerListingPrefab;
	private GameObject PlayerListingPrefab{
		get { return _playerListingPrefab; }
	}

	private List<PlayerListing> _playerListings = new List<PlayerListing>();
	private List<PlayerListing>  PlayerListings{
		get{ return _playerListings; }
	}

	//call back from PhotonNetwork when someone has joine a room
	private void OnJoinedRoom(){

		foreach( Transform child in transform){		//to destroy all duplicates that would be created if a player is rejoining a room
			Destroy (child.gameObject);
		}

		MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling ();

		PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;

		foreach(PhotonPlayer player in photonPlayers){
			PlayerJoinedRoom (player);
		}
	}

	//Kick out players from a room when master players leaves room
	//called by photon whenever the master client is switched
	private void OnMasterClientSwitched(PhotonPlayer newMasterClient){
			PhotonNetwork.LeaveRoom ();
	}

	//This method syncs player names of players that have joined a room/game on all players in the network
	//This is called by photon when a player joins a room
	private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer){
		PlayerJoinedRoom (photonPlayer);
	}

	//place holder
	private void PlayerJoinedRoom(PhotonPlayer photonPlayer){
		if(photonPlayer == null){
			return;
		}

		PlayerLeftRoom (photonPlayer);	//in case OnjoinedRoom message is sent when player is already in room, this prevents  getting duplicates

		GameObject playerListingObj = Instantiate (PlayerListingPrefab);
		playerListingObj.transform.SetParent (transform, false);

		PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing> ();
		playerListing.ApplyPhotonPlayer (photonPlayer);

		PlayerListings.Add (playerListing);
	}

	//Called by photon when a player leaves a room
	private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		PlayerLeftRoom (photonPlayer);
	}

	//place holder
	private void PlayerLeftRoom(PhotonPlayer photonPlayer){

		int index = PlayerListings.FindIndex ( x => x.PhotonPlayer == photonPlayer);

		if(index != -1){
			Destroy (PlayerListings [index].gameObject);
			PlayerListings.RemoveAt (index);
		}

	}

	//This method is called when the player wants to exit the game
	public void OnClickExitGame(){
		PhotonNetwork.LeaveRoom ();
	}

}

