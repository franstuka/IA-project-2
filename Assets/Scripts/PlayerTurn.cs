using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour {

    private byte playerTurn;
    //private byte lastTurn;
    [SerializeField] private CombatStats selected;

    /*private void Start()
    {
        playerTurn = GameManager.instance.GetTurn();
    }*/
    void LateUpdate()
    {
        playerTurn = GameManager.instance.GetTurn();

        //Debug.Log(selected == null);
        //Debug.Log(playerTurn);
        //if (selected)
        //{
        //    Debug.Log(selected.GetTeam());
        //}

        if (selected && playerTurn != selected.GetTeam())
        {
            Debug.Log("HI!! ^^");
            selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
            selected = null;
        }

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
                        /*if ((cell.unityOrConstructionOnCell.GetComponent<Pawn>() && (cell.unityOrConstructionOnCell.GetComponent<Pawn>().GetWorking()>0)))
                        {

                        }*/

                        if (cell.unityOrConstructionOnCell.GetComponent<Pawn>())
                        {
                            if (cell.unityOrConstructionOnCell.GetComponent<Pawn>().GetWorking() == 0)
                            {
                                selected = cell.unityOrConstructionOnCell;
                                selected.GetComponent<PlayerMovement>().Select();
                            }
                        }
                        else
                        {
                            selected = cell.unityOrConstructionOnCell;
                            selected.GetComponent<PlayerMovement>().Select();
                        }
                        /*selected = cell.unityOrConstructionOnCell;
                        selected.GetComponent<PlayerMovement>().Select();*/
                        //Click(selected);
                    }
                }
            }

        }

        else if (selected)
        {
            if (Input.GetKeyDown("t") && selected.GetUnityType() == CombatStats.UnitType.Peon)
            {
                selected.GetComponent<PlayerMovement>().Construct();
                //createUnit(UnitType.Peon, );
            }

            if (!selected.GetComponent<PlayerMovement>().GetSelected())
            {
                selected = null;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Debug.Log(selected);
                selected.GetComponent<PlayerMovement>().UnSelect();
                selected = null;
                //Debug.Log(selected);

            }

        }

        if (selected  && selected.GetComponent<PlayerMovement>().GetConstruct() == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13)) //layer 13 click detection
                {
                    Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(selected.transform.position);
                    Vector2Int rayhit = GridMap.instance.CellCordFromWorldPoint(hit.point);


                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (rayhit.x == coord.x + i && rayhit.y == coord.y + j
                                && coord.x + i >= 0 && coord.x + i < GridMap.instance.GetGridSizeX()
                                && coord.y + j >= 0 && coord.y + j < GridMap.instance.GetGridSizeY()
                                && GridMap.instance.grid[coord.x + i, coord.y + j].unityOrConstructionOnCell == null)
                            {
                                Vector3 pos = GridMap.instance.grid[coord.x + i, coord.y + j].GlobalPosition;
                                selected.GetComponent<Pawn>().StartConstruction(pos);
                                //Vector3 pos = GridMap.instance.grid[coord.x + 1, coord.y].GlobalPosition;
                                //selected.GetComponent<Pawn>().CreateTower(pos);
                            }
                        }
                    }
                }
            }
        }         
    }

    /*private void Click(CombatStats selected)
    {

            if (selected && playerTurn != selected.GetTeam())
            {
                selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
                selected = null;
            }
            if (!selected)
            {
                //SelectUnit();

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

    }*/

    public void UnselectSelectUnit()
    {
        selected = null;
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
        else if (selected.GetUnityType() == CombatStats.UnitType.Peon)
        {

        }

    }

}
