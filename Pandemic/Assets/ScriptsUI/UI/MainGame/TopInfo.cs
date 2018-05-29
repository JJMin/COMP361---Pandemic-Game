using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopInfo : MonoBehaviour {
    private bool isTweenPanelShown = false;


    public void OnClickTopInfoPanel() {
        if (!isTweenPanelShown)
        {
            GetComponentInChildren<TweenPosition>().PlayForward();
            isTweenPanelShown = true;
        }
        else {
            GetComponentInChildren<TweenPosition>().PlayReverse();
            isTweenPanelShown = false;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
