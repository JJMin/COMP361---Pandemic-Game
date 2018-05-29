using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEnum : MonoBehaviour {

	private void Awake() {


	}

//	void Update() {
//		if (PhotonView.isMine) {	//if person controlling indeed owns this game object
//			
//		} 
//		else {
//
//		}
//	}
//
//
//	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
//		
//		if (stream.isWriting) {			//check if we are sending the data
//			stream.SendNext(roleList);
//		} 
//		else {							//or we are reading the data
//			roleList = (List<string>) stream.ReceiveNext();
//		}
//
//	}
//
//	void UpdateRoleList() {
//
//
//	}
//
//	public void ShuffleRoles() {
//		int n = roleList.Count;
//		while (n > 1) {
//			n--;
//			int k = Random.Range(0, n);
//			string role = roleList[k];
//			roleList[k] = roleList[n];
//			roleList[n] = role;
//		}
//	}
}
