using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : CombatStats {
    [SerializeField] private GameManager gameManager;   
    private byte[] UnitCost;
    private byte unitSelected;
    


	// Use this for initialization
	void Start () {
        gameManager = GetComponent<GameManager>();
        UnitCost = new byte[3]; //0: Pawn, 1: Lancer, 2: Horseman
        UnitCost[0] = 50;
        UnitCost[1] = 125;
        UnitCost[2] = 150;        
	}
	
	// Update is called once per frame
	void Update () {
        CreateUnit();
		
	}


    void CreateUnit()
    {
        byte goldToSubstract = 0;

        if(unitSelected == 0)
        {
            goldToSubstract = UnitCost[0];
        }

        else if(unitSelected == 1)
        {
            goldToSubstract = UnitCost[1];
        }

        else if(unitSelected == 2)
        {
            goldToSubstract = UnitCost[2];
        }

        if(gameManager.playersGold[gameManager.turn] - goldToSubstract >= 0)
        {
            gameManager.playersGold[gameManager.turn] -= goldToSubstract;
        }
        
    }

}
