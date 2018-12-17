using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : CombatStats {
    
    private const byte maxSlots = 3; 
    private byte[] contTypes;
    private byte[] UnitCost;  
    private byte unitSelected;
    private byte range;
    private byte slots = 0;
    [SerializeField] bool actionAvaliable = true;
    private struct unitData{
        //string unit;
        UnitType type; 
        byte tier;
        float maxForce;
        float force;
        float maxDamage;
        byte turn;


        public unitData(UnitType type, byte tier, float maxForce, float force, float maxDamage, byte turn)
        {
            this.type = type;
            this.tier = tier;
            this.maxForce = maxForce;
            this.force = force;
            this.maxDamage = maxDamage;
            this.turn = turn;
        }

        public UnitType GetUnitType()
        {
            return type;
        }

        public byte GetUnitTier()
        {
            return tier;
        }

        public float GetUnitMaxForce()
        {
            return maxForce;
        }

        public float GetUnitForce()
        {
            return force;
        }

        public float GetUnitMaxDamage()
        {
            return maxDamage;
        }

        public byte GetUnitTurn()
        {
            return turn;
        }
    }

    [SerializeField] private GameObject peon;
    [SerializeField] private GameObject caballero;
    [SerializeField] private GameObject lancero;
    [SerializeField] private GameObject general;
    [SerializeField] private LinkedList<unitData> units = new LinkedList<unitData>();


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
	/*void Update () {
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

    }*/

    public void SaveUnit(CombatStats unit, byte turns,byte coste)
    {
        if(slots < maxSlots)
        {
            if (GameManager.instance.GetPlayersGold(GameManager.instance.GetTurn()) - coste >= 0)
            {
                GameManager.instance.ChangeGold(coste);
                slots++;
                Debug.Log(unit == null);
                //unitData data = new unitData(unit.GetComponentInParent<GameObject>().name , unit.GetTier(), unit.GetMaxForce(), unit.GetForce(), unit.GetMaxDamage(), 0);
                unitData data = new unitData(unit.GetUnityType(), unit.GetTier(), unit.GetMaxForce(), unit.GetForce(), unit.GetMaxDamage(), turns);
                units.AddLast(data);
                if (turns == 0)
                {
                    unit.GetComponent<PlayerMovement>().UnSelect();
                    Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(unit.transform.position);
                    GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = null;
                    List<LinkedList<GameObject>> unitlist = GameManager.instance.GetUnitList();
                    //LinkedListNode<GameObject> node = unitlist[unit.GetTeam()].Find(unit.GetComponent<GameObject>());
                    Debug.Log(unitlist[unit.GetTeam()].Count);
                    unitlist[unit.GetTeam()].Remove(unit.gameObject);
                    Debug.Log(unitlist[unit.GetTeam()].Count);
                    Destroy(unit.gameObject);
                }
            }

        }
    }
    public void GenerateUnit(byte slot, Vector2Int posicion)
    {
        //Debug.Log(slot);
        //Debug.Log(slots);

        if (slot < slots) {

            LinkedListNode<unitData> node = units.First;
            for (int i = 0; i < slot; i++)
            {
                node = node.Next;
            }

            unitData data = node.Value;
            GameObject piece = new GameObject();

            switch (data.GetUnitType())
            {
                case UnitType.Peon:
                    piece = peon;
                    break;

                case UnitType.Lancero: 
                    piece = lancero;
                    break;

                case UnitType.Caballeria:
                    piece = caballero;
                    break;

                case UnitType.General:
                    piece = general;
                    
                    break;

                default:
                    break;
            }

            Cell cellAux = GridMap.instance.grid[posicion.x, posicion.y];

            GameObject newPiece = Instantiate(piece, cellAux.GlobalPosition, Quaternion.identity);
            //Debug.Log("¡¡STOY VIVIO JOSDEPUTA!!");

            cellAux.unityOrConstructionOnCell = newPiece.GetComponent<CombatStats>();
            List<LinkedList<GameObject>> list = GameManager.instance.GetUnitList();
            list[team].AddLast(newPiece);

            newPiece.GetComponent<CombatStats>().SetStats(CombatStatsType.DAMAGE, data.GetUnitMaxDamage());
            newPiece.GetComponent<CombatStats>().SetStats(CombatStatsType.MAXDAMAGE, data.GetUnitMaxDamage());
            newPiece.GetComponent<CombatStats>().SetStats(CombatStatsType.FORCE, data.GetUnitForce());
            newPiece.GetComponent<CombatStats>().SetStats(CombatStatsType.MAXFORCE, data.GetUnitMaxForce());
            newPiece.GetComponent<CombatStats>().SetStatTier(data.GetUnitTier());

            units.Remove(data);
            slots--;
        }
    }

    public void CreateUnit(UnitType type, CombatStats torre)
    {
        if (slots <= maxSlots)
        {
            byte goldToSubstract = 0;
            GameObject piece = null;
            byte turns = 0;
            //unitData data;

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
                SaveUnit(piece.GetComponent<CombatStats>(), turns, goldToSubstract);
                Debug.Log("Unidad Creada");
                // slots++;

                //data = new unitData(piece.GetComponent<CombatStats>().GetUnityType(), piece.GetComponent<CombatStats>().GetTier(), piece.GetComponent<CombatStats>().GetMaxForce(), piece.GetComponent<CombatStats>().GetForce(), piece.GetComponent<CombatStats>().GetMaxDamage(), turns);

                /*Vector2Int posTorre = GridMap.instance.CellCordFromWorldPoint(torre.transform.position);
                Instantiate(piece, new Vector3 (posTorre.x, posTorre.y, 0) , Quaternion.identity);*/
            }
           
        }                                            
    }

    public List<UnitType> UnitsInside()
    {
        List<UnitType> list = new List<UnitType>();

        for (LinkedListNode<unitData> node = units.First; node != null; node = node.Next)
        {
            list.Add(node.Value.GetUnitType());
        }

        return list;
    } 

    public void SetActionAvaliable()
    {
        actionAvaliable = true;
    }

    public void SetActionUnavaliable()
    {
        actionAvaliable = false;
    }

    public bool GetActionAvaliable()
    {
        return actionAvaliable;
    }
    
    public void SpawnUnit(Vector2Int pos)
    {

    }
}
