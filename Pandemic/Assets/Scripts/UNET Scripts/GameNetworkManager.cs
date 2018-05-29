using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PandemicEnum;

class GameNetworkManager : NetworkManager {

    public GameObject playerObjectPrefab;
    public GameObject game;
	public GameObject board;
	public GameObject actionPanelPrefab;
    public Material roleMaterial;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        GameObject startingCity = GameObject.Find("Board/Cities/Atlanta");
        GameObject localPlayer = Instantiate(this.playerObjectPrefab, Vector3.zero, Quaternion.identity);
        GameScript curGame = game.GetComponent<GameScript>();
        RoleScript role = curGame.getRole();
        localPlayer.GetComponent<PlayerScript>().init("AAAAA", startingCity, role);
     
        //localPlayer.GetComponent<Renderer>().material = roleMaterial;
        Color pawnColor = Color.black;
        switch (role.getRoleName()) {
            case Role.ContingencyPlanner:
                pawnColor = Color.cyan;
                break;
            case Role.Dispatcher:
                pawnColor = Color.magenta;
                break;
            case Role.Medic:
                pawnColor = Color.yellow;
                break;
            case Role.OperationsExpert:
                pawnColor = Color.grey;
                break;
            case Role.QuarantineSpecialist:
                pawnColor = Color.green;
                break;
            case Role.Researcher:
                pawnColor = Color.red;
                break;
            case Role.Scientist:
                pawnColor = Color.white;
                break;
            default:
                break;
        }
        localPlayer.GetComponent<Renderer>().material.SetColor("_Color", pawnColor);

//        GameObject actionPanel = Instantiate(this.actionPanelPrefab);
//        actionPanel.GetComponent<CanvasScript>().init(localPlayer);
//        localPlayer.GetComponent<PlayerScript>().setActionPanel(actionPanel);
//
//        actionPanel.SetActive(false);


		// init event trigger controller and action panel
		this.board.GetComponent<EventTriggerController>().init();
		//this.actionPanel.GetComponent<ActionPanelScript>().init(localPlayer);

		NetworkServer.AddPlayerForConnection (conn, localPlayer, playerControllerId);

    }

}
