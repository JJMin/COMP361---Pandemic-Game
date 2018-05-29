using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PandemicEnum;

public class ActionPanelScript : MonoBehaviour {

	private const string actionsLeftPrefix = "Actions Left: ";

	public GameObject board;
	public PlayerScript playerScript;
    public GameObject player;

	public Text playerNameText;
	public Text actionsLeftText;
	public Text roleNameText;

	public Button button1;
	public Button button2;
	public Button button3;
	public Button button4;
	public Button button5;
	public Button button6;
	public Button button7;
	public Button button8;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Initial setup of the Action Panel.
	 */ 
	public void init(GameObject player) {
		this.playerScript = player.GetComponent<PlayerScript> ();
        this.player = player;

		this.board = GameObject.Find ("Board");

		// set player information in the info panel
		this.playerNameText.text = this.playerScript.getPlayerName();
		updateRoleText ();
		updateActionsLeftText ();

		attachListenersToButtons ();
	}

	public void createUI() {
		
	}

	/**
	 * Attach OnClick Listeners to every action button
	 */ 
	public void attachListenersToButtons() {
//		foreach (Button b in buttons) {
//			b.onClick.AddListener (delegate {
//				buttonClicked (b);
//			});
//		}


		// attach listeners to buttons for movement actions
		this.button1.GetComponentInChildren<Text>().text = "Drive/Ferry";
		this.button1.onClick.AddListener (delegate {
			this.board.GetComponent<EventTriggerController>().setupTriggersForDrive(this.player);
		});
			
		this.button2.GetComponentInChildren<Text>().text = "Direct Flight";
		this.button3.GetComponentInChildren<Text>().text = "Charter Flight";
		this.button4.GetComponentInChildren<Text>().text = "Shuttle Flight";

		// attach listeners to buttons for basic actions
		this.button5.GetComponentInChildren<Text>().text = "Build Research Station";
		this.button6.GetComponentInChildren<Text>().text = "Treat Disease";
		this.button7.GetComponentInChildren<Text>().text = "Discover Cure";
		this.button8.GetComponentInChildren<Text>().text = "Share Knowledge";

	}
		
	public void disableAllButtons() {
		Button[] buttons = this.GetComponentsInChildren<Button>();

		foreach (Button b in buttons) {
			b.interactable = false;
		}
	}

	// Set which buttons will be interactable TODO fix
	public void resetButtonPanel() {
//		Button[] buttons = this.GetComponentsInChildren<Button>();
//		for (int i = 0; i < buttons.Length; i++) {
//			Button b = buttons[i].GetComponent<Button>();
//			if (b.GetComponentInChildren<Text>().text.Equals("Drive")) {
//				b.interactable = true;
//			}
//		}
	}

	/**
	 * Update the Role text in the info panel 
	 */
	public void updateRoleText() {
		Role roleName = playerScript.getRole ().getRoleName ();
		this.roleNameText.text = roleName.ToString ();
	}

	/**
	 * Update the Actions Left text in the info panel
	 */ 
	public void updateActionsLeftText() {
		int actionsLeft = playerScript.getActionsLeft ();
		this.actionsLeftText.text = actionsLeftPrefix + actionsLeft.ToString ();
	}
}
