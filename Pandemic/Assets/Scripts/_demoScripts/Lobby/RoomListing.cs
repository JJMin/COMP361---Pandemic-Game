using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {
	
	[SerializeField]
	private Text _roomNameText;
	private Text RoomNameText{		//getter
		get{ return _roomNameText; }
	}

	public string RoomName{ get; private set; }
	public bool Updated{ get; set;}

	// set up a listener
	private void Start () {
		GameObject lobbyCanvasObj = MainCanvasManager.Instance.LobbyCanvas.gameObject;

		//This shouldn't happen but if it does then we have a check
		if( lobbyCanvasObj == null ){
			return;
		}

		LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas> ();


		Button button = GetComponent<Button> ();
		button.onClick.AddListener ( () => lobbyCanvas.OnClickJoinRoom(RoomNameText.text) );

	}

	//we destroy the listener whenever the lobbyCanvasObj object disappears
	private void OnDestroy(){
		Button button = GetComponent<Button> ();
		button.onClick.RemoveAllListeners ();
	}

	//This method is called whenever user wants to change the text
	public void SetRoomNameText(string txt){
		RoomName = txt;
		RoomNameText.text = RoomName;
	}
}
