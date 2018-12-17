using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyList { GOLD_DOMAIN, GOLD_DEFENSE, CASTLE_DEFENSE, CASTLE_ATTACK, SKIRMISH, MAP_DOMAIN, REINFORCEMENT, STRENGTHEN }
public enum DebugView{ nothing, basic_resources, terrain_cost, defense_pawn, defense_knight, defense_alfil, defense_general,
    attack_pawn, attack_knight, attack_alfil, attack_general, better_position_general, enemy_tower_defense, ally_tower_defense,
    attack_tower, ally_heal_zone, enemy_heal_zone, ally_exploted_resources, enemy_exploted_resources, relative_force }
public struct UnitGroup
{
    List<GameObject> UnitList;
    StrategyList ActiveStrategy;
}


public class IAManager : MonoBehaviour {

    public List<float[,]> estrategyMapsList;
    /* map order:
      0 basic resources
      1 terrain cost
      2 defense pawn
      3 defense knight
      4 defense alfil
      5 defense general
      6 attack pawn
      7 attack knight
      8 attack alfil
      9 attack general
      10 better position general
      11 enemy tower defense
      12 ally tower defense
      13 attack tower
      14 ally heal zone
      15 enemy heal zone
      16 ally exploted resources
      17 enemy exploted resources
      18 relative force
    */
    public List<UnitGroup> unitGroups;
    [SerializeField] private float decayMapDistancePerc = 0.1f;
    [SerializeField] private int decayMapDistanceInCells;
    [SerializeField] private float decayMapDistance;
    [SerializeField] private DebugView debugView = 0;
    public void Start()
    {
        //inicialize maps
        estrategyMapsList = new List<float[,]>(19);
        for (int i = 0; i< 19; i++)
        {
            estrategyMapsList.Add(new float[GridMap.instance.GetGridSizeX(), GridMap.instance.GetGridSizeY()]);
        }
        //set decay
        decayMapDistance = GridMap.instance.GetGridSizeX() > GridMap.instance.GetGridSizeY() ? GridMap.instance.GetGridSizeX() * decayMapDistancePerc * GridMap.instance.GetCellRadius() * 2 
            : GridMap.instance.GetGridSizeY() * decayMapDistancePerc * GridMap.instance.GetCellRadius() * 2;
        decayMapDistanceInCells = Mathf.FloorToInt(decayMapDistance / (GridMap.instance.GetCellRadius() * 2f)) +1;
        //Set static maps
        SetResourcesMap();
    }

    private void SetResourcesMap()
    {
        for (int i = 0; i < GridMap.instance.GetGridSizeX(); i++)
        {
            for (int j = 0; j < GridMap.instance.GetGridSizeY(); j++)
            {
                if(GridMap.instance.grid[i,j].CellType == CellTypes.MINE)
                {
                    for (int x = i-decayMapDistanceInCells >= 0? i - decayMapDistanceInCells : 0; x < GridMap.instance.GetGridSizeX() && x < i + decayMapDistanceInCells; x++)
                    {
                        for (int y = j - decayMapDistanceInCells >= 0 ? j - decayMapDistanceInCells : 0; y < GridMap.instance.GetGridSizeY() && y < j + decayMapDistanceInCells; y++)
                        {
                            estrategyMapsList[0].SetValue((float)estrategyMapsList[0].GetValue(x,y) + (1 / Mathf.Pow(1 + Vector3.Distance(GridMap.instance.grid[i,j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition), 2)),x,y);
                        }
                    }
                }
            }
        }
    }

    public void UpdateMaps()
    {
        
    }

    private void OnDrawGizmos()
    {
        switch ((int)debugView)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    float maxCost = float.MinValue;
                    float minCost = float.MaxValue;
                    foreach (float n in estrategyMapsList[0])
                    {
                        if (maxCost < n)
                        {
                            maxCost = n;
                        }
                        else if (minCost > n)
                        {
                            minCost = n;
                        }
                    }
                    for (int i = 0; i < GridMap.instance.GetGridSizeX(); i++)
                    {
                        for (int j = 0; j < GridMap.instance.GetGridSizeY(); j++)
                        {
                            if ((float)estrategyMapsList[0].GetValue(i,j) == float.MaxValue)
                            {
                                Gizmos.color = Color.black;
                            }
                            else
                            {
                                Gizmos.color = new Color((float)estrategyMapsList[0].GetValue(i, j) / maxCost, (float)estrategyMapsList[0].GetValue(i, j) / maxCost, (float)estrategyMapsList[0].GetValue(i, j) / maxCost, 1); ;
                            }

                            Gizmos.DrawCube(GridMap.instance.grid[i, j].GlobalPosition, Vector3.one * (GridMap.instance.GetCellRadius() * 2 * 19 / 20));
                        }
                    }
                    break;
                }
        }
    }
}