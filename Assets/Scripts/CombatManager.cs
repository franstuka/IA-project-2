using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    #region singleton
    public static CombatManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of grid is trying to active");
            return;
        }
        instance = this;
        
    }
    #endregion

    private const float randomincrement = 20;
    private const float advantageincrement = 20;
    private const float cellincrement = 20;
    private const float generalincrement = 50;
    private const byte generalrange = 3;

    public void combat(CombatStats attacker, CombatStats defender)
    {
        float totaldamageA = 0;
        float totaldamageD = 0;

        Vector2Int attackerpos = GridMap.instance.CellCordFromWorldPoint(attacker.transform.position);
        Vector2Int defenderpos = GridMap.instance.CellCordFromWorldPoint(defender.transform.position);

        totaldamageA += attacker.GetDamage();
        totaldamageA += (attacker.GetDamage() * (Random.Range(1, 21)) / 100) ;

        totaldamageD += defender.GetDamage();
        totaldamageD += (attacker.GetDamage() * (Random.Range(1, 21)) / 100);


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
                            case CellTypes.PLAIN:
                            {
                                totaldamageA -= attacker.GetDamage() * cellincrement / 100;
                                break;
                            }

                            case CellTypes.FOREST:
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
                            case CellTypes.PLAIN:
                            {
                                totaldamageA += attacker.GetDamage() * cellincrement / 100;
                                break;
                            };

                            case CellTypes.FOREST:
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
                                Debug.Log("Hola");
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

        Debug.Log(GeneralInRange(attacker));
        if (GeneralInRange(attacker))
        {
            totaldamageA += Mathf.FloorToInt((attacker.GetDamage() * generalincrement / 100));
        }

        if (GeneralInRange(defender))
        {
            totaldamageD += Mathf.FloorToInt((attacker.GetDamage() * generalincrement / 100));
        }
        
        attackUnity(defender, totaldamageA);
        attackUnity(attacker, totaldamageD);

    }


    private bool GeneralInRange(CombatStats attacker)
    {
        Vector2Int coord = GridMap.instance.CellCordFromWorldPoint(attacker.transform.position);

        for (int i = -generalrange ; i <= generalrange; i++)
        {
            for (int j = -generalrange; j <= generalrange ; j++)
            {

                if ((0 <= (coord.x + i) && (coord.x + i) < GridMap.instance.GetGridSizeX())
                    && 0 <= (coord.y + j) && (coord.y + j) < GridMap.instance.GetGridSizeY()
                    && (i != 0) && (j != 0)) 
                {
                    Debug.Log(GridMap.instance.grid[coord.x + i, coord.y + j].unityOrConstructionOnCell == null);
                    if (GridMap.instance.grid[coord.x + i, coord.y + j].unityOrConstructionOnCell && GridMap.instance.grid[coord.x + i, coord.y + j].unityOrConstructionOnCell.GetComponent<CombatStats>().GetUnityType() == CombatStats.UnitType.General && GridMap.instance.grid[coord.x + i, coord.y + j].unityOrConstructionOnCell.GetComponent<CombatStats>().GetTeam() == attacker.GetTeam()) 
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
            defender.SetAttack(totaldamage);        
    }

}


