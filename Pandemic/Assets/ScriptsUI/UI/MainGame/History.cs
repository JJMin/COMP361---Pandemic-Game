using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : MonoBehaviour {
    private bool isShown = false;
    private TweenPosition tweenPos;
    public GameObject PlayerBG;
   
    public GameObject InfectionBG;
  
    //当点击history时 弹出队伍信息框
    public void OnClickHistory()
    {
        tweenPos = GetComponentInChildren<TweenPosition>();
        if (!isShown)
        {
            transform.Find("HistoryLabel").GetComponent<UILabel>().text = "▲";
            tweenPos.PlayForward();
            isShown = true;
        }
        else
        {
            transform.Find("HistoryLabel").GetComponent<UILabel>().text = "▼ History ▼";
            tweenPos.PlayReverse();
            isShown = false;
        }
    }
    public void OnClickPlayerDiscardIcon() {
            InfectionBG.SetActive(false);
            PlayerBG.SetActive(true);
        
    }
    public void OnClickInfectionDiscardIcon() { 
       
            InfectionBG.SetActive(true);
            PlayerBG.SetActive(false);
        
    }
}
