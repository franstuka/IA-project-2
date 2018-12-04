using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Caballeria, Peon, Lancero, General, Torre, Castillo };

public class CombatStats : MonoBehaviour {

    public enum CombatStatsType{ MAXFORCE, FORCE, MAXDAMAGE, DAMAGE};
    public enum UnitType { Caballeria, Peon, Lancero, General, Torre, Castillo };
    [SerializeField] private byte tier = 0;
    [SerializeField] private float Force = 0;
    [SerializeField] private float MaxForce = 0;
    [SerializeField] private float MaxDamage = 0;
    [SerializeField] private float Damage;
    [SerializeField] public UnitType Type;

    private byte team;

    public Animator anim;
    
    [SerializeField] protected AudioSource DoDamageSound;
    [SerializeField] private AudioSource GetDamageSound;
    [SerializeField] private AudioSource DieSound;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        Damage = MaxDamage;
    }

    public UnitType GetUnityType()
    {
        return UnitType.Caballeria;
    }

    public byte GetTeam()
    {
        return team;
    }

    public float GetForce()
    {
        return Force;
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
        return MaxDamage * (100 * Force / MaxForce);
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
                MaxDamage += valor;
                if (MaxDamage < 0)
                    MaxDamage = 0;
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

    public virtual void Die()
    {

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
