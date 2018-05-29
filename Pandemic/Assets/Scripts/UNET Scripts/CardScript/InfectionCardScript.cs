using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InfectionCardScript : CardScript {

	public GameObject city;
	public GameObject disease;

	public void init(GameObject city, GameObject disease) {
		this.city = city;
		this.disease = disease;
	}

	public GameObject getCity() {
		return this.city;
	}

	public GameObject getDisease() {
		return this.disease;
	}
}
