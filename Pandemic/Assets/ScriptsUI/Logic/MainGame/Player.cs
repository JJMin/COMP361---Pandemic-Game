using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Job { 
    SCIENTIST,
    WORKER,
    TEACHER
}
public class Player{
    public string userName;
    public int remainAction;
    public Pawn pawn;

    public Job job;

    public Player(Job job) {
        this.job = job;
        
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
