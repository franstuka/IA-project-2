using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CombatStats
{

    [SerializeField] Navegation nav;
    [SerializeField] bool selected = false;

    private LinkedList<Cell> adyacents;

    private void Start()
    {
        nav = GetComponent<Navegation>();
    }

    // Update is called once per frame
    void Update () {

        if (selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13)) //layer 13 click detection
                {
                    Cell cellAux = GridMap.instance.CellFromWorldPoint(hit.point);

                    if (adyacents.Contains(cellAux)) {
                        if (cellAux.unityOrConstructionOnCell && (cellAux.unityOrConstructionOnCell.GetTeam() != GetTeam() || cellAux.unityOrConstructionOnCell.GetUnityType() != GetUnityType()))
                        {
                            nav.SetDestinationPlayerAndCost(cellAux.GlobalPosition);
                            LinkedList<Vector2Int> aux = nav.GetSavedPath();
                            byte pasos = 0;
                            for (LinkedListNode<Vector2Int> auxNode = aux.First; auxNode.Value != null; auxNode = auxNode.Next)
                            {
                                pasos += GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost();
                            }

                            GetComponent<Units>().SetMovementsAvailable( (byte)(GetComponent<Units>().GetMovementsAvailable() - pasos) );

                            aux.RemoveLast();
                            if (cellAux.unityOrConstructionOnCell.GetTeam() != GetTeam())
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 1));
                            }
                        }
                        else if (cellAux.unityOrConstructionOnCell && cellAux.unityOrConstructionOnCell.GetTeam() == GetTeam())
                        {
                            nav.SetDestinationPlayerAndCost(cellAux.GlobalPosition);
                            LinkedList<Vector2Int> aux = nav.GetSavedPath();
                            byte pasos = 0;
                            for (LinkedListNode<Vector2Int> auxNode = aux.First; auxNode.Value != null; auxNode = auxNode.Next)
                            {
                                pasos += GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost();
                            }

                            GetComponent<Units>().SetMovementsAvailable((byte)(GetComponent<Units>().GetMovementsAvailable() - pasos));

                            if (cellAux.unityOrConstructionOnCell.GetUnityType() == this.GetUnityType())
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 2));
                            }
                            else if (cellAux.unityOrConstructionOnCell.GetUnityType() == UnitType.Torre)
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 3));
                            }
                        }
                        nav.SetDestinationPlayerAndCost(cellAux.GlobalPosition);
                    }
                    //nav.SetDestinationPlayerAndCost(hit.point); //update movements and move
                }
            }
        }
	}

    IEnumerator WaitUnitilStoped(Cell cellToGo, byte action)
    {
        yield return new WaitUntil(()=>nav.GetStopped());

        switch (action)
        {
            case 1:
                {
                    CombatManager.instance.combat(this, cellToGo.unityOrConstructionOnCell);

                    if (!cellToGo.unityOrConstructionOnCell)
                    {
                        nav.SetDestinationPlayerAndCost(cellToGo.GlobalPosition);
                    }
                    break;
                }
            case 2:
                {
                    GetComponent<Units>().Stack(cellToGo.unityOrConstructionOnCell, this);
                    break;
                }
            case 3:
                {

                    break;
                }

        }

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
        selected = false;
        adyacents.Clear();
    }

    public void ShowAccesibles()
    {

        byte cont = 0;
        byte pasos = GetComponent<Units>().GetMovementsAvailable();  //pasos que puede realizar
        Cell cellActual = GridMap.instance.CellFromWorldPoint(transform.position);
        float size = GridMap.instance.GetCellRadius() * 2;
        findAccesibles(cellActual, cellActual, cont, pasos, size);

    }

    private void findAccesibles(Cell cell, Cell cellActual, int cont, byte pasos, float size) 
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i != 0 && j != 0) { 
                    Cell cellAux = GridMap.instance.CellFromWorldPoint(new Vector3(cell.GlobalPosition.x + i * size, cell.GlobalPosition.y, cell.GlobalPosition.z + j * size));

                    if (cellAux != cellActual && (cont + cellAux.GetMovementCost()) < pasos)
                    {
                        if (!adyacents.Contains(cellAux))
                        {
                            adyacents.AddLast(cellAux);
                        }

                        findAccesibles(cellAux, cellActual, (cont + cellAux.GetMovementCost()), pasos, size);
                    }
                }
            }
        }

    }
}
