using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class move : MonoBehaviour {
    public TweenPosition tweenPosStart;
    public TweenPosition tweenPosLogin;
    public TweenPosition tweenPosReg;
    public GameObject lobby;
    public GameObject NewGameScreen;
    public GameObject LoadGameScreen;

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
			// *********** I may have to reposition  every object in this scene so it gets back to normal when player gets back
			// Load to Lobby scene
			SceneManager.LoadScene("_LobbyScene", LoadSceneMode.Single);
		
            //tween animation if pass
            //GetComponentInParent<TweenAlpha>().PlayForward();
            //lobby.SetActive(true);
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

	public void OnClickExitLobbyScene(){
		SceneManager.LoadScene("playerLoginScene", LoadSceneMode.Single);
	}

	/*----------------------------------------------------------------------------------------------------------------------------------------*/


    public void OnClickExitBtnInLobby() {
        lobby.GetComponent<TweenAlpha>().PlayReverse();
        SetRegLoginDefault();
        GetComponentInParent<TweenAlpha>().PlayReverse();
    }
    public void OnClickNewGameBtnInLobby() {
        lobby.GetComponent<TweenAlpha>().PlayReverse();
        NewGameScreen.SetActive(true);
        NewGameScreen.GetComponent<TweenAlpha>().PlayForward();
    }
    public void OnClickLoadGameBtnInLobby() {
        lobby.GetComponent<TweenAlpha>().PlayReverse();
        LoadGameScreen.SetActive(true);
        LoadGameScreen.GetComponent<TweenAlpha>().PlayForward();
    }

    
    public void OnClickBackBtnInNewGameScreen() {
        NewGameScreen.GetComponent<TweenAlpha>().PlayReverse();
        lobby.GetComponent<TweenAlpha>().PlayForward();
    }

	//CreateGame takes you to waiting room
	public void OnClickCreateGameBtnInNewGameScreen() {
		NewGameScreen.GetComponent<TweenAlpha>().PlayReverse();

	}

	//exit room will get you back to the lobby
	public void OnClickExitBtnInCurrentRoom() {
		
		lobby.GetComponent<TweenAlpha>().PlayForward();
	}

	//clicking start game takes this scene to lobby. But the player's screen switches to mainGame scene
	public void OnClickStartGameBtnInCurrentRoom() {
		
		lobby.GetComponent<TweenAlpha>().PlayForward();
	}

    public void OnClickBackBtnInLoadGameScreen() {
        LoadGameScreen.GetComponent<TweenAlpha>().PlayReverse();
        lobby.GetComponent<TweenAlpha>().PlayForward();
    }
    
   
}
