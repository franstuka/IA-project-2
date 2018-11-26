using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public byte turn;
    private byte playerOne;
    private byte playerTwo;
    public int[] playersGold;    

	// Use this for initialization
	void Start () {
        turn = 0;
        playerOne = 0;
        playerTwo = 1;
        playersGold = new int[2];
        playersGold[playerOne] = 500;
        playersGold[playerTwo] = 500;       
	}
	
	// Update is called once per frame
	void Update () {
        ChangeTurn();
        ChangeGold();  
		
	}


    void ChangeTurn()
    {
        UpdateGold();
        AddUnits();

    }

    void UpdateGold()
    {

    }

    void AddUnits()
    {

    }  

    void ChangeGold()
    {

    }


}
