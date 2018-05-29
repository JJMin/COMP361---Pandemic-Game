using UnityEngine;

public class DDOL : MonoBehaviour {
	
	private void Awake() {
		DontDestroyOnLoad(this);	//this makes sure the this object is not destroyed between scenes
	}

}
