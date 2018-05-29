using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void init(GameObject player) {
		GameObject actionPanel = gameObject.transform.Find ("ActionPanel").gameObject;
		actionPanel.GetComponent<ActionPanelScript> ().init (player);
	}
}
