using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour {
    public static GameSetting instance;
    public int noPlayers;
    public int noEPcards;
    public Chanllenge chanllenges;

    void Awake() {
        instance = this;
    }

    public void updateGameSetting(int noPlayers,int noEpcards,Chanllenge challgs) {
        this.noPlayers = noPlayers;
        this.noEPcards = noEpcards;
        this.chanllenges = challgs;
    }
}
