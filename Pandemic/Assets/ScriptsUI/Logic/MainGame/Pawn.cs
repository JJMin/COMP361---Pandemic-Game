using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {
    public Player player;
    public City currentCity;//当前棋子所在城市
    public string role;

   
    public void setPawnColor() { 
        switch(player.job){
            case Job.SCIENTIST:
                this.gameObject.GetComponent<UISprite>().color = Color.white;
                    break;
            case Job.TEACHER:
                this.gameObject.GetComponent<UISprite>().color = Color.yellow;
                    break;
        }
        
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
