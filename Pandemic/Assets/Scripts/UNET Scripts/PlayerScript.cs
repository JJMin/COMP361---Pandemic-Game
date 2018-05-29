using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PandemicEnum;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerScript : NetworkBehaviour {

	public string playerName;
	public GameObject currentCity;
	public int actionsLeft;
	public int totalActions;
	public List<PlayerCardScript> hand;
	public RoleScript role;
	public GameObject currentGame;
    // Because current game is not set yet
    public GameObject[] cities;
    public GameObject actionPanel;
	public GameObject actionPanelPrefab;


    // Use this for initialization
    void Start () {
    }

	// colour local player blue so for testing
	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer> ().material.color = Color.blue;

		this.actionPanel = Instantiate(this.actionPanelPrefab);
		actionPanel.GetComponent<CanvasScript>().init(gameObject);
	}

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) {
            return;
        }
        else {
			this.actionPanel.SetActive(true);
        }

		/* Add call for setupEvents
		 *
		 */

		// TESTING NETWORKING

//		if (Input.GetKeyDown(KeyCode.M)) {
//            GameObject startingCity = GameObject.Find("Board/Cities/Atlanta");
//            RoleScript r = new RoleScript();
//            r.setRole(Role.Scientist);
//            gameObject.GetComponent<PlayerScript>().init("Lea", startingCity, r);
//		}
//
//		if (Input.GetKeyDown(KeyCode.N)) {
//			GameObject p = GameObject.Find ("Board/Cities/Paris");
//			CmdDriveRequest(gameObject, p);
//		}
//
//		if (Input.GetKeyDown(KeyCode.H)) {
//			GameObject d = GameObject.Find ("Diseases/YellowDisease");
//			CmdTreatDiseaseRequest (gameObject, d);
//		}
//		if (Input.GetKeyDown(KeyCode.J)) {
//			GameObject d = GameObject.Find ("Diseases/RedDisease");
//			CmdTreatDiseaseRequest (gameObject, d);
//		}
//		if (Input.GetKeyDown(KeyCode.K)) {
//			GameObject d = GameObject.Find ("Diseases/BlueDisease");
//			CmdTreatDiseaseRequest (gameObject, d);
//		}
//		if (Input.GetKeyDown(KeyCode.L)) {
//			GameObject d = GameObject.Find ("Diseases/BlackDisease");
//			CmdTreatDiseaseRequest (gameObject, d);
//		}
	}

	public void init(string name, GameObject startCity, RoleScript role) {
		this.playerName = name;
		this.role = role;
		this.hand = new List<PlayerCardScript> ();

		// TODO:: if role == generalist, set total actions to 5
		/*if (role.getRoleName() == Role.Generalist) {
			this.totalActions = 5;
		} 
		else {
			this.totalActions = 4;
		}*/
		this.totalActions = 4;
		this.actionsLeft = this.totalActions;

		setCurrentCity (startCity);	
    }
		
    // PLAYER ACTIONS

    // ---- DRIVE/FERRY

    /**
	 * Request server to perform a Drive/Ferry action.
	 */
    [Command]
	public void CmdDriveRequest (GameObject player, GameObject city) {
		RpcDrive(player, city);
    }

	/**
	 * Propagate Drive/Ferry action to all clients
	 */ 
	[ClientRpc]
	public void RpcDrive(GameObject player, GameObject city) {
		PlayerScript playerScript = player.GetComponent<PlayerScript> ();
        Role role = playerScript.getRole().getRoleName();

        playerScript.setCurrentCity (city);
		playerScript.decrementActionsLeft ();

        // Medic remove all cubes of cured diseases in the destination city
        if (role == Role.Medic) {
			playerScript.treatDiseasesByMedicMovement (city);
		}
	}

	// ---- TREAT DISEASE

	/**
	 * Request server to perform a TreatDisease action.
	 */ 
	[Command]
	public void CmdTreatDiseaseRequest (GameObject player, GameObject disease) {
		RpcTreatDisease (player, disease);
	}

	/**
	 * Propagate TreatDisease action to all clients
	 */ 
	[ClientRpc]
	public void RpcTreatDisease(GameObject player, GameObject disease) {
		PlayerScript playerScript = player.GetComponent<PlayerScript> ();
		Role role = playerScript.getRole ().getRoleName ();

		CityScript cityScript = playerScript.getCurrentCity ().GetComponent<CityScript> ();

		DiseaseScript diseaseScript = disease.GetComponent<DiseaseScript> ();
		DiseaseColour colour = diseaseScript.getColour ();

		if (diseaseScript.getCured()) {
			// remove all cubes of the chosen disease
			int numCubesRemoved = cityScript.removeAllCubes (colour);
			diseaseScript.removeCubes (numCubesRemoved);

			// check for eradication
			if (diseaseScript.getCubesOnBoard() == 0) {
				diseaseScript.setEradicated (true);
			}
		}
		else if (role == Role.Medic) {
			// remove all cubes of the chosen disease
			int numCubesRemoved = cityScript.removeAllCubes (colour);
			diseaseScript.removeCubes (numCubesRemoved);
		}
		else {
			cityScript.decrementCubeCount (colour);
			diseaseScript.removeCubes (1);
		}
			
		playerScript.decrementActionsLeft ();
	}

	// ---- DIRECT FLIGHT

	// Take direct flight
	[Command]
	public void CmdTakeDirectFlightRequest(GameObject player, GameObject city) {
		RpcDriveTakeDirectFlight (player, city);
	}

	// Update Player location
	[ClientRpc]
	public void RpcDriveTakeDirectFlight(GameObject player, GameObject city) {
		PlayerScript ps = player.GetComponent<PlayerScript> ();
		ps.setCurrentCity (city);
		ps.decrementActionsLeft ();
	}


	// GETTERS & SETTERS

	public string getPlayerName() {
		return this.playerName;
	}

	public void setPlayerName(string name) {
		this.playerName = name;
	}

	public void decrementActionsLeft() {
		int newCount = Mathf.Max (this.actionsLeft - 1, 0);
		this.actionsLeft = newCount;
	}

	public int getActionsLeft() {
		return this.actionsLeft;
	}
		
	public void setTotalActions(int numActions) {
		this.totalActions = numActions;
	}

	public GameObject getCurrentCity() {
		return this.currentCity;
	}

	public void setCurrentCity(GameObject city) {
		this.currentCity = city;
		updatePosition ();
	}

    public void setCurrentGame(GameObject game) {
        this.currentGame = game;
    }

    public void setActionPanel(GameObject panel) {
        this.actionPanel = panel;
    }

	public List<PlayerCardScript> getHand() {
		return this.hand;
	}

	public void addCardToHand(PlayerCardScript card) {
		this.hand.Add (card);
	}

	public void removeCardFromHand(PlayerCardScript card) {
		this.hand.Remove (card);
	}

	// for terrorist
	public void discardHand() {
		this.hand.Clear ();
	}

	public RoleScript getRole() {
		return this.role;
	}

	public void setRole(Role role) {
		this.role.setRole (role);
	}

	// HELPER METHODS

	/*
	 * Update the position of the Player GameObject (the pawn).
	 * To be used whenever currentCity is modified (e.g. movement actions).
	 */ 
	private void updatePosition() {
		Vector3 newPos = new Vector3 (
			                 this.currentCity.transform.position.x,
			                 transform.position.y,
			                 this.currentCity.transform.position.z);
		transform.position = newPos;

	}

	/**
	 * Remove all disease cubes of cured disease in a given city. 
	 * Use this helper method for movement actions (Medic power).
	 */ 
	private void treatDiseasesByMedicMovement(GameObject city) {
		CityScript cityScript = city.GetComponent<CityScript> ();

		GameObject[] diseaseList = GameObject.FindGameObjectsWithTag ("Disease");

		foreach (GameObject disease in diseaseList) {
			DiseaseScript diseaseScript = disease.GetComponent<DiseaseScript> ();
			DiseaseColour colour = diseaseScript.getColour ();

			if (diseaseScript.getCured()) {
				int numCubesRemoved = cityScript.removeAllCubes (colour);
				diseaseScript.removeCubes (numCubesRemoved);

				// check for eradication
				if (diseaseScript.getCubesOnBoard() == 0) {
					diseaseScript.setEradicated (true);
				}
			}
		}
	}

	/*
	 * Set up event triggers for available cities for Take Direct Flight
	 * To be used whenever takeDirectFlight is called
	 */
	private void setupEventsForTakeDirectFlight() {
		// Remove all existing Event Triggers on cities
		GameScript gameScript = currentGame.GetComponent<GameScript> ();

		// Get the hand of the current Player
		List<PlayerCardScript> currentHand = this.getHand ();

		/* For each City Card in the Player's Hand, get the corresponding City
		 * Attach an event trigger to each City
		 */
		foreach (PlayerCardScript cardScript in currentHand) {
			if (cardScript.GetType () is PlayerCityCardScript) {
				PlayerCityCardScript cityCardScript = (PlayerCityCardScript) cardScript;
				GameObject city = cityCardScript.getCity ();

				EventTrigger trigger = city.GetComponent<EventTrigger> ();

				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerClick;
				//entry.callback.AddListener ( (c) => {this.takeDirectFlight(city);} );

				EventTrigger.Entry newTurnEntry = new EventTrigger.Entry ();
				newTurnEntry.eventID = EventTriggerType.PointerClick;
				newTurnEntry.callback.AddListener  ( (c) => {gameScript.nextTurn();} );
	
				trigger.triggers.Add (entry);
				trigger.triggers.Add (newTurnEntry);
			}
		}
	}
}
