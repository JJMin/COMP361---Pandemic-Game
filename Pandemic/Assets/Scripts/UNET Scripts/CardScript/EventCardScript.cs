using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PandemicEnum;

[System.Serializable]
public class EventCardScript : PlayerCardScript {

    public EventCard cardName;

    public void init(EventCard eventName) {
        this.cardName = eventName;
    }

    public EventCard getName() {
        return this.cardName;
    }





}
