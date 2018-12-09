using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Units {
    
    private byte towerCost = 200;
    private bool isMining;
    [SerializeField] private byte working = 0;

    private Vector2Int workZone;

    [SerializeField] GameObject tower;

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
    public void Work()
    {
        if (working > 0)
        {
            working--;
            if (working == 0)
            {
                CreateTower();
            }
        }
    }

    public void StartConstruction(Vector2Int workZone)
    {
        this.workZone = workZone;
        working = 3;
    }

    void CreateTower()
    {
        if(GameManager.instance.GetPlayersGold(GameManager.instance.GetTurn()) - towerCost >= 0)
        {
            GameManager.instance.ChangeGold(towerCost);
            //Vector2Int posicion = GridMap.instance.CellCordFromWorldPoint(pos.transform.position);
            //Instantiate(tower, new Vector3(posicion.x, posicion.y, 0) , Quaternion.identity); 
            Instantiate(tower, new Vector3(workZone.x, workZone.y, 0), Quaternion.identity);
            workZone = new Vector2Int();
        }
    }

    public byte GetWorking()
    {
        return working;
    }
    public bool GetIsMining()
    {
        return isMining;
    }
}
