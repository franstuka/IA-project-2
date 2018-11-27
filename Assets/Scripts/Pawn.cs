﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Units {
    [SerializeField] private GameManager gameManager;
    private byte towerCost;
    private bool isMining;

    // Use this for initialization
    void Start () {
        gameManager = GetComponent<GameManager>();
        towerCost = 200;
        isMining = false;
    }
	
	// Update is called once per frame
	void Update () {
        CreateTower();        
	}

    void CreateTower()
    {
        if(gameManager.GetPlayersGold(gameManager.GetTurn()) - towerCost >= 0)
        {
            gameManager.ChangeGold(towerCost);            
        }
    }
}
