using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is responsible for keeping tracking of all the subcanvases under it
public class MainCanvasManager : MonoBehaviour {

	public static MainCanvasManager Instance;

	//lobby canvas control
	[SerializeField]
	private LobbyCanvas _LobbyCanvas;
	public LobbyCanvas LobbyCanvas{
		get{ return _LobbyCanvas; }
	}

	//currentroom canvas control
	[SerializeField]
	private CurrentRoomCanvas _currentRoomCanvas;
	public CurrentRoomCanvas CurrentRoomCanvas{
		get{ return _currentRoomCanvas; }
	}

	private void Awake(){
		Instance = this;
	}
}
