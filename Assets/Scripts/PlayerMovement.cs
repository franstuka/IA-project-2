using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CombatStats
{

    [SerializeField] Navegation nav;

    private void Start()
    {
        nav = GetComponent<Navegation>();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~13)) //layer 13 click detection
            {
                nav.SetDestinationPlayerAndCost(hit.point); //update movements and move
            }
        }
	}
}
