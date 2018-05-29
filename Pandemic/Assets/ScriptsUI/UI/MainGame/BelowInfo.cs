using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowInfo : MonoBehaviour {
    private bool isBelowPanelShown=true;
    private bool isEventPanelShown=false;
    public TweenScale eventPanelTween;
   
    //当点击下面
    public void OnClickBelowInfo() {
        if (!isBelowPanelShown&&isEventPanelShown)//下面没有显示且左边显示了
        {
            //下面显示
            transform.Find("BelowPanel").GetComponent<TweenPosition>().PlayReverse();
            transform.Find("tri").GetComponent<UILabel>().text = "▲                                                                                      ▲";
            isBelowPanelShown = true;
            //左边消失
            eventPanelTween.PlayReverse();
          
            isEventPanelShown = false;

        }
        else if(!isBelowPanelShown){
            transform.Find("BelowPanel").GetComponent<TweenPosition>().PlayReverse();
            transform.Find("tri").GetComponent<UILabel>().text = "▲                                                                                      ▲";
            isBelowPanelShown = true;
            //下面显示
           

        }
        else if(isBelowPanelShown){
            transform.Find("BelowPanel").GetComponent<TweenPosition>().PlayForward();
            transform.Find("tri").GetComponent<UILabel>().text = "▼                                                                                      ▼";
            isBelowPanelShown = false;
        }
       
    }
    public void OnClickEventCardYellow() {
        if (isBelowPanelShown&&!isEventPanelShown)//下边显示了且左边没显示
        {
            transform.Find("BelowPanel").GetComponent<TweenPosition>().PlayForward();
            transform.Find("tri").GetComponent<UILabel>().text = "▼                                                                                      ▼";
            isBelowPanelShown = false;
           
            eventPanelTween.PlayForward();
           
            isEventPanelShown = true;
        }
        else if(!isEventPanelShown){
            eventPanelTween.PlayForward();
           
            isEventPanelShown = true;
        }
        else if(isEventPanelShown){
            eventPanelTween.PlayReverse();
           
            isEventPanelShown = false;
        }
    }

}
