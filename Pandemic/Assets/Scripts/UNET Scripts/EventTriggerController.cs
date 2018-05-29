using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using PandemicEnum;

public class EventTriggerController : MonoBehaviour {

	public GameObject[] cities;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void init() {
		this.cities = GameObject.FindGameObjectsWithTag("City");
	}

	public void setupTriggersForDrive(GameObject player) {
		PlayerScript playerScript = player.GetComponent<PlayerScript> ();
		GameObject currentCity = playerScript.getCurrentCity ();
		List<GameObject> neighbours = currentCity.GetComponent<CityScript>().getNeighbours();

		foreach (GameObject city in neighbours) {
			EventTrigger trigger = city.GetComponent<EventTrigger>();

			// add trigger to execute drive action
			EventTrigger.Entry actionEntry = new EventTrigger.Entry();
			actionEntry.eventID = EventTriggerType.PointerClick;
			actionEntry.callback.AddListener (delegate {
				playerScript.CmdDriveRequest(player, city);
			} );
			trigger.triggers.Add(actionEntry);


			//EventTrigger.Entry newTurnEntry = new EventTrigger.Entry();
			//newTurnEntry.eventID = EventTriggerType.PointerClick;
			//newTurnEntry.callback.AddListener((c) => { nextTurn(); });
			//trigger.triggers.Add(newTurnEntry);

			// FOR TESTING
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener (delegate {
				removeCityTriggers();
			} );
			trigger.triggers.Add(entry);
		}
	}

	/**
	 * Remove all event triggers from every city.
	 */ 
	public void removeCityTriggers() {
		foreach (GameObject city in cities) {
			EventTrigger trigger = city.GetComponent<EventTrigger>();
			if (trigger != null) {
				trigger.triggers.Clear();
			}
		}
	}
}
