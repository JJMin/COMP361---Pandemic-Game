using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon.Chat;


public class GameChat : MonoBehaviour, IChatClientListener
{
	//public GameObject loginForm;
	public GameObject chatBox;
	//public GameObject messagesContainer;
	private ChatClient chatClient;
	private string usernameText;
	public UILabel messagesText;
	public UIInput inputText;
	public UIGrid grid;
	string chatChannel;
	//GameObject go = NGUITools.addChild(grid.gameObject,messagesText.gameObject)
	//grid.addChild(go.transform)
	private bool isChatBoxShow = false;

	private void Start()
	{
		Debug.Log("GAMECHAT: Start");
		usernameText = PhotonNetwork.player.NickName;
		Application.runInBackground = true;
		Connect();
	}

	private void Update()
	{
		if (chatClient != null)
		{
			chatClient.Service();
			//Debug.Log("GAMECHAT UPDATE Status: "+chatClient.State.ToString());
		}

		HandleMessageSubmit();
	}

	private void HandleMessageSubmit()
	{
		if (!chatBox.activeSelf) return;

		if (inputText.value != "" && Input.GetKey(KeyCode.Return))
		{
			chatClient.PublishMessage(chatChannel, inputText.value);
			inputText.value = "";

		}
	}

	private void Connect()
	{
		Debug.Log("GAMECHAT: Connect");
		if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.ChatAppID))
		{
			print("No ChatAppID provided");
			return;
		}

		chatChannel = PhotonNetwork.room.Name+"CHANNEL";		/*************  append room name *****************/
		Debug.Log("GAMECHAT: About to connect to chat");

		ExitGames.Client.Photon.ConnectionProtocol connectProtocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;
		chatClient = new ChatClient(this, connectProtocol);
		chatClient.ChatRegion = "Cae";
		ExitGames.Client.Photon.Chat.AuthenticationValues authValues = new ExitGames.Client.Photon.Chat.AuthenticationValues();
		authValues.UserId = usernameText;
		authValues.AuthType = ExitGames.Client.Photon.Chat.CustomAuthenticationType.None;
		chatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "0.0.1", authValues);
		Debug.Log("GAMECHAT: Connected");
	}

	public void OnConnected()
	{
		Debug.Log("GAMECHAT: Connected to Chat");

		chatBox.SetActive(true);
		messagesText.text = "";

		chatClient.Subscribe(new string[] { chatChannel });
		chatClient.SetOnlineStatus(ChatUserStatus.Online);
	}

	public void OnDisconnected()
	{
		Debug.Log("GAMECHAT: DisConnected from Chat");
		chatBox.SetActive(false);
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		

		for (int i = 0; i < senders.Length; i++)
		{
			GameObject go = NGUITools.AddChild(grid.gameObject, messagesText.gameObject);
			//messagesText.text = messagesText.text + "\n"
			//					+ senders[i] + ": "
			//					+ messages[i];
			go.GetComponent<UILabel>().text = senders[i] + ": " + messages[i];
			grid.AddChild(go.transform);

			Debug.Log("New message: " + messagesText.text);
		}



	}

	public void OnPrivateMessage(string sender, object message, string channelName) { }

	public void OnSubscribed(string[] channels, bool[] results)
	{
		messagesText.text += "Chat Online.";
		chatClient.PublishMessage(chatChannel, "Joined");
	}

	public void OnUnsubscribed(string[] channels) { }

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }

	public void OnChatStateChange(ChatState state) { }

	public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message) { }

	void OnApplicationQuit()
	{
		if (chatClient != null)
		{
			chatClient.Disconnect();
		}
	}

	public void OnClickChat()
	{
		if (!isChatBoxShow)
		{
			GetComponentInChildren<TweenPosition>().PlayForward();
			isChatBoxShow = true;
			transform.Find("ChatLabel").GetComponent<UILabel>().text = "▲";
		}
		else
		{
			GetComponentInChildren<TweenPosition>().PlayReverse();
			isChatBoxShow = false;
			transform.Find("ChatLabel").GetComponent<UILabel>().text = "▼ Chat ▼";
		}
	}
}

