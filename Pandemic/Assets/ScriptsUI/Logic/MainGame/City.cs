using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public enum cityRegion { 
    blue,
    yellow,
    black,
    red
}

public class City: Photon.MonoBehaviour {

    private PhotonView PhotonView;

    public GameObject prefabRS;

    public cityRegion region;
    public UILabel name;
    public List<GameObject> adjacentCityList = new List<GameObject>();
    public bool hasResearchStation = false;
    public int nbOfRS = 1;

    public int yellowDiseaseCube = 0;
    public int blackDiseaseCube = 0;
    public int blueDiseaseCube = 0;
    public int redDiseaseCube = 0;

    private void Awake() {
        PhotonView = GetComponent<PhotonView>();
    }

    public void onClick(){
        GameBoard.Instance.movePawn(GameBoard.Instance.currentPlayer, this);
    }



    public void addResearchStation(string cardName, string city) {
        Debug.Log("ADD RESEARCH STATION CALLED!");
        if (!this.hasResearchStation && this.nbOfRS < 7) {
            Debug.Log("YOU CAN BUILD ONE!!");
            if (cardName != null) {
                Debug.Log("not a operation specialist");
                GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
                myPlayer.GetComponent<PlayerMovement>().DecActions();
                myPlayer.GetComponent<PlayerMovement>().RemoveCard(cardName);
                PhotonView.RPC("RPC_AddRS", PhotonTargets.All, city);
            }
            else {
                Debug.Log("is a operation specialist");
                GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
                myPlayer.GetComponent<PlayerMovement>().DecActions();
                PhotonView.RPC("RPC_AddRS", PhotonTargets.All, city);
            }
        }
        
    }

    [PunRPC]
    public void RPC_AddRS(string cityName) {
        GameObject city = GameObject.Find(cityName);
        this.hasResearchStation = true;
        GameObject rs = Instantiate(prefabRS);
        rs.transform.parent = city.transform;
        rs.transform.localScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
        rs.transform.localPosition = new Vector3(-16, 15, 0);
        this.nbOfRS++;
        Debug.Log("Research Station Created");
    }

    public void removeResearchStation() {
        if (this.hasResearchStation && this.nbOfRS > 0) {
            PhotonView.RPC("RPC_RemoveRS", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void RPC_RemoveRS() {
        this.hasResearchStation = false;
        this.nbOfRS--;
        foreach (Transform rs in gameObject.transform) {
            if (rs.tag == "researchStation") { 
                Destroy(rs);
            }
        }
      
    }

    public bool hasRS() {
        return this.hasResearchStation;
    }


    

}
