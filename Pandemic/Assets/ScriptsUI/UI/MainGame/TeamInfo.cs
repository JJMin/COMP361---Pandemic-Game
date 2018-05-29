using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//挂在maingame/UIroot/teaminfolabel上
public class TeamInfo : MonoBehaviour {
    private bool isShown = false;
    private TweenPosition tweenPos;



    //当点击team info时 弹出队伍信息框
    public void OnClickTeamInfo() { 
        tweenPos=GetComponentInChildren<TweenPosition>();
        if(!isShown){
            transform.Find("Label").GetComponent<UILabel>().text = "▲";
            tweenPos.PlayForward();
            isShown=true;
        }else{
            transform.Find("Label").GetComponent<UILabel>().text = "▼ Team Info ▼";
            tweenPos.PlayReverse();
            isShown = false;
        }
    }
	
}
