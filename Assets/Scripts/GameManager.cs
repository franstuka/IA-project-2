using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI goldText;
    private byte turn;
    private LinkedList<GameObject> units;
    private byte playerOne;
    private byte playerTwo;
    private int[] playersGold;
    private byte goldPerTurn;
    public int goldIncomeIncrement;

	// Use this for initialization
	void Awake () {        
        turn = 0;
        units = new LinkedList<GameObject>();
        playerOne = 0;
        playerTwo = 1;
        playersGold = new int[2];
        playersGold[playerOne] = 500;
        playersGold[playerTwo] = 500;
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
        goldText.text = "" + playersGold[turn];
    }

    void AddUnits()
    {

    }  

    public void ChangeGold(byte goldCost)
    {
        playersGold[turn] -= goldCost;
        goldText.text = "" + playersGold[turn];
    }

    public int GetPlayersGold(byte player)
    {
        return playersGold[player];
    }

    public byte GetTurn()
    {
        return turn;
    }

    public void SetTurn(byte newTurn)
    {
        this.turn = newTurn;
    }

}
