using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellTypes { PLAIN, FOREST, HILLS, MOUNTAINS ,MINE , CASTLE};

public class Cell {

    public Vector3 GlobalPosition;
    public CellTypes CellType;
    public AStarNode Node;
    public CombatStats unityOrConstructionOnCell;


    public Cell(CellTypes CellType, Vector3 GlobalPosition)
    {
        this.CellType = CellType;
        this.GlobalPosition = GlobalPosition;
        Node = new AStarNode(int.MaxValue);
    }

    public byte GetMovementCost()//unitType
    {
        switch(CellType)
        {
            case CellTypes.CASTLE:
            case CellTypes.HILLS:
                {
                    return 2;
                }
            case CellTypes.FOREST:
                {
                    //if(Unidad.Type == Horse) //coste doble
                    return 1;
                }
            case CellTypes.PLAIN:
                {
                    return 1;
                }
            case CellTypes.MOUNTAINS:
                {
                    return byte.MaxValue;
                }
            case CellTypes.MINE:
                {
                    return 1;
                }
            default:
                {
                    Debug.LogError("UnknowCell");
                    return 1;
                }
        }

    }
}
