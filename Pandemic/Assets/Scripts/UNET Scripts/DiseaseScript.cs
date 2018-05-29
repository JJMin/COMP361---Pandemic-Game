using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PandemicEnum;



/**
 * TODO:
 * 	Extension:
 * 	- Add virulent strain attribute
 */
public class DiseaseScript : MonoBehaviour {

	// TODO: init all vars
	public DiseaseColour colour;

	public int cubesOnBoard;  
	public int maxCubes;       

	public bool cured;
	public bool eradicated;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void init(DiseaseColour colour, int maxCubes) {
		this.colour = colour;
		this.maxCubes = maxCubes;
		this.cubesOnBoard = 0;
		this.cured = false;
		this.eradicated = false;
	}

	// GETTERS & SETTERS
	public DiseaseColour getColour() {
		return this.colour;
	}
		
	public int getCubesOnBoard() {
		return this.cubesOnBoard;
	}

	public void setCubesOnBoard(int count) {
		this.cubesOnBoard = count;
	}

	public int getMaxCubes() {
		return this.maxCubes;
	}

	public void setMaxCubes(int count) {
		this.maxCubes = count;
	}

	public bool getCured() {
		return this.cured;
	}

	public void setCured(bool cured) {
		this.cured = cured;
	}

	public bool getEradicated() {
		return this.eradicated;
	}

	public void setEradicated(bool eradicated) {
		this.eradicated = eradicated;
	}

	// METHODS

	/**
	 * Add count number of cubes, limited to the max. number of cubes.
	 */ 
	public void addCubes (int count) {
		int newCount = Mathf.Min (this.cubesOnBoard + count, this.maxCubes);
		this.cubesOnBoard = newCount;
	}

	/**
	 * Remove count number of cubes, minimum is 0.
	 */ 
	public void removeCubes (int count) {
		int newCount = Mathf.Max (this.cubesOnBoard - count, 0);
		this.cubesOnBoard = newCount;
	}
}
