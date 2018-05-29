using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerWall : MonoBehaviour {

	//will be called whenever something passes through it
	private void OnTriggerEnter(Collider other){

		//server is the one that manages this
		if(!PhotonNetwork.isMasterClient){
			return;
		}

		PhotonView photonView = other.GetComponent<PhotonView> ();	//photonview from the player that hit the trigger
		if(photonView != null){
			PlayerManagement.Instance.ModifyHealth (photonView.owner, -10);
		}

	
	}

}
