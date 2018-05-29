using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolHelper : MonoBehaviour {

    public static void Shuffle<T>(List<T> list){
       
        for (int i = 0; i < list.Count;i++ ) {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

    }
	
}
