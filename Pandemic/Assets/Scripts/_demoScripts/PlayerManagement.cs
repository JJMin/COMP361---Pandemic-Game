using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is used by server for managing players, making sure they follow rules of the game
//in this example, it manages player health
public class PlayerManagement : MonoBehaviour {

	public static PlayerManagement Instance;
	private PhotonView PhotonView;

	private List<PlayerStats> PlayerStats = new List<PlayerStats> ();

	private void Awake(){
		Instance = this;
		PhotonView = GetComponent<PhotonView> ();
	}

	//add creates PlayerStats for photonPlayers and add to list of PlayerStats
	public void AddPlayerStats(PhotonPlayer photonPlayer){
		int index = PlayerStats.FindIndex (x => x.PhotonPlayer == photonPlayer);

		if(index == -1){
			PlayerStats.Add (new PlayerStats (photonPlayer, 30));	//default start health = 30
		}
	}

	public void ModifyHealth(PhotonPlayer photonPlayer, int value){
		int index = PlayerStats.FindIndex (x => x.PhotonPlayer == photonPlayer);
		if( index != -1){
			PlayerStats [index].Health += value;		//here the changes are now saved at the server, now we have to update the client *************** 
														//Do this under playernetwork script (but can choose any script)
			PlayerNetwork.Instance.NewHealth(photonPlayer, PlayerStats[index].Health);
			print ("decreased player health");
		}
	}
}


//
public class PlayerStats{

	public readonly PhotonPlayer PhotonPlayer;				//the photonPlayer assigned this class script will only be read on chisen attributes
	public int Health;


	public PlayerStats(PhotonPlayer photonPlayer, int health){
		PhotonPlayer = photonPlayer;
		Health = health;
	}

}