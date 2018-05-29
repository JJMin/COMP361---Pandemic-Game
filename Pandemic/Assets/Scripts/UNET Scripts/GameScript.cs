using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PandemicEnum;


/**
 *  NEEDS CLEANUP
 */
[System.Serializable]
public class GameScript : MonoBehaviour {

	public static GameScript current;
	public GameObject playerPrefab;
	public Material roleMaterial;

	public GameObject localPlayer;
	public GameObject[] cities;

	public Deck playerDeck;
	public Deck playerDiscardPile;
    public Deck infectionDeck;
    public Deck infectionDiscardPile;

    public List<RoleScript> gameRole = new List<RoleScript>();

	//useful for saving and loading game
	public string roomname;
	public List<string> playersInGame = new List<string> ();		


	// Use this for initialization
	void Start () {

		// Maybe should move this to another method for initialization
		cities = GameObject.FindGameObjectsWithTag ("City");
        // Add every role in roleGame so we can choose from them when initializing a player
        foreach (Role role in PandemicEnum.Role.GetValues(typeof(Role))) {
            RoleScript newRole = new RoleScript();
            newRole.setRole(role);
            gameRole.Add(newRole);
        }


        // Add every cityCard in player deck
        playerDeck = new Deck();
        for (int i = 0; i<cities.Length; i++) {
            PlayerCityCardScript cityCard = new PlayerCityCardScript();
            int population = cities[i].GetComponent<CityScript>().getPopulation();
            cityCard.init(cities[i], population);
            playerDeck.insert(cityCard);
        }

		foreach(PhotonPlayer player in PhotonNetwork.otherPlayers){		//fill in player list
			playersInGame.Add(player.NickName);
		}
		roomname = PhotonNetwork.room.Name;								//get room name

        //setupBaseGame ();
    }

	// Update is called once per frame
	void Update () {

	}

	public void setupBaseGame() {
		GameObject startingCity = GameObject.Find ("Board/Cities/Atlanta");

		// TODO: find a better way to assign material for player? With the current method,
		// we are forced to create a public var for each possible colour/role
		localPlayer = Instantiate (playerPrefab, playerPrefab.transform.position, Quaternion.identity);
		localPlayer.GetComponent<Renderer> ().material = roleMaterial;
        //localPlayer.GetComponent<PlayerScript> ().assignStartingCity (startingCity);
        //localPlayer.GetComponent<PlayerScript> ().setPlayerName ("Philippe");

        // Pick a random role, assign and remove it from list
        int randomIndex = Random.Range(0, this.gameRole.Count);
        localPlayer.GetComponent<PlayerScript> ().init ("Philippe", startingCity, gameRole[randomIndex]);
        gameRole.RemoveAt(randomIndex);
        localPlayer.GetComponent<PlayerScript>().setCurrentGame(gameObject);
        Debug.Log(gameObject.GetComponent<GameScript>().localPlayer.GetComponent<PlayerScript>().getPlayerName());

		initActionPanel ();

		nextTurn ();
	}

	public void nextTurn() {
		//setupEvents ();
	}

	private void initActionPanel() {
		GameObject panel = GameObject.Find ("Canvas/ActionPanel");
		panel.GetComponent<ActionPanelScript> ().init (localPlayer);
	}

    // GETTERS AND SETTERS

    public GameObject[] getCities() {
        return this.cities;
    }

    public GameObject getLocalPlayer() {
        return this.localPlayer;
    }

    public RoleScript getRole() {
        int randomIndex = Random.Range(0, this.gameRole.Count);
        RoleScript role = gameRole[randomIndex];
        gameRole.RemoveAt(randomIndex);
        return role;
    }




    public class Deck {
		public List<CardScript> deck = new List<CardScript>();

		public CardScript draw() {
			if (deck.Count > 0) {
				CardScript drawn = this.deck[deck.Count - 1];
				this.deck.RemoveAt(deck.Count - 1);
				return drawn;
			}
			return null;
		}

		public CardScript drawFromBottom() {
			if (deck.Count > 0) {
				CardScript drawn = this.deck[0];
				this.deck.RemoveAt(0);
				return drawn;
			}
			return null;
		}

		public void insert(CardScript card) {
			this.deck.Add(card);
		}

		public List<CardScript> getDeck() {
			return this.deck;
		}

		public void shuffle() {
			for(int i = 0; i < this.deck.Count; i++) {
				CardScript temp = this.deck[i];
				int randomIndex = Random.Range(i, this.deck.Count);
				this.deck[i] = this.deck[randomIndex];
				this.deck[randomIndex] = temp;
			}
		}

	}
}
