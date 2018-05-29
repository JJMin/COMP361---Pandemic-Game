using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleFlight : MonoBehaviour {

    public void shuttleFlightClicked() {
        // check if player is in a city with a research station
        GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
        string curCityName = myPlayer.GetComponent<PlayerMovement>().TargetParent;
        GameObject curCity = GameObject.Find(curCityName);
        // check if player has its current city cityCard and init button
        if (curCity.GetComponent<City>().hasRS()) {
            GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
            foreach (GameObject city in cities) {
                if (city.GetComponent<City>().hasRS()) { 
                    UIButton button = city.GetComponent<UIButton>();
                    EventDelegate onclick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShuttleFlight>(), "takeShuttleFlight");
                    EventDelegate.Parameter param = new EventDelegate.Parameter();
                    param.value = button;
                    param.expectedType = button.GetType();
                    onclick.parameters[0] = param;

                    EventDelegate.Add(button.onClick, onclick);
                }
                
            }
        }
    }

    public void takeShuttleFlight(UIButton button) {
        // update position
        GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
        GameObject newCity = button.tweenTarget;
        myPlayer.GetComponent<PlayerMovement>().TargetParent = newCity.name;
        myPlayer.transform.parent = newCity.transform;
        myPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
        myPlayer.transform.localPosition = new Vector3(0, 0, 0);
        // remove button eventDelegate
        GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
        foreach (GameObject city in cities) {
            UIButton toRemoveButton = city.GetComponent<UIButton>();
            toRemoveButton.onClick.Clear();
        }
        // decrease action left
		myPlayer.GetComponent<PlayerMovement> ().DecActions ();
    }

    /*
     foreach (Transform card in hand.transform) {
            if (card.tag == "CityCard" && card.GetComponent<CityCards>().getCity().name.Equals(myPlayer.GetComponent<PlayerMovement>().TargetParent)) {
                // init every city with research station button
                GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
                foreach (GameObject city in cities) {
                    if (city.GetComponent<City>().hasRS()) {
                        UIButton button = city.GetComponent<UIButton>();
                        EventDelegate onclick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<ShuttleFlight>(), "takeShuttleFlight");
                        EventDelegate.Parameter param = new EventDelegate.Parameter();
                        param.value = button;
                        param.expectedType = button.GetType();
                        onclick.parameters[0] = param;
                        EventDelegate.Parameter param2 = new EventDelegate.Parameter();
                        param2.value = card.name;
                        param2.expectedType = card.name.GetType();
                        onclick.parameters[1] = param2;

                        EventDelegate.Add(button.onClick, onclick);
                    }
                }
            }
        } 
     */


}


