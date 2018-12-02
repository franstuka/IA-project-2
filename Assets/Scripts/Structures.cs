using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : CombatStats {


    [SerializeField] private GameManager gameManager;
    private const byte MaxSlots = 3;
    private UnitType[] slotsType;
    private byte[] contTypes;
    private byte[] UnitCost;  
    private byte unitSelected;
    private byte range;
    private byte slots;  
    private bool isCastle;
    


	// Use this for initialization
	void Start () {
        gameManager = GetComponent<GameManager>();
        slotsType = new UnitType[3];
        contTypes = new byte[3];
        UnitCost = new byte[4]; //0: Pawn, 1: Lancer, 2: Horseman, 3: General
        UnitCost[0] = 50;
        UnitCost[1] = 125;
        UnitCost[2] = 150;
        UnitCost[3] = 200;    
	}
	
	// Update is called once per frame
	void Update () {    
           		
	}

    void CreateUnit(UnitType type, CombatStats torre)
    {
        if (slots <= MaxSlots)
        {
            byte goldToSubstract = 0;
            GameObject piece = new GameObject();

            switch (type)
            {
                case UnitType.Peon:
                    goldToSubstract = UnitCost[0];
                    piece = GameObject.Find("Peon");                   
                    break;

                case UnitType.Lancero:
                    goldToSubstract = UnitCost[1];
                    piece = GameObject.Find("Lancero");
                    break;

                case UnitType.Caballeria:
                    goldToSubstract = UnitCost[2];
                    piece = GameObject.Find("Caballeria");
                    break;

                case UnitType.General:
                    goldToSubstract = UnitCost[3];
                    piece = GameObject.Find("General");
                    break;             
            }

            if (gameManager.GetPlayersGold(gameManager.GetTurn()) - goldToSubstract >= 0)
            {
                gameManager.ChangeGold(goldToSubstract);
                slots++;
                Vector2Int posTorre = GridMap.instance.CellCordFromWorldPoint(torre.transform.position);
                Instantiate(piece, new Vector3 (posTorre.x, posTorre.y, 0) , Quaternion.identity);                              
            }                                    
        }                                            
    }
}
