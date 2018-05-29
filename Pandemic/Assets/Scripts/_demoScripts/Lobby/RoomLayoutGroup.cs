using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {

	[SerializeField]
	private GameObject _roomListingPrefab;
	private GameObject RoomListingPrefab{
		get{ return _roomListingPrefab; }
	}

	//make a list of RoomListing
	private List<RoomListing> _roomListingButtons = new List<RoomListing>();
	private List<RoomListing> RoomListingButtons{
		get{ return _roomListingButtons; }
	}

	//This method is called by photon every time you receive a list of rooms
	private void OnReceivedRoomListUpdate(){
		
		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();

		foreach( RoomInfo room in rooms){
			RoomReceived (room);
		}

		RemoveOldRooms ();
	}

	//helper method for OnReceivedRoomListUpdate()
	//This method will check if the room already exist. If it does, it will just update necessary values 
	// otherwise it will just ignore
	private void RoomReceived(RoomInfo room){

		//takes all scripts in RoomListingButtons List and compare their RoomName and if they equal roon.Name
		int index = RoomListingButtons.FindIndex (x => x.RoomName == room.Name); //if not same, index returns -1

		//if roombutton could not be found, create button for the room
		if(index == -1){
			if(room.IsVisible && room.PlayerCount <room.MaxPlayers){
				
				GameObject roomListingObj = Instantiate (RoomListingPrefab);
				roomListingObj.transform.SetParent (transform, false);

				RoomListing roomListing = roomListingObj.GetComponent<RoomListing> ();
				RoomListingButtons.Add (roomListing);

				index = (RoomListingButtons.Count - 1);	//set index to the index of the added butto
			}
		}

		//if roombuttn already exists
		if( index != -1){
			RoomListing roomListing = RoomListingButtons [index];
			roomListing.SetRoomNameText (room.Name);	//just changing the available roomname
			roomListing.Updated = true;
		}
	}

	//This removes any rooms for the buttons that we have but no longer exist
	private void RemoveOldRooms(){

		List<RoomListing> removeRooms = new List<RoomListing> ();

		foreach(RoomListing roomListing in RoomListingButtons){
			if (!roomListing.Updated) {
				removeRooms.Add (roomListing);
			} 
			else {
				roomListing.Updated = false;
			}
		}

		foreach(RoomListing roomListing in removeRooms){
			GameObject roomListingObj = roomListing.gameObject;
			RoomListingButtons.Remove (roomListing);
			Destroy (roomListingObj);
		}

	}
}
