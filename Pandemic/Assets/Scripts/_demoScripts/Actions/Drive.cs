using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {

    public void driveClicked() {
        
        // init the neighbours button
        GameObject myPlayer = GameObject.Find("_NetworkManager").GetComponent<PlayerNetwork>().myPawn;
        string curCityName = myPlayer.GetComponent<PlayerMovement>().TargetParent;
        GameObject curCity = GameObject.Find(curCityName);
        List<GameObject> neighbours = curCity.GetComponent<City>().adjacentCityList;
        foreach (GameObject neighbour in neighbours) {
            UIButton button = neighbour.GetComponent<UIButton>();
            EventDelegate onclick = new EventDelegate(GameObject.Find("ActionManager").GetComponent<Drive>(), "driveTo");
            EventDelegate.Parameter param = new EventDelegate.Parameter();
            param.value = button;
            param.expectedType = button.GetType();
            onclick.parameters[0] = param;

            EventDelegate.Add(button.onClick, onclick);
        }

    }

    public void driveTo(UIButton button) {
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

}
