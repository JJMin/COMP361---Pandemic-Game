using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
    public GameObject pawnPrefab;
    public static GameBoard Instance;
    public City BornPlace;
    public List<Pawn> PawnList=new List<Pawn>();
    public Player currentPlayer;
    void Awake() {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
//        Player p1 = new Player(Job.SCIENTIST);
//        
//        Player p2 = new Player(Job.TEACHER);
//       
//        movePawn(p1, BornPlace);
//        movePawn(p2, BornPlace);
//        currentPlayer = p1;
	}

    //移动player的棋子到dest城市
    public void movePawn(Player player,City dest) {
       
        if(player.pawn!=null) GameObject.Destroy(player.pawn.gameObject);//销毁当前棋子
        GameObject pawn = generatePawn(player,dest);


        
    }
    //在目标城市生成角色棋子
    public GameObject generatePawn(Player p,City dest) {
      
       Pawn[] pawnArr= dest.gameObject.GetComponentsInChildren<Pawn>();//获取目标城市上所有的棋子
       int nbPawn = pawnArr.Length;//棋子个数

        GameObject pawn=NGUITools.AddChild(dest.gameObject, pawnPrefab);

        p.pawn=pawn.GetComponent<Pawn>();
        pawn.GetComponent<Pawn>().player = p;
        pawn.GetComponent<Pawn>().setPawnColor();//设置棋子颜色

        pawn.transform.localPosition = new Vector3(nbPawn*10, 30,0 );
       
        return pawn;

    }
    public void OnClickMove() { 
        
    }
	// Update is called once per frame
	void Update () {
		
	}
}
