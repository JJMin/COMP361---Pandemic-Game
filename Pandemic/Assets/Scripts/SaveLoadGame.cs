using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadGame {

	//public static List<GameScript> savedGames = new List<GameScript>();
	public static MyGames game = new MyGames();

	//it's static so we can call it from anywhere
	public static void Save() {
		
		if(game.username.Equals(string.Empty)){
			game.username = playerDetails.Instance.username;
		}

		SaveLoadGame.game.savedGames.Add(GameScript.current);
		BinaryFormatter bf = new BinaryFormatter();

		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/SavedGames.data"); //you can call it anything you want
		bf.Serialize(file, SaveLoadGame.game);
		file.Close();
	}   

	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/SavedGames.data")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/SavedGames.data", FileMode.Open);
			SaveLoadGame.game = (MyGames) bf.Deserialize(file);
			file.Close();
		}
	}
}

[System.Serializable]
public class MyGames{
	
	public string username;
	public List<GameScript> savedGames;

	public MyGames(){
		savedGames = new List<GameScript>();
	}
}