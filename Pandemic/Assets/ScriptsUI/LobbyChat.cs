using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChat : MonoBehaviour {
    public UIInput chatInput;
    public GameObject chatPrefab;
    public UIGrid grid;
    //when click send button in chatlobby
    public void OnCickSendInLobbyChatBar() {
        if(chatInput.value!=""){
            GameObject chatItem = NGUITools.AddChild(grid.gameObject, chatPrefab);
            chatItem.transform.Find("content").GetComponent<UILabel>().text = chatInput.value;//更新显示
            grid.AddChild(chatItem.transform);
            chatInput.value = "";
        }
       
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
