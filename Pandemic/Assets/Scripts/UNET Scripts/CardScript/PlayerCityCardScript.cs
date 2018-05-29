using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCityCardScript : PlayerCardScript {

	public string cityName;
	public int population;
	public GameObject city;
		
	public void init(GameObject city, int population) {
		this.city = city;
		this.population = population;
		this.cityName = city.name;
	}

	// GETTERS AND SETTERS

	public string getName() {
		return this.cityName;
	}

	public int getPopulation() {
		return this.population;
	}

	public GameObject getCity() {
		return this.city;
	}
}

