using UnityEngine;
using UnityEngine.SceneManagement;

//Script used for starting a game
//This is attached to the currentRoom GameObject
public class CurrentRoomCanvas : MonoBehaviour {

	public PhotonView PhotonView;

	//this just starts the game scene
	public void OnClickStartGame(){

		if(!PhotonNetwork.isMasterClient){			// can also add "&& PhotonNetwork.playerList > 1" so that the game starts with at least 2 players
			return;
		}

		if(PhotonNetwork.room.PlayerCount < 3){	//at least 3 players needed to start the game
			return;												/**************************REMEMBER TO UNCOMMENT THIS ********************************/
		}

		PhotonNetwork.room.IsOpen = false;
		PhotonNetwork.room.IsVisible = false;

		PhotonView.RPC ("RPC_LoadLevel", PhotonTargets.All);

		//PhotonNetwork.LoadLevel (1);	//This will start Scene on number 1 (So the game scene should be on # 1); the lobby is on # 0 in this one
	}

	[PunRPC]
	public void RPC_LoadLevel() {
		PhotonNetwork.LoadLevel ("MainGame");
	}

	private void OnJoinedRoom(){
		if(PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers){
			PhotonNetwork.room.IsOpen = false;
			PhotonNetwork.room.IsVisible = false;
		}
	}

	//if room was full, open it when player leaves. *********** check what to do when game scene is loaded 
	private void OnLeftRoom(){

		if(PhotonNetwork.room.IsOpen == false || PhotonNetwork.room.IsVisible == false){
			PhotonNetwork.room.IsOpen = true;
			PhotonNetwork.room.IsVisible = true;
		}
	}

}
