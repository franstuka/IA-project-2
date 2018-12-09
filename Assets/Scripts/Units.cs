﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : CombatStats {
    [SerializeField] private byte maxMovementsAvailable;
    [SerializeField] private byte movementsAvailable;
    [SerializeField] private float forceIncrement = 0.9f;
    [SerializeField] private float damageIncrement = 0.9f;



    private void Awake()
    {
        SetMaxMovements();
        movementsAvailable = maxMovementsAvailable;

       
    }
   

    void SetMaxMovements()
    {
        switch (GetUnityType())
        {
            case UnitType.Caballeria:
                maxMovementsAvailable = 5;
                break;
            case UnitType.General:
                maxMovementsAvailable = 3;
                break;
            case UnitType.Lancero:
            case UnitType.Peon:
                maxMovementsAvailable = 3;
                break;
        }
    }

    // Use this for initialization

	
	// Update is called once per frame
	void Update () {

        Debug.Log(GetForce() + " " + name);
		
	}    
    public byte GetMaxMovementsAvaliable()
    {
        return maxMovementsAvailable;
    }

    public byte GetMovementsAvailable()
    {
        return movementsAvailable;
    }

    public void SetMovementsAvailable(byte movements)
    {
        movementsAvailable = movements;
    }

    void Heal()
    {
        float newValue = GetForce() + GetMaxForce() / 4;
        if (newValue > GetMaxForce())
        {
            newValue = GetMaxForce();
        }
        SetStats(CombatStatsType.FORCE, newValue);
    }

    void UpdateUnit(CombatStats firstUnit, CombatStats secondUnit)
    {
        firstUnit.ChangeStats(CombatStatsType.MAXFORCE, ((firstUnit.GetMaxForce()) * forceIncrement));
        firstUnit.ChangeStats(CombatStatsType.FORCE, (firstUnit.GetForce() + secondUnit.GetForce()));
        firstUnit.ChangeStats(CombatStatsType.MAXDAMAGE, (firstUnit.GetMaxDamage() * damageIncrement));
    }

    public void Stack(CombatStats firstUnit, CombatStats secondUnit)
    {
        if (firstUnit.GetTier() < 2 || firstUnit.GetTier() != secondUnit.GetTier())
        {
            List<LinkedList<GameObject>> list = GameManager.instance.GetUnitList();
            byte turn = GameManager.instance.GetTurn();
            UpdateUnit(firstUnit, secondUnit);
            list[turn].Remove(firstUnit.GetComponent<GameObject>());
            firstUnit.SetTier();


            Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(secondUnit.transform.position);
            GridMap.instance.grid[coord.x, coord.y].unityOrConstructionOnCell = null;
            Destroy(secondUnit.transform.gameObject);
        }
        
        

      
    }

}
