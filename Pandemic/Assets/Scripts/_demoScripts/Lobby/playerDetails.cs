using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetails : MonoBehaviour {

	public static playerDetails Instance;
	public string username;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}
}
