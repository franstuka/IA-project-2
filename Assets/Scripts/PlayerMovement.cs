﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Navegation nav;
    [SerializeField] bool selected = false;

    private LinkedList<Cell> adyacents;

    private void Awake()
    {
        adyacents = new LinkedList<Cell>();
    }

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
                    //Debug.Log(adyacents.Contains(cellAux));
                    if (adyacents.Contains(cellAux)) {
                        if (cellAux.unityOrConstructionOnCell && (cellAux.unityOrConstructionOnCell.GetTeam() != GetComponent<CombatStats>().GetTeam() || cellAux.unityOrConstructionOnCell.GetUnityType() != GetComponent<CombatStats>().GetUnityType()))
                        {
                            nav.SetDestinationPlayerAndCost(cellAux.GlobalPosition);
                            LinkedList<Vector2Int> aux = nav.GetSavedPath();
                            byte pasos = 0;
                            for (LinkedListNode<Vector2Int> auxNode = aux.First; auxNode != null; auxNode = auxNode.Next)
                            {
                                pasos += GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost(); 
                            }

                            GetComponent<Units>().SetMovementsAvailable((byte)(GetComponent<Units>().GetMovementsAvailable() - pasos));

                            Debug.Log(aux.Count);
                            aux.RemoveLast();
                            Debug.Log(aux.Count);

                            if (cellAux.unityOrConstructionOnCell.GetTeam() != GetComponent<CombatStats>().GetTeam())
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 1));
                            }
                        }
                        else if (cellAux.unityOrConstructionOnCell && cellAux.unityOrConstructionOnCell.GetTeam() == GetComponent<CombatStats>().GetTeam())
                        {
                            nav.SetDestinationPlayerAndCost(cellAux.GlobalPosition);
                            LinkedList<Vector2Int> aux = nav.GetSavedPath();
                            byte pasos = 0;
                            for (LinkedListNode<Vector2Int> auxNode = aux.First; auxNode.Value == null; auxNode = auxNode.Next)
                            {
                                Debug.Log(GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost());
                                pasos += GridMap.instance.grid[auxNode.Value.x, auxNode.Value.y].GetMovementCost();
                            }

                            //Debug.Log((byte)(GetComponent<Units>().GetMovementsAvailable() - pasos));
                            GetComponent<Units>().SetMovementsAvailable((byte)(GetComponent<Units>().GetMovementsAvailable() - pasos));

                            if (cellAux.unityOrConstructionOnCell.GetUnityType() == GetComponent<CombatStats>().GetUnityType())
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 2));
                            }
                            else if (cellAux.unityOrConstructionOnCell.GetUnityType() == CombatStats.UnitType.Torre)
                            {
                                StartCoroutine(WaitUnitilStoped(cellAux, 3));
                            }
                        }

                        else
                        {
                            StartCoroutine(WaitUnitilStoped(cellAux, 0));
                        }

                        Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(transform.position);
                        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = null;

                        nav.SetDestinationPlayer(cellAux.GlobalPosition);
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
                    Debug.Log("Combate");
                    CombatManager.instance.combat(GetComponent<CombatStats>(), cellToGo.unityOrConstructionOnCell);

                    if (!cellToGo.unityOrConstructionOnCell)
                    {
                        nav.SetDestinationPlayerAndCost(cellToGo.GlobalPosition);
                    }
                    break;
                }
            case 2:
                {
                    Debug.Log("Stack");
                    GetComponent<Units>().Stack(GetComponent<CombatStats>(), cellToGo.unityOrConstructionOnCell);
                    break;
                }
            case 3:
                {

                    break;
                }

        }

        Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(cellToGo.GlobalPosition);
        GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = GetComponent<CombatStats>();

    }

    public bool GetSelected()
    {
        return selected;
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
        byte pasos = GetComponent<Units>().GetMovementsAvailable();
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
                if (i >= 0 && j >= 0 ) {         // te sales?

                    Cell cellAux = GridMap.instance.CellFromWorldPoint(new Vector3(cell.GlobalPosition.x + i * size, cell.GlobalPosition.y, cell.GlobalPosition.z + j * size));

                    //Debug.Log(GridMap.instance.CellCordFromWorldPoint(cellAux.GlobalPosition));
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
