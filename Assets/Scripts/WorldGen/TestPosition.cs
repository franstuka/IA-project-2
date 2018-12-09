using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPosition : MonoBehaviour {

    private void Update()
    {
        if(Input.GetKeyDown("2"))
            Debug.Log(GridMap.instance.CellCordFromWorldPoint(transform.position) + " " +
                GridMap.instance.grid[GridMap.instance.CellCordFromWorldPoint(transform.position).x, GridMap.instance.CellCordFromWorldPoint(transform.position).y].CellType);
    }
}
