using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PandemicEnum;

[System.Serializable]
public class CityScript : MonoBehaviour {

	public List<GameObject> neighbours = new List<GameObject> ();

	// counters for number of disease cubes, DiseaseColour enum value is index
	// [yellow, red, blue, black, purple]
	public int[] cubeCount = new int[5];
    public int population;
	public bool researchStation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<GameObject> getNeighbours() {
		return this.neighbours;
	}

    // GETTERS & SETTERS

    public int getPopulation() {
        return this.population;
    }

	public int getCubeCount(DiseaseColour colour) {
		return this.cubeCount[(int) colour];
	}

	public void setCubeCount(DiseaseColour colour, int count) {
		this.cubeCount[(int) colour] = count;
	}

	public bool getResearchStation() {
		return this.researchStation;
	}

	public void setResearchStation(bool researchStation) {
		this.researchStation = researchStation;
	}

	// METHODS
	public void incrementCubeCount(DiseaseColour colour) {
		// TODO: CHECK FOR OUTBREAK (>3)
		this.cubeCount[(int) colour]++;
	}

	/**
	 * Remove 1 disease cube of a given colour, minimum of 0. 
	 */ 
	public void decrementCubeCount(DiseaseColour colour) {
		int currCount = this.cubeCount [(int)colour];
		int newCount = Mathf.Max (currCount - 1, 0);
		this.cubeCount [(int)colour] = newCount;
	}
		
	/**
	 * Sets the number of cubes of a certain disease to 0. 
	 * Returns the number of cubes removed.
	 */
	public int removeAllCubes(DiseaseColour colour) {
		int numRemoved = this.cubeCount [(int)colour];
		this.cubeCount [(int)colour] = 0;
		return numRemoved;
	}
}
