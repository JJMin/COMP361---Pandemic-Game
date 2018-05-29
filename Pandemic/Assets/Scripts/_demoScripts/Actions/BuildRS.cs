using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildRS : MonoBehaviour {

    public void buildResearchStationClicked() {
        GameObject hand = GameObject.Find("PlayerHand/Scroll View/Grid");
        GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
        string role = myPlayer.GetComponent<PlayerMovement>().playerRole;
        if (role == "Operations Expert") {
            string curCityName = myPlayer.GetComponent<PlayerMovement>().TargetParent;
            GameObject curCity = GameObject.Find(curCityName);
            curCity.GetComponent<City>().addResearchStation(null, curCity.name);
        }
        else { 
            // check if player has its current city cityCard
            foreach (Transform card in hand.transform) {
                if (card.tag == "CityCard" && card.GetComponent<CityCards>().getCity().name.Equals(myPlayer.GetComponent<PlayerMovement>().TargetParent)) {
                    // construct research station
                    card.GetComponent<CityCards>().getCity().GetComponent<City>().addResearchStation(card.name, card.GetComponent<CityCards>().getCity().name);
                    break;
                }
            }
        }
        
    }
}
