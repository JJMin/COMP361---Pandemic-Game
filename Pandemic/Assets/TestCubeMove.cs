using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestCubeMove : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal")*100f;
        var z = Input.GetAxis("Vertical")*100f;

        transform.Translate(x, 0, z);
	}

	public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
