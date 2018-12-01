using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    private const float randomincrement = 20;
    private const float advantageincrement = 20;
    private const float cellincrement = 20;
    private const float generalincrement = 50;
    private const byte generalrange = 3;

    void combat(CombatStats attacker, CombatStats defender)
    {
        float totaldamageA = 0;
        float totaldamageD = 0;

        Vector2Int attackerpos = GridMap.instance.CellCordFromWorldPoint(attacker.transform.position);
        Vector2Int defenderpos = GridMap.instance.CellCordFromWorldPoint(defender.transform.position);

        totaldamageA += attacker.GetDamage();
        totaldamageA += (attacker.GetDamage() * randomincrement / 100) * Random.Range(0, 1);

        totaldamageD += defender.GetDamage();
        totaldamageD += (defender.GetDamage() * randomincrement / 100) * Random.Range(0, 1);

        switch (attacker.GetUnityType())
        {
                case CombatStats.UnitType.Lancero:
                {
                    switch (defender.GetUnityType())
                    {
                            case CombatStats.UnitType.Caballeria:
                            { 
                                totaldamageA += attacker.GetDamage() * advantageincrement / 100;                           
                                break;
                            };

                            case CombatStats.UnitType.Peon:
                            {
                                totaldamageD += defender.GetDamage() * (advantageincrement / 2) / 100;
                                break;
                            };

                            case CombatStats.UnitType.General:
                            {
                                totaldamageA += attacker.GetDamage() * advantageincrement / 100;
                                break;
                            };
                            default:
                            {
                                break;
                            }
                    }

                    switch (GridMap.instance.grid[defenderpos[0], defenderpos[1]].CellType)
                    {
                            case CellTypes.Llanura:
                            {
                                totaldamageA -= attacker.GetDamage() * cellincrement / 100;
                                break;
                            }

                            case CellTypes.Bosque:
                            {
                                totaldamageA += attacker.GetDamage() * cellincrement / 100;
                                break;
                            }

                            default:
                            {
                                break;
                            }
                    }

                    break;
                }

                case CombatStats.UnitType.Caballeria:
                {
                    switch (defender.GetUnityType())
                    {


                            case CombatStats.UnitType.Lancero:
                            {
                                totaldamageD += defender.GetDamage() * (advantageincrement) / 100;
                                break;

                            };

                            case CombatStats.UnitType.Peon:
                            {
                                totaldamageA += attacker.GetDamage() * advantageincrement / 100;
                                break;

                            };

                            case CombatStats.UnitType.General:
                            {
                                totaldamageA += (attacker.GetDamage() * advantageincrement / 100);
                                break;

                            };


                        default:
                            {
                                break;
                            };
                    }

                    switch (GridMap.instance.grid[defenderpos[0], defenderpos[1]].CellType)
                    {
                            case CellTypes.Llanura:
                            {
                                totaldamageA += attacker.GetDamage() * cellincrement / 100;
                                break;
                            };

                            case CellTypes.Bosque:
                            {
                                totaldamageA -= attacker.GetDamage() * cellincrement / 100;
                                break;
                            };

                            default:
                            {
                                break;
                            };
                    }

                    break;
                }

                case CombatStats.UnitType.Peon:
                {
                    switch (defender.GetUnityType())
                    {
                            case CombatStats.UnitType.Caballeria:
                            {

                                totaldamageD += defender.GetDamage() * (advantageincrement) / 100;
                                break;

                            };

                            case CombatStats.UnitType.Lancero:
                            {
                                totaldamageA += attacker.GetDamage() * (advantageincrement / 2) / 100;
                                break;

                            };

                            case CombatStats.UnitType.General:
                            {
                                totaldamageA += attacker.GetDamage() * (advantageincrement / 2) / 100;
                                break;

                            };

                            case CombatStats.UnitType.Torre:
                            {
                                totaldamageA += attacker.GetDamage() * advantageincrement / 100;
                                break;

                            };

                            case CombatStats.UnitType.Castillo:
                            {
                                totaldamageA += attacker.GetDamage() * advantageincrement / 100;
                                break;

                            };


                        default:
                            {
                                break;
                            }
                    }

                    break;
                }

                case CombatStats.UnitType.General:
                {
                    if (defender.GetUnityType() == CombatStats.UnitType.Peon)
                    {
                        totaldamageD += (defender.GetDamage() * (advantageincrement / 2) / 100);
                    }

                    else
                    {
                        totaldamageD += (defender.GetDamage() * (advantageincrement) / 100);
                    }

                    break;
                }

                case CombatStats.UnitType.Torre:
                {
                    if (Mathf.Abs(defenderpos[0] - attackerpos[0]) <= 1 &&  Mathf.Abs(defenderpos[1] - attackerpos[1]) <= 1)
                    {
                        totaldamageD += defender.GetDamage();
                        totaldamageD += (defender.GetDamage() * randomincrement / 100) * Random.Range(0, 1);

                        if (attacker.GetUnityType() == CombatStats.UnitType.Peon)
                        {
                            totaldamageD += defender.GetDamage() * advantageincrement / 100;
                        }
                    }

                    else
                    {
                        totaldamageD = 0;
                    }


                     break;
                }

                case CombatStats.UnitType.Castillo:
                {


                    if (Mathf.Abs(defenderpos[0] - attackerpos[0]) <= 1 && Mathf.Abs(defenderpos[1] - attackerpos[1]) <= 1)
                    {
                        totaldamageD += defender.GetDamage();
                        totaldamageD += (defender.GetDamage() * randomincrement / 100) * Random.Range(0, 1);

                        if (attacker.GetUnityType() == CombatStats.UnitType.Peon)
                        {
                            totaldamageD += defender.GetDamage() * advantageincrement / 100;
                        }
                    }

                    else
                    {
                        totaldamageD = 0;
                    }
                    break;
                }


        }

        if (GeneralInRange(attacker))
        {
            totaldamageA += Mathf.FloorToInt((attacker.GetDamage() * generalincrement / 100));
        }

        attackUnity(defender, totaldamageA);

        if (GeneralInRange(defender))
        {
            totaldamageD += Mathf.FloorToInt((attacker.GetDamage() * generalincrement / 100));
        }

        attackUnity(attacker, totaldamageD);
    }

    private bool GeneralInRange(CombatStats attacker)
    {
        Vector2 attackerpos = GridMap.instance.CellCordFromWorldPoint(attacker.transform.position);
        byte team = GameManager.instance.GetTurn();
 
        List<LinkedList<GameObject>> units = new List<LinkedList<GameObject>>();
        units = GameManager.instance.GetUnits();
        LinkedListNode<GameObject> unit; 

        for (unit = units[team].First; unit != null; unit = unit.Next)
        {

            if (unit.Value.GetComponent<CombatStats>().GetUnityType() == CombatStats.UnitType.General)
            {
                Vector2 generalpos = GridMap.instance.CellCordFromWorldPoint(unit.Value.transform.position);

                if (Mathf.Abs(generalpos[0] - attackerpos[0] ) <= generalrange && Mathf.Abs(generalpos[1] - attackerpos[1]) <= generalrange)
                {
                    return true;
                }

            }

            unit = unit.Next;

        }

        return false;


    }
    private void attackUnity(CombatStats defender, float totaldamage)
        {
            defender.SetForce(totaldamage);
        }

}


