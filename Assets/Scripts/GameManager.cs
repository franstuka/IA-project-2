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

        Vector3 posicion = GridMap.instance.grid[1, 0].GlobalPosition;
        GameObject Peon = Instantiate(peon, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);

        units[1].AddLast(Peon);

        Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(Peon.transform.position);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = Peon.GetComponent<CombatStats>();

        Vector3 posicion2 = GridMap.instance.grid[1, 1].GlobalPosition;
        GameObject Peon2 = Instantiate(peon, new Vector3(posicion2.x, 1, posicion2.z), Quaternion.identity);

        units[1].AddLast(Peon2);

        Vector2Int coord2 = GridMap.instance.CellCordFromWorldPoint(Peon2.transform.position);
        GridMap.instance.grid[coord2.x, coord2.y].unityOrConstructionOnCell = Peon2.GetComponent<CombatStats>();

        Vector3 posicion3 = GridMap.instance.grid[0, 0].GlobalPosition;
        GameObject Peon3 = Instantiate(peonw, new Vector3(posicion3.x, 1, posicion3.z), Quaternion.identity);

        units[0].AddLast(Peon3);

        Vector2Int coord3 = GridMap.instance.CellCordFromWorldPoint(Peon3.transform.position);
        GridMap.instance.grid[coord3.x, coord3.y].unityOrConstructionOnCell = Peon3.GetComponent<CombatStats>();

    }
    #endregion

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private byte playersNum = 2;
    [SerializeField] private int[] playersGold;
    [SerializeField] private byte turn = 1;
    [SerializeField] private int initialGold = 500;
    [SerializeField] private int baseGoldWin = 25;
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


    private void Start()
    {
        for (int i = 0; i < playersNum; i++)
        {
            playersGold[i] = initialGold;
        }

    }

    // Update is called once per frame
    void Update () {        
        //ChangeTurn();        	
	}



    public void ChangeTurn()
    {

        UpdateGold();
        AddUnits();

        if (turn++ > playersNum -1)
        {
            turn = 0;
        }
        else
        {
            turn++;
        }

    }

    void UpdateGold()
    {
        LinkedListNode<GameObject> node = units[turn].First;
        while(node != null)
        {
            if (node.Value.GetComponent<Pawn>() != null && node.Value.GetComponent<Pawn>().GetIsMining())
                playersGold[turn] += baseGoldWin * node.Value.GetComponent<Pawn>().GetTier();
            node = node.Next;
        }
        goldText.text = "" + playersGold[turn];
    }

    void AddUnits()
    {

    }  

    public void ChangeGold(byte goldCost)
    {
        playersGold[turn] -= goldCost;
        goldText.text = "" + playersGold[turn];
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
