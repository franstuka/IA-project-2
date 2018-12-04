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
    }
    #endregion

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private byte playersNum = 2;
    [SerializeField] private int[] playersGold;
    [SerializeField] private byte turn;
    [SerializeField] private int initialGold = 500;
    [SerializeField] private int baseGoldWin = 25;
    private List<LinkedList<GameObject>> units;

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
