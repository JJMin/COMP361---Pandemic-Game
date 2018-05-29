using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectFlight : MonoBehaviour {

    public void directFlightClicked() {
        // init the cityCard in hands button
        GameObject hand = GameObject.Find("PlayerHand/Scroll View/Grid");
        
        foreach (Transform card in hand.transform) {
            if (card.tag == "CityCard") {
                foreach (Transform sprite in card.transform) {
                    UIButton button = sprite.GetComponent<UIButton>();
                    EventDelegate onclick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<DirectFlight>(), "takeDirectFlight");
                    EventDelegate.Parameter param = new EventDelegate.Parameter();
                    EventDelegate.Parameter param2 = new EventDelegate.Parameter();
                    param.value = card.GetComponent<CityCards>().getCity();
                    param2.value = card.name;
                    param.expectedType = card.GetComponent<CityCards>().getCity().GetType();
                    param2.expectedType = card.name.GetType();
                    onclick.parameters[0] = param;
                    onclick.parameters[1] = param2;

                    EventDelegate.Add(button.onClick, onclick);
                }
            }
        }
    }

    public void takeDirectFlight(GameObject newCity, string cityCardName) {
        // remove button eventDelegate
        GameObject hand = GameObject.Find("PlayerHand/Scroll View/Grid");

        foreach (Transform card in hand.transform) {
            if (card.tag == "CityCard") {
                foreach (Transform sprite in card.transform) {
                    UIButton button = sprite.GetComponent<UIButton>();
                    button.onClick.Clear();
                }
            }
        }

        // move player
        GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
        myPlayer.GetComponent<PlayerMovement>().TargetParent = newCity.name;
        myPlayer.transform.parent = newCity.transform;
        myPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
        myPlayer.transform.localPosition = new Vector3(0, 0, 0);

        // discard card
        Debug.Log("Calling Remove Card");
        myPlayer.GetComponent<PlayerMovement>().RemoveCard(cityCardName);

        // decrease action left
		myPlayer.GetComponent<PlayerMovement> ().DecActions ();
    }

}
