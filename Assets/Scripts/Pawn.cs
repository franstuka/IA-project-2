using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Units {
    [SerializeField] private GameManager gameManager;
    private byte towerCost;

    // Use this for initialization
    void Start () {
        gameManager = GetComponent<GameManager>();
        towerCost = 200;
    }
	
	// Update is called once per frame
	void Update () {
        CreateTower();
	}

    void CreateTower()
    {
        if(gameManager.playersGold[gameManager.turn] - towerCost >= 0)
        {
            gameManager.playersGold[gameManager.turn] -= towerCost;
        }
    }
}
