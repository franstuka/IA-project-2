using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStats : MonoBehaviour {

    public enum CombatStatsType{ MAXHP , HP , DAMAGE , DEFENSE };
    [SerializeField] private int MaxHp = 0;
    [SerializeField] private int HP = 0;
    [SerializeField] private float Force = 0;
    [SerializeField] private float MaxForce = 0;
    [SerializeField] private int Defense = 0;

    public enum UnitType { Caballeria, Peon , Lancero, General, Torre, Castillo };
    [SerializeField] public UnitType Type;
    private byte team;

    public Animator anim;
    
    [SerializeField] protected AudioSource DoDamageSound;
    [SerializeField] private AudioSource GetDamageSound;
    [SerializeField] private AudioSource DieSound;

    private void OnEnable()
    {
        HP = MaxHp;
    }

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
    public int GetHP()
    {
        return HP;
    }

    public int GetMaxHP()
    {
        return MaxHp;
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
                MaxHp += valor;
                if (MaxHp < 0)
                {
                    MaxHp = 0;
                }
                if (MaxHp < HP)
                {
                    HP = MaxHp;
                }
                break;
            case 1:
                if(valor < 0)
                {
                    if(1-Defense > 0)
                        HP += valor * (1 - Defense);
                }
                else
                {
                    HP += valor;
                }    
                if (HP < 0)
                {
                    HP = 0;
                    Die();
                }
                else
                    if(HP>MaxHp)
                        HP = MaxHp;
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
                MaxHp = valor;
                break;
            case 1:
                if(valor <= MaxHp)
                    HP = valor;
                else
                {
                    HP = MaxHp;
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
}
