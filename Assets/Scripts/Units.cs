using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : CombatStats {
    private byte maxMovementsAvailable;
    private byte movementsAvailable;
    private Navegation nav;
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
                maxMovementsAvailable = 4;
                break;
            case UnitType.General:
                maxMovementsAvailable = 3;
                break;
            case UnitType.Lancero:
            case UnitType.Peon:
                maxMovementsAvailable = 2;
                break;
        }
    }

    // Use this for initialization
    void Start () {
        nav = GetComponent<Navegation>();
    }
	
	// Update is called once per frame
	void Update () {
		
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
        firstUnit.ChangeStats(CombatStatsType.MAXFORCE, ((firstUnit.GetMaxForce() * 2) * forceIncrement));
        firstUnit.ChangeStats(CombatStatsType.FORCE, (firstUnit.GetForce() + secondUnit.GetForce()));
        firstUnit.ChangeStats(CombatStatsType.MAXDAMAGE, (firstUnit.GetMaxDamage() * 2 * damageIncrement));
    }

    public void Stack(CombatStats firstUnit, CombatStats secondUnit)
    {
        if (firstUnit.GetTier() < 2 || firstUnit.GetTier() != secondUnit.GetTier())
        {
            List<LinkedList<GameObject>> list = GameManager.instance.GetUnitList();
            byte turn = GameManager.instance.GetTurn();
            UpdateUnit(firstUnit, secondUnit);
            list[turn].Remove(list[turn].Find(secondUnit.GetComponent<GameObject>()));
            firstUnit.SetTier();
        }
        
        

      
    }

}
