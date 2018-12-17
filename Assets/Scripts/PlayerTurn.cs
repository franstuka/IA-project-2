using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour {

    private byte playerTurn;
    //private byte lastTurn;
    private CombatStats selected;

    private void Start()
    {
        playerTurn = GameManager.instance.GetTurn();
    }
    void LateUpdate()
    {
        if (!selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13))
                {
                    Cell cell = GridMap.instance.CellFromWorldPoint(hit.point);
                    //Debug.Log(GridMap.instance.CellCordFromWorldPoint(hit.point));

                    //Debug.Log(cell.unityOrConstructionOnCell);
                    if (cell.unityOrConstructionOnCell && cell.unityOrConstructionOnCell.GetTeam() == playerTurn)
                    {
                        selected = cell.unityOrConstructionOnCell;
                        selected.GetComponent<PlayerMovement>().Select();
                        //Click(selected);
                    }
                }
            }

        }

        if (selected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //Debug.Log(selected);
                selected.GetComponent<PlayerMovement>().UnSelect();
                selected = null;
                //Debug.Log(selected);

            }

        }
    }

    private void Click(CombatStats selected)
    {

            if (selected && playerTurn != selected.GetTeam())
            {
                selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
                selected = null;
            }
            if (!selected)
            {
                SelectUnit();

            }
            else
            {

                if (Unselect())
                {
                    selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
                    selected = null;

                }
                else
                {

                if (selected.GetUnityType() != CombatStats.UnitType.Torre && selected.GetUnityType() != CombatStats.UnitType.Castillo)      // es del tipo unidad
                {
                    if (selected.GetComponent<Units>().GetMovementsAvailable() > 0)
                    {
                        selected.GetComponent<PlayerMovement>().Select();
                    }
                    else
                    {
                        selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
                    }
                }

                DoThings();

               
            }

        }

    }

    private void SelectUnit()
    {
        

    }


    private bool Unselect() {
        if (Input.GetMouseButtonDown(1))
        {
            return true;
        }
        return false;
    }

    private void DoThings() {

        if (selected.GetUnityType() == CombatStats.UnitType.Torre)
        {
            if (Input.GetKey("a"))
            {
                selected.GetComponent<Structures>().CreateUnit(CombatStats.UnitType.Peon, selected);
            }
            else if (Input.GetKey("s"))
            {
                selected.GetComponent<Structures>().CreateUnit(CombatStats.UnitType.Lancero, selected);
            }

            else if (Input.GetKey("d"))
            {
                selected.GetComponent<Structures>().CreateUnit(CombatStats.UnitType.Caballeria, selected);
            }

            else if (Input.GetKey("f"))
            {
                selected.GetComponent<Structures>().CreateUnit(CombatStats.UnitType.General, selected);
            }

            /*else if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13))
                {
                    Cell cell = GridMap.instance.CellFromWorldPoint(hit.point);
                    if (cell.unityOrConstructionOnCell && cell.unityOrConstructionOnCell.GetTeam() == playerTurn)
                    {
                        selected = cell.unityOrConstructionOnCell;
                        //selected.GetComponent<PlayerMovement>().Select();   //No recuerdo si se podia hacer así
                    }
                }
            }*/

        }

    }
}
