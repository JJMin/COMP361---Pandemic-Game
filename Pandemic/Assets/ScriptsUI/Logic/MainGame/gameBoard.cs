using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//挂在uiroot上
//玩家牌由城市牌 事件牌和流行病牌组成
public class gameBoard : MonoBehaviour {

    public List<Card> playerCardList = new List<Card>();
    public List<Card> playerCardDiscardList;
    public List<Card> infectionCardList;
    public List<Card> infectionDiscardList;
   

    //初始化玩家牌堆
    public void InitplayerCardList() {
        //初始化48张城市卡到playercardlist里
       GameObject[] citys= GameObject.FindGameObjectsWithTag("City");
       List<GameObject> cityCardsList = new List<GameObject>(citys);
       toolHelper.Shuffle(cityCardsList);
       int i = 0;
        for (i = 0; i < 48;i++ ) {
            CityCard cityCard = new CityCard(cityCardsList[i].GetComponent<City>().name.text);
            playerCardList.Add(cityCard);
        }
        //初始化x张事件卡到playercardlist里根据是否玩otb版本
             //普通版的5张
        GameObject[] eventCards = GameObject.FindGameObjectsWithTag("EventCard");
        List<GameObject> eventCardsList =  new List<GameObject>(eventCards);
        if (GameSetting.instance.chanllenges == Chanllenge.NULL)
        {
            
            for (int j = 0; j < 5; j++)
            {
                EventCard eventCard = new EventCard(eventCards[j].name);
                playerCardList.Add(eventCard);
            }
        }
             //OTB规定事件 实际事件卡数量为玩家数*2
        else
        {
            int NoPlayers = GameSetting.instance.noPlayers;
            GameObject[] eventCardsOTBarr = GameObject.FindGameObjectsWithTag("EventCardOTB");//获取八张OTB事件卡array
            List<GameObject> eventCardsOTBlist=new List<GameObject>(eventCardsOTBarr);//获取八张OTB事件卡list
            eventCardsOTBlist.AddRange(eventCardsList);//获取共13张
            toolHelper.Shuffle(eventCardsOTBlist);//洗牌
            for (int j = 0; j < NoPlayers*2; j++ ) {

                EventCard eventCardOTB = new EventCard(eventCardsOTBlist[j].name);
               // Debug.Log(eventCardOTB.eventCardName);
                playerCardList.Add(eventCardOTB);
            }
           
        }
       //初始化流行病卡
        if(GameSetting.instance.chanllenges==Chanllenge.NULL){

        }

    }
   

    // Use this for initialization
	void Start () {
        InitplayerCardList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
