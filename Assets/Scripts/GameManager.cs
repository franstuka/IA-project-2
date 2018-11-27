using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {   
    public byte turn;
    private byte playerOne;
    private byte playerTwo;
    public int[] playersGold;
    private byte goldPerTurn;
    public int goldIncomeIncrement;

	// Use this for initialization
	void Awake () {        
        turn = 0;
        playerOne = 0;
        playerTwo = 1;        
        goldPerTurn = 100;
        goldIncomeIncrement = 0;             
	}
	
	// Update is called once per frame
	void Update () {   
        ChangeTurn();       
		
	}


    void ChangeTurn()
    {
        UpdateGold();
        AddUnits();

    }

    void UpdateGold()
    {
        /*for(int i = 0; i < Unit.Length; i++)
        {
            if(Unit[i].Type == Pawn && Unit[i].isMining == true){
                goldIncomeIncrement += 25;
            }
        }*/
        playersGold[turn] += goldPerTurn + goldIncomeIncrement;       
    }

    void AddUnits()
    {

    }  

    public void ChangeGold(byte goldCost)
    {
        playersGold[turn] -= goldCost;       
    }


}
