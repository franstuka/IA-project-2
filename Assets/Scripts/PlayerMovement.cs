using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Navegation nav;
    [SerializeField] bool selected = false;

    [SerializeField] GameObject area;

    private LinkedList<Cell> adyacents;
    private LinkedList<GameObject> areas;
    private bool moving = false;
    [SerializeField] private bool construct = false;

    private void Awake()
    {
        adyacents = new LinkedList<Cell>();
        areas = new LinkedList<GameObject>();
        //area.transform.localScale = new Vector3(GridMap.instance.GetCellRadius() * 2, 0, GridMap.instance.GetCellRadius() * 2);
    }

    private void Start()
    {
        nav = GetComponent<Navegation>();
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(moving);

        if (selected && moving != true && construct == false)
        {
            //Debug.Log(this.name);
            //Debug.Log(this == null);
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13)) //layer 13 click detection
                {
                    Cell cellAux = GridMap.instance.CellFromWorldPoint(hit.point);
                    //Debug.Log(adyacents.Contains(cellAux));
                    if (adyacents.Contains(cellAux)) {
                        if (cellAux.unityOrConstructionOnCell && (cellAux.unityOrConstructionOnCell.GetTeam() != GetComponent<CombatStats>().GetTeam())) // cellAux.unityOrConstructionOnCell.GetUnityType() != GetComponent<CombatStats>().GetUnityType()))
                        {
                            //nav.SetDestinationPlayer(cellAux.GlobalPosition);
                            LinkedList<Vector2Int> aux = nav.GetPath(cellAux.GlobalPosition);

                            byte pasos = 0;
                            for (LinkedListNode<Vector2Int> auxNode = aux.First; auxNode != null; auxNode = auxNode.Next)
                            {
                                pasos += GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost();
                            }

                            //GetComponent<Units>().SetMovementsAvailable((byte)(GetComponent<Units>().GetMovementsAvailable() - pasos));

                            //Debug.Log(aux.Count);
                            aux.RemoveLast();
                            Cell cellPrevAux = null;
                            if (aux.Count != 0)
                            {
                                cellPrevAux = GridMap.instance.grid[aux.Last.Value.x, aux.Last.Value.y];
                            }


                            if (cellAux.unityOrConstructionOnCell.GetTeam() != GetComponent<CombatStats>().GetTeam())
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 1));
                            }
                            else
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 0));
                            }

                            cellAux = cellPrevAux;
                            //cellAux = GridMap.instance.grid[aux.Last.Value.x, aux.Last.Value.y];
                            Debug.Log(aux.Count);
                        }
                        else if (cellAux.unityOrConstructionOnCell && cellAux.unityOrConstructionOnCell.GetTeam() == GetComponent<CombatStats>().GetTeam())
                        {
                            //nav.SetDestinationPlayerAndCost(cellAux.GlobalPosition);
                            LinkedList<Vector2Int> aux = nav.GetPath(cellAux.GlobalPosition);
                            byte pasos = 0;
                            for (LinkedListNode<Vector2Int> auxNode = aux.First; auxNode.Value == null; auxNode = auxNode.Next)
                            {
                                Debug.Log(GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost());
                                pasos += GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost();
                            }

                            Debug.Log(aux.Count);
                            aux.RemoveLast();
                            Cell cellPrevAux = null;
                            if (aux.Count != 0)
                            {
                                cellPrevAux = GridMap.instance.grid[aux.Last.Value.x, aux.Last.Value.y];
                            }


                            //Debug.Log((byte)(GetComponent<Units>().GetMovementsAvailable() - pasos));

                            if (cellAux.unityOrConstructionOnCell.GetUnityType() == GetComponent<CombatStats>().GetUnityType())
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 2));
                            }

                            else if (cellAux.unityOrConstructionOnCell.GetUnityType() == CombatStats.UnitType.Torre)
                            {
                                Debug.Log("Entro Torre");
                                StartCoroutine(WaitUnitilStoped(cellAux, 3));
                            }

                            cellAux = cellPrevAux;
                        }

                        else
                        {
                            StartCoroutine(WaitUnitilStoped(cellAux, 0));
                        }

                        Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(transform.position);

                        /*if (cellAux != GridMap.instance.CellFromWorldPoint(transform.position))
                        {
                            GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = null;
                        }*/

                        if (cellAux != null)
                        {
                            if (!GetComponent<Structures>())
                            {
                                nav.SetDestinationPlayer(cellAux.GlobalPosition);
                                GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = null;
                            }

                        }
                    }


                    //nav.SetDestinationPlayerAndCost(hit.point); //update movements and move



                }
            }
        }
	}

    IEnumerator WaitUnitilStoped(Cell cellToGo, byte action)
    {
        moving = true;
        //Debug.Log(nav.GetStopped());
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(()=>nav.GetStopped());

        switch (action)
        {
            case 0:
                break;
            case 1:
                {
                    Debug.Log("Combate");
                    CombatManager.instance.combat(GetComponent<CombatStats>(), cellToGo.unityOrConstructionOnCell);

                    if (GetComponent<Units>())
                    { //Debug.Log(cellToGo.unityOrConstructionOnCell);

                        if (cellToGo.unityOrConstructionOnCell == null)
                        {
                            Debug.Log("HI!! ^^");
                            nav.SetDestinationPlayer(cellToGo.GlobalPosition);

                            Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(cellToGo.GlobalPosition);
                            GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = GetComponent<CombatStats>();
                            moving = false;
                        }
                        else
                        {
                            Vector2Int pos = GridMap.instance.CellCordFromWorldPoint(transform.position);
                            Debug.Log(pos);
                            GridMap.instance.grid[pos.x, pos.y].unityOrConstructionOnCell = GetComponent<CombatStats>();
                            Debug.Log(GridMap.instance.grid[pos.x, pos.y].unityOrConstructionOnCell);
                            moving = false;
                        }
                    }
                    moving = false;
                    UnSelect();
                    break;
                }
            case 2:
                {
                    Debug.Log("Stack");

                    if (this.GetComponent<CombatStats>() != cellToGo.unityOrConstructionOnCell) 
                    {
                        Debug.Log("Stack2");
                        GetComponent<Units>().Stack(GetComponent<CombatStats>(), cellToGo.unityOrConstructionOnCell);
                        nav.SetDestinationPlayer(cellToGo.GlobalPosition);
                        UnSelect();
                    }
                    break;
                }
            case 3:
                {
                    Debug.Log("Entrar en torre");
                    cellToGo.unityOrConstructionOnCell.GetComponent<Structures>().SaveUnit(GetComponent<CombatStats>(),0,0);

                    break;
                }
            default:
                {
                    break;
                }

        }

        if(action != 3 && action != 1)
        {
            Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(cellToGo.GlobalPosition);
            GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = GetComponent<CombatStats>();
            moving = false;

        }

    }

    public bool GetSelected()
    {
        return selected;
    }

    public bool GetConstruct()
    {
        return construct;
    }

    public void Select()
    {
        if (!selected)
        { 
            selected = true;
            ShowAccesibles();
        }
    }

    public void UnSelect()
    {
        //Vector2Int aux = GridMap.instance.CellCordFromWorldPoint(transform.position);
        //Debug.Log(GridMap.instance.grid[aux.x, aux.y].unityOrConstructionOnCell);

        if (GetComponent<Units>())
        {
            GetComponent<Units>().SetMovementsAvailable(0);
        }
        else if (GetComponent<Structures>())
        {
            GetComponent<Structures>().SetActionUnavaliable();
        }
        selected = false;
        construct = false;
        adyacents.Clear();
        while(areas.Count>0)
        {
            Destroy(areas.First.Value);
            areas.RemoveFirst();
        }
        

    }

    public void Construct()
    {
        construct = true;
    }

    public void ShowAccesibles()
    {
        if (this.GetComponent<Units>())
        {
            byte cont = 0;
            byte pasos = GetComponent<Units>().GetMovementsAvailable();
            Cell cellActual = GridMap.instance.CellFromWorldPoint(transform.position);
            //Debug.Log(cellActual.CellType + "   " + GridMap.instance.CellCordFromWorldPoint(cellActual.GlobalPosition));
            float size = GridMap.instance.GetCellRadius() * 2;
            FindAccesibles(cellActual, cont, pasos, size);
            ShowAreaAccesibles();
        }

        else if (GetComponent<Structures>())
        {
            if (GetComponent<Structures>().GetActionAvaliable())
            {
                Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(transform.position);
                for (int i = -2; i < 3; i++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        if (coord.x + i >= 0 && coord.x + i < GridMap.instance.GetGridSizeX()
                            && coord.y + j >= 0 && coord.y + j < GridMap.instance.GetGridSizeY())
                        {
                            Cell cellAux = GridMap.instance.CellFromWorldPoint(GridMap.instance.grid[coord.x + i, coord.y + j].GlobalPosition);
                            adyacents.AddLast(cellAux);

                        }
                    }

                }
            }

            ShowAreaAccesibles();
        }
    }

    private void ShowAreaAccesibles()
    {
        //Debug.Log(adyacents.Count);
        for (LinkedListNode<Cell> cell = adyacents.First; cell != null; cell = cell.Next)
        {
            Vector3 aux = new Vector3(cell.Value.GlobalPosition.x, cell.Value.GlobalPosition.y, cell.Value.GlobalPosition.z);
            GameObject aux2 = Instantiate(area, aux, Quaternion.identity);
            areas.AddLast(aux2);

        }
    }

    private void FindAccesibles(Cell cell,  int cont, byte pasos, float size) 
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //if ((cell.GlobalPosition.x + i * size > 0 && cell.GlobalPosition.x + i * size < GridMap.instance.GetGridSize().x) && (cell.GlobalPosition.y + j * size > 0 && cell.GlobalPosition.y + j * size < GridMap.instance.GetGridSize().y)) {         // te sales?

                    Cell cellAux = GridMap.instance.CellFromWorldPoint(new Vector3(cell.GlobalPosition.x + i * size, cell.GlobalPosition.y, cell.GlobalPosition.z + j * size));

                    
                    if ((cont + cellAux.GetMovementCost()) <=pasos)
                    {
                        if (!adyacents.Contains(cellAux))
                        {
                            //Debug.Log(cellAux.CellType + " "+ GridMap.instance.CellCordFromWorldPoint(cellAux.GlobalPosition) + "   " + cellAux.GetMovementCost());
                            adyacents.AddLast(cellAux);
                        }

                        FindAccesibles(cellAux,  (cont + cellAux.GetMovementCost()), pasos, size);
                    }
               // }
            }
        }

    }
}
