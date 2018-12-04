using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour {

    [SerializeField] private byte playerTurn;
    //private byte lastTurn;
    private CombatStats selected;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update () {

        playerTurn = gameManager.GetTurn();

        if ( selected && playerTurn != selected.GetTeam())
        {
            selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
            selected = null;
        }
        if (!selected)
        {
            SelectUnit();

        } else {

            if (Unselect())
            {
                selected.GetComponent<PlayerMovement>().UnSelect(); //No recuerdo si se podia hacer así
                selected = null;

            } else {

                if (selected.GetUnityType() != CombatStats.UnitType.Torre && selected.GetUnityType() != CombatStats.UnitType.Castillo)      // es del tipo unidad
                {
                    if(selected.GetComponent<Units>().MovimientoDisponible())
                    {
                        //selected.GetComponent<PlayerMovement>().Select();   //No recuerdo si se podia hacer así
                    } else
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
        if (Input.GetMouseButtonDown(0))
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
        }
    }


    private bool Unselect() {
        if (Input.GetMouseButtonDown(1))
        {
            return true;
        }
        return false;
    }

    private void DoThings() { }
}
