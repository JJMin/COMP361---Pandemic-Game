using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainGame : Photon.MonoBehaviour {

	private PhotonView PhotonView;

	public List<PhotonPlayer> playerList;
	public PhotonPlayer currentPlayer;
	public int currentPlayerIndex;
	public int numPlayers;

	//useful for saving and loading game
	public string roomname;
	public List<string> playerNames;

	// Use this for initialization
	void Start () {
		//cities = GameObject.FindGameObjectsWithTag("City");
		//        foreach (PhotonPlayer player in PhotonNetwork.otherPlayers) {       //fill in player list
		//            playersInGame.Add(player.NickName);
		//        }
		//        roomname = PhotonNetwork.room.Name;
	}

	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Setup game
	 * 
	 * list of players, turn order, current player
	 */
	public void SetupGame () {
		playerList = new List<PhotonPlayer> (PhotonNetwork.playerList);    // no guaranteed order
		playerList.Sort ();    // so all clients have the list in the same order

		numPlayers = playerList.Count;

		// TODO: starting player is the one holding the city card with the highest population

		currentPlayerIndex = 0;
		currentPlayer = playerList [currentPlayerIndex];    // starting player

		// info useful for saving/loading game
		roomname = PhotonNetwork.room.Name;
		playerNames = playerList.Select (player => player.NickName).ToList ();

		// tell player he is the current player
		if (currentPlayer == PhotonNetwork.player) {
			GameObject pawn = GameObject.Find ("_NetworkManager").GetComponent<PlayerNetwork> ().myPawn;
			pawn.GetComponent<PlayerMovement> ().StartTurn ();
		}
	}

	public void EndTurn () {
		// TODO: end of turn events

		// update current player
		currentPlayerIndex = (currentPlayerIndex + 1) % numPlayers;
		currentPlayer = playerList [currentPlayerIndex];

		if (currentPlayer == PhotonNetwork.player) {
			GameObject pawn = GameObject.Find ("_NetworkManager").GetComponent<PlayerNetwork> ().myPawn;
			pawn.GetComponent<PlayerMovement> ().StartTurn ();
		}	
	}
}
