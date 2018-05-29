using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateRoom : Photon.PunBehaviour {

	[SerializeField]		//rooms will be serialized and stored in textfile
	private Text _roomName;
	public Text RoomName{ 
		get { return _roomName; }
	}

	[SerializeField]
	private Text _numPlayers;
	public Text NumPlayers{
		get { return _numPlayers; }
	}

	[SerializeField]
	private Text _numEpidermicCards;
	private Text NumEpidermicCards{
		get { return _numEpidermicCards; }
	}

	[SerializeField]
	private Toggle _virulentStrain;
	private Toggle VirulentStrain{
		get { return _virulentStrain; }
	}

	[SerializeField]
	private Toggle _bioTerrorist;
	private Toggle BioTerrosist{
		get{ return _bioTerrorist; }
	}

	[SerializeField]
	private Toggle _mutation;
	private Toggle Mutation{
		get { return _mutation; }
	}

	public void Onclick_CreateRoom(){

		if(RoomName.text.Equals(string.Empty)){		//can't create an empty room
			return;
		}

		RoomOptions roomOptions = new RoomOptions (){ IsVisible = true, IsOpen = true, MaxPlayers = System.Convert.ToByte(NumPlayers.text) }; // config setup

		
		if (PhotonNetwork.CreateRoom (RoomName.text, roomOptions, TypedLobby.Default)){
			print ("Create room succesfully sent");
		} else {
			print ("Create room failed to send, not connected to server");	//here it means you are not connected to the server yet when calling create room
		}

	}

	//this is called when user is trying to create a room name that already exists.
	//***** This will not happen in our game since the system/game will be creating the distinct room names
	private void OnPhotonCreateRoomFailed( Object[] codeAndMessage){
		print ("Create room failed: " + codeAndMessage[1]);	// codeAndMessage[1] contains reason createroom failed
	}

	private void OnCreatedRoom(){
		print ("Room created successfully");
	}
}
