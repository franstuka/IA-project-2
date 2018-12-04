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


        public unitData(string unit, byte tier, float maxForce, float force, float maxDamage, byte turn)
        {
            this.unit = unit;
            this.tier = tier;
            this.maxForce = maxForce;
            this.force = force;
            this.maxDamage = maxDamage;
            this.turn = turn;
        }
    }

    [SerializeField] private GameObject peon;
    [SerializeField] private GameObject caballero;
    [SerializeField] private GameObject lancero;
    [SerializeField] private GameObject general;

    [SerializeField] private LinkedList<unitData> units;


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

    public void SaveUnit(CombatStats unit)
    {
        unitData data = new unitData(unit.GetComponentInParent<GameObject>().name , unit.GetTier(), unit.GetMaxForce(), unit.GetForce(), unit.GetMaxDamage(), 0);
        units.AddLast(data);
    }

    public void CreateUnit(UnitType type, CombatStats torre)
    {
        if (slots <= maxSlots)
        {
            byte goldToSubstract = 0;
            GameObject piece = null;
            byte turns = 0;
            unitData data;

            switch (type)
            {
                case UnitType.Peon:
                    goldToSubstract = UnitCost[0];
                    piece = peon;
                    turns = 1;
                    break;

                case UnitType.Lancero:
                    goldToSubstract = UnitCost[1];
                    piece = lancero;
                    turns = 2;
                    break;

                case UnitType.Caballeria:
                    goldToSubstract = UnitCost[2];
                    piece = caballero;
                    turns = 2;
                    break;

                case UnitType.General:
                    goldToSubstract = UnitCost[3];
                    piece = general;
                    turns = 3;
                    break;

                default:
                    break;
            }
            if (piece)
            {
                if (GameManager.instance.GetPlayersGold(GameManager.instance.GetTurn()) - goldToSubstract >= 0)
                {
                    GameManager.instance.ChangeGold(goldToSubstract);
                    slots++;

                    data = new unitData(piece.name, piece.GetComponent<CombatStats>().GetTier(), piece.GetComponent<CombatStats>().GetMaxForce(), piece.GetComponent<CombatStats>().GetForce(), piece.GetComponent<CombatStats>().GetMaxDamage(), turns);

                    /*Vector2Int posTorre = GridMap.instance.CellCordFromWorldPoint(torre.transform.position);
                    Instantiate(piece, new Vector3 (posTorre.x, posTorre.y, 0) , Quaternion.identity);*/
                }
            }
        }                                            
    }

    public void SpawnUnit(Vector2Int pos)
    {

    }
}
