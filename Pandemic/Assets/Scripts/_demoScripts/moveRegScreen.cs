using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveRegScreen : MonoBehaviour {
    public TweenPosition tweenPosStart;
    public TweenPosition tweenPosLogin;
    public TweenPosition tweenPosReg;
    public GameObject lobby;
    //public GameObject NewGameScreen;
    //public GameObject LoadGameScreen;
    //public GameObject CurrentRoom;

    public void OnClickLoginBtnInStart() {
        tweenPosStart.PlayForward();
        tweenPosLogin.PlayForward();
    }
    public void OnClickRegisterBtnInStart()
    {
        tweenPosStart.PlayForward();
        tweenPosReg.PlayForward();
    }

    public void OnClickBackBtnInLogin() {
        tweenPosStart.PlayReverse();
        tweenPosLogin.PlayReverse();
    }
    public void OnClickLoginBtnInLogin() {
       
        bool check=transform.parent.GetComponent<RegLoginUIcheck>().checkLogin();
        if(check){
            //tween animation if pass
            GetComponentInParent<TweenAlpha>().PlayForward();
            lobby.SetActive(true);
            //lobby.GetComponent<TweenAlpha>().PlayForward();
        }
       
    }
    //设置从lobby退回regin界面为default
    public void SetRegLoginDefault() {
        tweenPosLogin.PlayReverse();
        tweenPosStart.PlayReverse();
    }

    public void OnClickBackBtnInReg() {
        tweenPosStart.PlayReverse();
        tweenPosReg.PlayReverse();
    }
   
    public void OnClickRegisterBtnInReg()
    {
        bool check = transform.parent.GetComponent<RegLoginUIcheck>().checkRegister();
        if(check){
            tweenPosReg.PlayReverse();
            tweenPosLogin.PlayForward();
        }
        
    }
   
}
