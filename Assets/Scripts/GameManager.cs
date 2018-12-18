using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    #region singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of grid is trying to active");
            return;
        }
        instance = this;
        units = new List<LinkedList<GameObject>>();
        for (int i = 0; i < playersNum; i++)
        {
            //units[i] = new LinkedList<GameObject>();
            units.Add(new LinkedList<GameObject>());
        }
        playersGold = new int[playersNum];

        ///////////////////// 
        int name = 0;

        //Peones Negros

        Vector3 posicion = GridMap.instance.grid[15, 19].GlobalPosition;
        GameObject Peon = Instantiate(peon, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
        Peon.name = "Peon" + name;
        name++;
        units[1].AddLast(Peon);

        Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(Peon.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Peon.GetComponent<CombatStats>();

        Vector3 posicion2 = GridMap.instance.grid[16, 18].GlobalPosition;
        GameObject Peon2 = Instantiate(peon, new Vector3(posicion2.x, 1, posicion2.z), Quaternion.identity);
        Peon2.name = "Peon" + name;
        units[1].AddLast(Peon2);

        Vector2Int coord2 = GridMap.instance.CellCordFromWorldPoint(Peon2.transform.position);
        GridMap.instance.grid[coord2.x, coord2.y].unityOrConstructionOnCell = Peon2.GetComponent<CombatStats>();

        //3
        posicion = GridMap.instance.grid[8, 15].GlobalPosition;
        Peon = Instantiate(peon, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
        units[1].AddLast(Peon);

        coord = GridMap.instance.CellCordFromWorldPoint(Peon.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Peon.GetComponent<CombatStats>();

        //4

        posicion = GridMap.instance.grid[7, 15].GlobalPosition;
        Peon = Instantiate(peon, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
        units[1].AddLast(Peon);

        coord = GridMap.instance.CellCordFromWorldPoint(Peon.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Peon.GetComponent<CombatStats>();

        //Torre Negra
        //1

        posicion = GridMap.instance.grid[15, 18].GlobalPosition;
        GameObject TorreN = Instantiate(torre, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
        units[1].AddLast(TorreN);

        coord = GridMap.instance.CellCordFromWorldPoint(TorreN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = TorreN.GetComponent<CombatStats>();

        //2
        posicion = GridMap.instance.grid[21, 7].GlobalPosition;
        TorreN = Instantiate(torre, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(TorreN);

        coord = GridMap.instance.CellCordFromWorldPoint(TorreN.transform.position);

        //3

        posicion = GridMap.instance.grid[17, 7].GlobalPosition;
        TorreN = Instantiate(torre, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(TorreN);

        coord = GridMap.instance.CellCordFromWorldPoint(TorreN.transform.position);

        //4

        posicion = GridMap.instance.grid[21, 11].GlobalPosition;
        TorreN = Instantiate(torre, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(TorreN);

        coord = GridMap.instance.CellCordFromWorldPoint(TorreN.transform.position);

        //5

        posicion = GridMap.instance.grid[17, 11].GlobalPosition;
        TorreN = Instantiate(torre, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(TorreN);

        coord = GridMap.instance.CellCordFromWorldPoint(TorreN.transform.position);

        //Caballos Negros
        //1

        posicion = GridMap.instance.grid[16, 25].GlobalPosition;
        GameObject CaballeroN = Instantiate(caballero, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(CaballeroN);

        coord = GridMap.instance.CellCordFromWorldPoint(CaballeroN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = CaballeroN.GetComponent<CombatStats>();
        //2

        posicion = GridMap.instance.grid[17, 25].GlobalPosition;
        CaballeroN = Instantiate(caballero, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(CaballeroN);

        coord = GridMap.instance.CellCordFromWorldPoint(CaballeroN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = CaballeroN.GetComponent<CombatStats>();
        //3

        posicion = GridMap.instance.grid[18, 25].GlobalPosition;
        CaballeroN = Instantiate(caballero, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(CaballeroN);

        coord = GridMap.instance.CellCordFromWorldPoint(CaballeroN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = CaballeroN.GetComponent<CombatStats>();

        //Lancero Negros
        //1
        posicion = GridMap.instance.grid[18, 23].GlobalPosition;
        GameObject LanceroN = Instantiate(lancero, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(LanceroN);

        coord = GridMap.instance.CellCordFromWorldPoint(LanceroN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = LanceroN.GetComponent<CombatStats>();

        //2

        posicion = GridMap.instance.grid[16, 23].GlobalPosition;
        LanceroN = Instantiate(lancero, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(LanceroN);

        coord = GridMap.instance.CellCordFromWorldPoint(LanceroN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = LanceroN.GetComponent<CombatStats>();

        //Generales Negros
        //1

        posicion = GridMap.instance.grid[17, 17].GlobalPosition;
        GameObject GeneralN = Instantiate(general, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(GeneralN);

        coord = GridMap.instance.CellCordFromWorldPoint(GeneralN.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = GeneralN.GetComponent<CombatStats>();

        //Peones Blancos
        //1
        Vector3 posicion3 = GridMap.instance.grid[17, 45].GlobalPosition;
        GameObject Peon3 = Instantiate(peonw, new Vector3(posicion3.x, 1, posicion3.z), Quaternion.identity);

        units[1].AddLast(Peon3);

        Vector2Int coord3 = GridMap.instance.CellCordFromWorldPoint(Peon3.transform.position);
        GridMap.instance.grid[coord3.x, coord3.y].unityOrConstructionOnCell = Peon3.GetComponent<CombatStats>();
        //2
        posicion3 = GridMap.instance.grid[19, 45].GlobalPosition;
        Peon3 = Instantiate(peonw, new Vector3(posicion3.x, 1, posicion3.z), Quaternion.identity);

        units[1].AddLast(Peon3);

        coord3 = GridMap.instance.CellCordFromWorldPoint(Peon3.transform.position);

        // Caballeros Blancos

        //1
        posicion = GridMap.instance.grid[4, 20].GlobalPosition;
        GameObject Caballero = Instantiate(caballerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Caballero);

        coord = GridMap.instance.CellCordFromWorldPoint(Caballero.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Caballero.GetComponent<CombatStats>();

        //2
        posicion = GridMap.instance.grid[15,17].GlobalPosition;
        Caballero = Instantiate(caballerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Caballero);

        coord = GridMap.instance.CellCordFromWorldPoint(Caballero.transform.position);
        Debug.Log(coord);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Caballero.GetComponent<CombatStats>();

        //3
        posicion = GridMap.instance.grid[15, 43].GlobalPosition;
        Caballero = Instantiate(caballerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Caballero);

        coord = GridMap.instance.CellCordFromWorldPoint(Caballero.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Caballero.GetComponent<CombatStats>();

        //Lanceros Blancos
        //1
        posicion = GridMap.instance.grid[15, 16].GlobalPosition;
        GameObject Lancero = Instantiate(lancerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Lancero);

        coord = GridMap.instance.CellCordFromWorldPoint(Lancero.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Lancero.GetComponent<CombatStats>();

        //2

        posicion = GridMap.instance.grid[16, 35].GlobalPosition;
        Lancero = Instantiate(lancerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Lancero);

        coord = GridMap.instance.CellCordFromWorldPoint(Lancero.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Lancero.GetComponent<CombatStats>();
        //3

        posicion = GridMap.instance.grid[20, 35].GlobalPosition;
        Lancero = Instantiate(lancerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Lancero);

        coord = GridMap.instance.CellCordFromWorldPoint(Lancero.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Lancero.GetComponent<CombatStats>();

        //4

        posicion = GridMap.instance.grid[19, 35].GlobalPosition;
        Lancero = Instantiate(lancerow, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Lancero);

        coord = GridMap.instance.CellCordFromWorldPoint(Lancero.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Lancero.GetComponent<CombatStats>();

        //General Blanco
        //1

        posicion = GridMap.instance.grid[18, 45].GlobalPosition;
        GameObject General = Instantiate(generalw, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(General);

        coord = GridMap.instance.CellCordFromWorldPoint(General.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = General.GetComponent<CombatStats>();

        //Torres Blancas
        //1

        posicion = GridMap.instance.grid[20, 45].GlobalPosition;
        GameObject Torre = Instantiate(torrew, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Torre);

        coord = GridMap.instance.CellCordFromWorldPoint(Torre.transform.position);

        //2

        posicion = GridMap.instance.grid[21, 48].GlobalPosition;
        Torre = Instantiate(torrew, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Torre);

        coord = GridMap.instance.CellCordFromWorldPoint(Torre.transform.position);

        //3

        posicion = GridMap.instance.grid[17, 48].GlobalPosition;
        Torre = Instantiate(torrew, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Torre);

        coord = GridMap.instance.CellCordFromWorldPoint(Torre.transform.position);

        //4

        posicion = GridMap.instance.grid[21, 52].GlobalPosition;
        Torre = Instantiate(torrew, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Torre);

        coord = GridMap.instance.CellCordFromWorldPoint(Torre.transform.position);

        //5

        posicion = GridMap.instance.grid[17, 52].GlobalPosition;
        Torre = Instantiate(torrew, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[0].AddLast(Torre);

        coord = GridMap.instance.CellCordFromWorldPoint(Torre.transform.position);

    }
    #endregion

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private byte playersNum = 2;
    [SerializeField] private int[] playersGold;
    [SerializeField] private byte turn = 1;
    [SerializeField] private int initialGold = 500;
    [SerializeField] private int baseGoldWin = 50;
    private int minesDigged = 0;
    public List<LinkedList<GameObject>> units;


    [SerializeField] private GameObject peon;
    [SerializeField] private GameObject caballero;
    [SerializeField] private GameObject lancero;
    [SerializeField] private GameObject general;
    [SerializeField] private GameObject torre;

    [SerializeField] private GameObject peonw;
    [SerializeField] private GameObject caballerow;
    [SerializeField] private GameObject lancerow;
    [SerializeField] private GameObject generalw;
    [SerializeField] private GameObject torrew;
    [SerializeField] private int currentMoney;


    private void Start()
    {
        for (int i = 0; i < playersNum; i++)
        {
            playersGold[i] = initialGold;
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) //&& turn == 1)
        {
            ChangeTurn();   
        }      	
    }



    public void ChangeTurn()
    {
        Debug.Log("Change Turn Player: " + turn);
        
        UpdateGold();
        AddUnits();

        if (turn ==  playersNum -1)
        {
            turn = 0;
        }
        else
        {
            turn++;
        }

        for (LinkedListNode<GameObject> node = units[turn].First; node != null; node = node.Next)
        {
            //Debug.Log(node.Value.name);
            if (node.Value != null)

            {


                if (node.Value.GetComponent<Units>())
                {
                    node.Value.GetComponent<Units>().SetMovementsAvailable(node.Value.GetComponent<Units>().GetMaxMovementsAvaliable());

                    if (node.Value.GetComponent<Pawn>())
                    {
                        node.Value.GetComponent<Pawn>().Work();
                    }
                }
                else if (node.Value.GetComponent<Structures>())
                {
                    node.Value.GetComponent<Structures>().SetActionAvaliable();
                }

            }
        }
            

        //Debug.Log("Change Turn Player: " + turn);
    }

    void UpdateGold()
    {
        LinkedListNode<GameObject> node = units[turn].First;
        while(node != null)
        {
            //Debug.Log("Money");
            if (node.Value.GetComponent<Pawn>() != null && node.Value.GetComponent<Pawn>().GetIsMining())
            {
                minesDigged++;
                Debug.Log(minesDigged);
                
            }
            node = node.Next;
        }

        playersGold[turn] += baseGoldWin * minesDigged;
        Debug.Log(playersGold[turn]);
        minesDigged = 0;
        //goldText.text = "" + playersGold[turn];
    }

    void AddUnits()
    {

    }  

    public void ChangeGold(byte goldCost)
    {
        playersGold[turn] -= goldCost;
        //goldText.text = "" + playersGold[turn];
    }

    public int GetPlayersGold(byte player)
    {
        return playersGold[player];
    }

    public byte GetTurn()
    {
        return turn;
    }

    public void SetTurn(byte newTurn)
    {
        turn = newTurn;
    }

    public List<LinkedList<GameObject>> GetUnitList()
    {
        return units;
    }

    
}
