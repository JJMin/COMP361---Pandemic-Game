using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is attached to the Lobby gameObject
public class LobbyCanvas : MonoBehaviour {

	[SerializeField]
	private RoomLayoutGroup _roomLayoutGroup;
	private RoomLayoutGroup RoomLayoutGroup{
		get{ return _roomLayoutGroup; }
	}

	public void OnClickJoinRoom(string roomName){

		if (PhotonNetwork.JoinRoom (roomName)) {
			
		} 
		else {
			print ("Join room failed");
		}
	}
}
