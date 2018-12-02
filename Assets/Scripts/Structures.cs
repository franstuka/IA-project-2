using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : CombatStats {
    
    private const byte maxSlots = 3; 
    private byte[] contTypes;
    private byte[] UnitCost;  
    private byte unitSelected;
    private byte range;
    private byte slots;   
    private struct unitData{
        string unit;
        byte tier;
        float maxForce;
        float force;
        float maxDamage;
        byte turn;
    }

    private LinkedList<unitData> units;


    // Use this for initialization
    void Start () {
        contTypes = new byte[3];
        UnitCost = new byte[4]; //0: Pawn, 1: Lancer, 2: Horseman, 3: General
        UnitCost[0] = 50;
        UnitCost[1] = 125;
        UnitCost[2] = 150;
        UnitCost[3] = 200;    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("a")) {
           //createUnit(UnitType.Peon, );
        }
        if (Input.GetKey("s")){
           //createUnit(UnitType.Lancero, );
        }

        if (Input.GetKey("d")){
           //createUnit(UnitType.Caballeria, );
        }

        if (Input.GetKey("f")){
           //createUnit(UnitType.General, );
        }

    }

    void CreateUnit(UnitType type, CombatStats torre)
    {
        if (slots <= maxSlots)
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

            if (GameManager.instance.GetPlayersGold(GameManager.instance.GetTurn()) - goldToSubstract >= 0)
            {
                GameManager.instance.ChangeGold(goldToSubstract);
                slots++;
                Vector2Int posTorre = GridMap.instance.CellCordFromWorldPoint(torre.transform.position);
                Instantiate(piece, new Vector3 (posTorre.x, posTorre.y, 0) , Quaternion.identity);                              
            }                                    
        }                                            
    }
}
