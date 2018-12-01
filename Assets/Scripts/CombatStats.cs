using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Caballeria, Peon, Lancero, General, Torre, Castillo };

public class CombatStats : MonoBehaviour {

    public enum CombatStatsType{ MAXFORCE, FORCE, DAMAGE , DEFENSE };
    public enum UnitType { Caballeria, Peon, Lancero, General, Torre, Castillo };
    [SerializeField] private float Force = 0;
    [SerializeField] private float MaxForce = 0;
    [SerializeField] private int Defense = 0;
    [SerializeField] private byte Tier = 0;
    [SerializeField] public UnitType Type;
    private byte team;

    public Animator anim;
    
    [SerializeField] protected AudioSource DoDamageSound;
    [SerializeField] private AudioSource GetDamageSound;
    [SerializeField] private AudioSource DieSound;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public UnitType GetUnityType()
    {
        return UnitType.Caballeria;
    }

    public byte GetTeam()
    {
        return team;
    }
    public float GetHP()
    {
        return Force;
    }

    public float GetMaxHP()
    {
        return MaxForce;
    }

    public virtual float GetDamage()
    {
        return Force;
    }

    public float GetDefense()
    {
        return Defense;
    }

    public void SetForce(float damage)
    {
        Force -= damage;
        if (Force < 0)
        {
            Force = 0;
            Die();
        }
    }


    public virtual void ChangeStats(CombatStatsType state , int valor)
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
                if(valor < 0)
                {
                    if(1-Defense > 0)
                        Force += valor * (1 - Defense);
                }
                else
                {
                    Force += valor;
                }    
                if (Force < 0)
                {
                    Force = 0;
                    Die();
                }
                else
                    if(Force > MaxForce)
                        Force = MaxForce;
                break;
            case 2:
                Force += valor;
                if (Force < 0)
                    Force = 0;
                break;
            case 3:
                Defense += valor;
                if (Defense < 0)
                    Defense = 0;
                break;
        }
    }
    public virtual void SetStats(CombatStatsType state, int valor)
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
                Force = valor;
                if (Force < 0)
                    Force = 0;
                break;
            case 3:
                Defense = valor;
                if (Defense < 0)
                    Defense = 0;
                break;
        }
    }

    public virtual void Die()
    {

    }

    public byte GetTier()
    {
        return Tier;
    }
}
