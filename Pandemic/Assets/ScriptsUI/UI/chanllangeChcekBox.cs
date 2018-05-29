using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//playerLoginScene/UIROOT/NewGameScreen/chanllenges管理复选框chanllenge
public enum Chanllenge { 
    NULL,
    V,
    VM,
    VB,
    M,
    B
}
public class chanllangeChcekBox : MonoBehaviour {
    public UISprite VScheck;
    public UISprite MTcheck;
    public UISprite BTcheck;
    private bool VSshown;
    private bool MTshown;
    private bool BTshown;


    //根据checkbox内容返回一个list
    public Chanllenge GetCheckStatus() {
        Chanllenge choosenChanllenge = Chanllenge.NULL;

        if (VSshown && !MTshown && !BTshown)
        {
            choosenChanllenge = Chanllenge.V;
        }
        else if (VSshown && MTshown && !BTshown )
        {

            choosenChanllenge = Chanllenge.VM;
        }
        else if (VSshown && !MTshown && BTshown)
        {

            choosenChanllenge = Chanllenge.VB;
            
        }
        else if (!VSshown && MTshown && !BTshown)
        {
            choosenChanllenge=Chanllenge.M;
            
           
        }
        else if (!VSshown && !MTshown && BTshown)
        {
            choosenChanllenge = Chanllenge.B;
            

        }
        return choosenChanllenge;

    }



    public void OnClickVS() {
        if (VSshown)
        {
            VScheck.enabled = false;
            VSshown = false;
        }
        else
        {
            VScheck.enabled = true;
            VSshown = true;
        }
    }
    public void OnClickMT() {
        if (MTshown)
        {
            MTcheck.enabled = false;
            MTshown = false;
           
        }
        else if(!MTshown&&BTshown)
        {
            MTcheck.enabled = true;
            MTshown = true;
            BTcheck.enabled = false;
            BTshown = false;
        }
        else if(!MTshown){
            MTcheck.enabled = true;
            MTshown = true;
        }
    }
    public void OnClickBT() {
        if (BTshown)
        {
            BTcheck.enabled = false;
            BTshown = false;
        }
        else if(!BTshown&&MTshown){
            BTcheck.enabled = true;
            BTshown = true;
            MTcheck.enabled = false;
            MTshown = false;
        }
        else if (!BTshown)
        {
            BTcheck.enabled = true;
            BTshown = true;
        }

    }
	
}
