using UnityEngine;
using UnityEngine.SceneManagement;		//*********************  Remember this  **************************
using System.IO;
//using UnityEngine.Networking;
//using PandemicEnum;
using System.Collections.Generic;

//This is a component of PlayerNetworkObject in DDOL object
public class PlayerNetwork : Photon.PunBehaviour {

	public static PlayerNetwork Instance; //makes this a singleton for each client
	public string PlayerName { get; private set; }
	private PhotonView PhotonView;
	private int PlayersInGame = 0;			//for master to keep track of how many clients have joined the game

	public static List<string> roleList;	// stores full list of roles
//	public Material roleMaterial;

	public GameObject myPawn;	// the pawn of the client running this script

	//public NetworkManager manager;


	//This excutes the user network instance
	private void Awake() {
		Debug.Log("Awake()");
		Instance = this;
		PhotonView = GetComponent<PhotonView> ();
		//PlayerName = playerDetails.Instance.username;
		PlayerName = PhotonNetwork.player.NickName;

		PhotonNetwork.sendRate = 60;					//default 20
		PhotonNetwork.sendRateOnSerialize = 30;			//default 10

		//creates a delegate using the scene manager class, whenever scene loading occurs, it will call my OnSceneFinishedLoading
		SceneManager.sceneLoaded += OnSceneFinishedLoading;		

	}


	/**************************************************************** Loaded New scene, handover networking ***********************************************************************/


	//This action is performed when a scene change has occured
	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
		Debug.Log("OnSceneFinishedLoading");

		if(scene.name == "MainGame"){		//game_scene is the name of my actual game scene *************************************************** NewGameScene
//			PhotonView = GetComponent<PhotonView> ();
			Debug.Log("MainGame");
			if (PhotonNetwork.isMasterClient) {  
				Debug.Log("isMaster");
				MasterLoadedGame ();
				//Here click host pandemic game 
//				if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null){
//					manager.StartHost();
//				}

			}
			else {
				Debug.Log("notMaster");
				NonMasterLoadedGame ();
				//click client for the rest
//				if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null){
//					//manager.StartClient();
//				}
			}

		}
	}

	//called when its masterclient/player who loaded the game
	//Tell all the other players to join the game_scene
	//******* Here need to add Photon View Script in PlayerNetwork on inspector, set view ID to 999*******

	private void MasterLoadedGame(){
		Debug.Log("MasterLoadedGame");
//		RPC_LoadedGameScene();

		PhotonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient);
//		PhotonView.RPC ("RPC_LoadGameOthers", PhotonTargets.Others); 		//Photon method that broadcasts message to other players in game

	}

	//called when its not a masterclient/player who loaded the game
	//******* Here need to add Photon View Script in PlayerNetwork on inspector *******
	private void NonMasterLoadedGame(){
		Debug.Log("NonMasterLoadedGame");

		PhotonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient);		//only the master needs to be made aware when a client has loaded the scene
	}

	//This just tells all other players to load to level 1 (scene at level 1)

	[PunRPC]
	private void RPC_LoadGameOthers(){
		Debug.Log("RPC_LoadGameOthers");
		PhotonNetwork.LoadLevel ("MainGame"); 
	}

	//This is going to be called on the master only, telling it # of players in the game vs # of players who are supposed to be in the game
	//when the # of players reaches the right amount, it then starts the game

	//private void RPC_LoadedGameScene(PhotonPlayer photonPlayer){
	[PunRPC]
	private void RPC_LoadedGameScene(){
		Debug.Log("RPC_LoadedGameScene");
		//PlayerManagement.Instance.AddPlayerStats (photonPlayer);

		PlayersInGame++;			//remember to decrease this as players leave the game and go back to the lobby	*************************************
		if (PlayersInGame == PhotonNetwork.playerList.Length) {	

			print ("All players are in the gamescene");
			PhotonView.RPC ("RPC_CreatePlayer", PhotonTargets.All);	//since all players are in game, create their prefabs
			Master_AssignRoles();
			Master_AssignHands();
			PhotonView.RPC ("RPC_SetTotalActions", PhotonTargets.All);
			PhotonView.RPC ("RPC_SetupGame", PhotonTargets.All);
		}
		else {
			print ("Not all players in the game, prefabs not created!!!!!!!!!!!!!!");
		}
	}

	//This method is for creating/spawning player prefabs on a network
	[PunRPC]
	private void RPC_CreatePlayer(){
		float randomXaxisValue = Random.Range (0f, 10f);

        //use PhotonNetwork.Instantiate than just Instatiate because its on the network;
        //"Prefabs" because its in Prefab folder and the prefab name is NewPlayer\
        // This instantiates and sends it over the nextwork

        GameObject currentCity = GameObject.Find("Atlanta");
        myPawn = PhotonNetwork.Instantiate( Path.Combine("Prefabs", "pawn"),  Vector3.up * randomXaxisValue, Quaternion.identity, 0);
		Debug.Log("Instantiating pawn");

        myPawn.GetComponent<PlayerMovement>().TargetParent = "Atlanta";
		myPawn.GetComponent<PlayerMovement> ().FindActionButtons ();
        myPawn.transform.parent = currentCity.transform;
        myPawn.transform.localScale = new Vector3(1f, 1f, 1f);
        myPawn.transform.localPosition = new Vector3(0, 0, 0);

        Debug.Log("Pawn Created");


	}

	// only called by MasterClient
	private void Master_AssignRoles() {
		Debug.Log("Master_AssignRoles");
		// instantiate roleList to contain all roles
		roleList = new List<string> ( new string[] { "Contingency Planner", "Dispatcher", "Medic", "Operations Expert", "Quarantine Specialist", "Researcher", "Scientist" } );

		// shuffles the roles 
		int n = roleList.Count;
		while (n > 1) {
			n--;
			int k = Random.Range(0, n);
			string role = roleList[k];
			roleList[k] = roleList[n];
			roleList[n] = role;
		}

		// calls function to assign roles to each player sequentially
		int index = 0; 
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			Debug.Log("*** INDEX = " + index);
			string role = roleList[index];
			PhotonView.RPC("RPC_AssignMyRole", player, role);
			PhotonView.RPC("RPC_ShiftPosition", player, index);
			index++;
       	}
	}

	[PunRPC]
	private void RPC_AssignMyRole(string myRole) {
		Debug.Log("Assigning my role");
		myPawn.GetComponent<PlayerMovement>().AssignMyRole(myRole);
	} 

	[PunRPC]
	private void RPC_ShiftPosition(int offset) {
		Debug.Log("Shifting my pawn");
		myPawn.GetComponent<PlayerMovement> ().ShiftPawn(offset);
	}

	// Master assigns hands to players
	[PunRPC]
	private void Master_AssignHands() {
		Debug.Log("PlayerNetwork : Master_AssignHands");
		Deck x = GameObject.Find("Decks").GetComponent<Deck> ();
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			x.Master_AssignHand(player);
		}
	}
	
	// Called from Player to Master to add a card to the specified discard pile
	[PunRPC]
	public void RPC_MasterDiscardCard(string discardDeck, string card) {
		var Decks = GameObject.Find("Decks");
		Decks.GetComponent<Deck>().MasterDiscardCard(discardDeck, card);
		
	}

	[PunRPC]
	private void RPC_SetupGame() {
		Debug.Log ("PlayerNetwork: RPC_SetupGame() on client " + PhotonNetwork.player.ID);
		MainGame game = GameObject.Find ("GameManager").GetComponent<MainGame> ();
		game.SetupGame ();
	}


	//calls RPC_NewHealth(PhotonPlayer photonPlayer, int health) which destroy the player object if its health is 0
	public void NewHealth(PhotonPlayer photonPlayer, int health){
		PhotonView.RPC ("RPC_NewHealth", photonPlayer, health);
	}


	[PunRPC]
	private void RPC_SetTotalActions () {
		Debug.Log ("PlayerNetwork: RPC_SetTotalActions() on client " + PhotonNetwork.player.ID);
		myPawn.GetComponent<PlayerMovement> ().SetTotalActions ();	
	}

}
