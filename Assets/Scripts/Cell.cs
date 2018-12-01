﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellTypes {Bosque,Llanura,emphy,blocked,chest,exit,enemy};

public class Cell {

    public Vector3 GlobalPosition;
    public int Cost;
    public CellTypes CellType;
    public AStarNode Node;

    public Cell(CellTypes CellType, Vector3 GlobalPosition, int Cost)
    {
        this.CellType = CellType;
        this.GlobalPosition = GlobalPosition;
        this.Cost = Cost;
        Node = new AStarNode(int.MaxValue);
    }
}
