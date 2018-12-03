using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Units {
    [SerializeField] private GameManager gameManager;
    private byte towerCost = 200;
    private bool isMining;

    // Use this for initialization
    void Start () {
        gameManager = GetComponent<GameManager>();
        isMining = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("g"))
        {
            //createTower( );
        }

    }

    void CreateTower(CombatStats pos)
    {
        if(gameManager.GetPlayersGold(gameManager.GetTurn()) - towerCost >= 0)
        {
            gameManager.ChangeGold(towerCost);
            GameObject torre = GameObject.Find("Torre");
            Vector2Int posicion = GridMap.instance.CellCordFromWorldPoint(pos.transform.position);
            Instantiate(torre, new Vector3(posicion.x, posicion.y, 0) , Quaternion.identity);          
        }
    }

    public bool GetIsMining()
    {
        return isMining;
    }
}
