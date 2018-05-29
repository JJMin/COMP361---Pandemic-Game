using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a component of LobbyNetworkObject
public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("Connecting to server");
		PhotonNetwork.ConnectUsingSettings("0.0.0");  //"1.0.0" --> game version
	}

	//connect to the master server and join the lobby
	private void OnConnectedToMaster(){
		print ("Connected to master");
		PhotonNetwork.automaticallySyncScene = false;		//This syncs all players in a room to a scene that masterclient is, if set true

		if (playerDetails.Instance != null)
		{
			PhotonNetwork.playerName = playerDetails.Instance.username;
		}
		else { 
			PhotonNetwork.playerName = "User#"+Random.Range(1000, 9999);
		}

		//now connected, so join lobby --> 
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	} 

	private void OnJoinedLobby(){
		print ("Joined Lobby");

		if( !PhotonNetwork.inRoom ){
			MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();	//This shows the lobby panel when the player or user joins the lobby
		}

	}


}
