using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour {

	public void endTurnClicked () {
		Debug.Log ("EndTurn button clicked");
		GameObject pawn = GameObject.Find ("_NetworkManager").GetComponent<PlayerNetwork> ().myPawn;
		pawn.GetComponent<PlayerMovement> ().EndTurn ();
	}
}
