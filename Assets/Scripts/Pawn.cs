using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Units {
    
    private byte towerCost = 200;
    private bool isMining;

    // Use this for initialization
    void Start () {      
        isMining = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("g"))
        {
            //createTowee( );
        }

    }

    void CreateTower(CombatStats pos)
    {
        if(GameManager.instance.GetPlayersGold(GameManager.instance.GetTurn()) - towerCost >= 0)
        {
            GameManager.instance.ChangeGold(towerCost);
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
