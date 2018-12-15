using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Caballeria, Peon, Lancero, General, Torre, Castillo };

public class CombatStats : MonoBehaviour {

    public enum CombatStatsType{ MAXFORCE, FORCE, MAXDAMAGE, DAMAGE, TIER};
    public enum UnitType { Caballeria, Peon, Lancero, General, Torre, Castillo };
    [SerializeField] private byte tier = 0;
    [SerializeField] private float Force = 0;
    [SerializeField] private float MaxForce = 0;
    [SerializeField] private float MaxDamage = 0;
    [SerializeField] private float Damage;
    [SerializeField] private UnitType Type;
    [SerializeField] protected byte team;

    public Animator anim;
    
    [SerializeField] protected AudioSource DoDamageSound;
    [SerializeField] private AudioSource GetDamageSound;
    [SerializeField] private AudioSource DieSound;


    private void Awake()
    {

        anim = GetComponent<Animator>();
        Damage = MaxDamage;
    }


    public byte GetTeam()
    {
        return team;
    }
    public UnitType GetUnityType()
    {
        return Type;
    }

    public float GetForce()
    {
        return Force;
    }

    public void SetAttack(float attack)
    {
        Force -= attack;
        if (Force <= 0)
        {
            Die();
        }
    }

    public float GetMaxForce()
    {
        return MaxForce;
    }

    public float GetMaxDamage()
    {
        return MaxDamage;
    }

    public float GetDamage()
    {
        return MaxDamage * Force / MaxForce;
    }


    /*public float GetDefense()
    {
        return Defense;
    }*/


    public virtual void ChangeStats(CombatStatsType state , float valor)
    {
        switch((int)state)
        {
            case 0:
                MaxForce += valor;
                if (MaxForce < 0)
                {
                    MaxForce = 0;
                }
                if (MaxForce < Force)
                {
                    Force = MaxForce;
                }
                break;
            case 1:
                /*if(valor < 0)
                {
                    if(1-Defense > 0)
                        Force += valor * (1 - Defense);
                }*/
                if(valor >= 0)
                {
                    Force += valor;
                }    
                if (Force <= 0)
                {
                    Force = 0;
                    Die();
                }
                else
                    if(Force > MaxForce)
                        Force = MaxForce;
                break;
            case 2:
                MaxDamage += valor;
                if (MaxDamage < 0)
                    MaxDamage = 0;
                break;
            case 3:
                if (valor >= 0)
                {
                    Force += valor;
                }
                break;


                /*case 3:
                    Defense += valor;
                    if (Defense < 0)
                        Defense = 0;
                    break;*/
        }
    }
    public virtual void SetStats(CombatStatsType state, float valor)
    {
        switch ((int)state)
        {
            case 0:
                MaxForce = valor;
                break;
            case 1:
                if(valor <= MaxForce)
                    Force = valor;
                else
                {
                    Force = MaxForce;
                }
                break;
            case 2:
                MaxDamage = valor;
                if (MaxDamage < 0)
                    MaxDamage = 0;
                break;
            /*case 3:
                Defense = valor;
                if (Defense < 0)
                    Defense = 0;
                break;*/
        }
    }

    public void SetStatTier(byte tier)
    {
        this.tier = tier;
    }

    public virtual void Die()
    {
        Vector2Int aux = GridMap.instance.CellCordFromWorldPoint(transform.position);
        GridMap.instance.grid[aux.x, aux.y].unityOrConstructionOnCell = null;
        Destroy(transform.gameObject);
    }

    public byte GetTier()
    {
        return tier;
    }

    public void SetTier()
    {
        tier++;
    }
}
