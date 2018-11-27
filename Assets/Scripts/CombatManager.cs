﻿using System.Collections;
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
        attacker = attacker.GetComponent<CombatStats>();
        Vector2Int attackerpos = GridMap.instance.CellCordFromWorldPoint(defender.transform.position);
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
                    totaldamageA += attacker.GetDamage();
                    totaldamageA += (attacker.GetDamage() * randomincrement / 100) * Random.Range(0, 1);

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
                    totaldamageA += attacker.GetDamage();
                    totaldamageA += (attacker.GetDamage() * randomincrement / 100) * Random.Range(0, 1);

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

        Vector2 dimensions = GridMap.instance.GetGridSize();
        for (int i = 0; i < generalrange * 2; i++)
        {
            for (int j = 0; j < generalrange * 2; j++)
            {
                if (0 < = (attacker.x - generalrange + i) <= GridSizeX && 0 < = (attacker.y - generalrange + i) <= GridSizeY)
                {
                    if (Chesspieces[attacker.x - generalrange + i, attacker.y + generalrange + j].Type = PieceType.General && Chesspieces[attacker.x - generalrange + i, attacker.y + generalrange + j].team == attacker.team && ((attacker.x - generalrange + i != attacker.x) && (attacker.y + generalrange + j != attacker.y))


                {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void attackUnity(CombatStats defender, float totaldamage)
        {
            defender.SetForce(totaldamage);
        }

}


