using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyList { GOLD_DOMAIN, GOLD_DEFENSE, CASTLE_DEFENSE, CASTLE_ATTACK, SKIRMISH, MAP_DOMAIN, REINFORCEMENT, STRENGTHEN }
public enum DebugView{ nothing, basic_resources, terrain_cost, defense_pawn, defense_knight, defense_bishop, defense_general,
    attack_pawn, attack_knight, attack_bishop, attack_general, better_position_general, enemy_tower_defense, ally_tower_defense,
    attack_tower, ally_heal_zone, enemy_heal_zone, ally_exploted_resources, enemy_exploted_resources, relative_force }
public struct UnitGroup
{
    List<GameObject> UnitList;
    StrategyList ActiveStrategy;
}


public class IAManager : MonoBehaviour {

    #region singleton

    public static IAManager instance;

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

    public List<float[,]> estrategyMapsList;
    /* map order:
      0 basic resources
      1 terrain cost
      2 defense pawn
      3 defense knight
      4 defense bishop
      5 defense general
      6 attack pawn
      7 attack knight
      8 attack bishop
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
    public byte IAPlayer = 0;
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
        SetBetterPositionGeneralMap();
        SetRelativeForceMap();
    }

    public void SetResourcesMap() //called from ScenarioGenerator after scenario has been created
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

    private void SetBetterPositionGeneralMap()
    {
        estrategyMapsList[10] = new float[GridMap.instance.GetGridSizeX(), GridMap.instance.GetGridSizeY()];
        List<LinkedList<GameObject>> units = GameManager.instance.GetUnitList();
        LinkedListNode<GameObject> unitsList = units[IAPlayer].First;
        int i, j;
        float factor;
        for (; unitsList != null; unitsList = unitsList.Next)
        {
            i = GridMap.instance.CellCordFromWorldPoint(unitsList.Value.transform.position).x;
            j = GridMap.instance.CellCordFromWorldPoint(unitsList.Value.transform.position).y;
            for (int x = i - decayMapDistanceInCells >= 0 ? i - decayMapDistanceInCells : 0; x < GridMap.instance.GetGridSizeX() && x < i + decayMapDistanceInCells; x++)
            {
                for (int y = j - decayMapDistanceInCells >= 0 ? j - decayMapDistanceInCells : 0; y < GridMap.instance.GetGridSizeY() && y < j + decayMapDistanceInCells; y++)
                {
                    factor = Vector3.Distance(GridMap.instance.grid[i, j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition) <= GridMap.instance.GetCellRadius() * 2 * CombatManager.instance.GetGeneralRange()?
                        1 : 1 / Mathf.Pow(1 + Vector3.Distance(GridMap.instance.grid[i, j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition) - GridMap.instance.GetCellRadius() * 2 * CombatManager.instance.GetGeneralRange(), 2);
                    estrategyMapsList[10].SetValue((float)estrategyMapsList[10].GetValue(x, y) + factor, x, y);
                }
            }
        }
    }

    private void SetRelativeForceMap()
    {
        estrategyMapsList[18] = new float[GridMap.instance.GetGridSizeX(), GridMap.instance.GetGridSizeY()];
        List<LinkedList<GameObject>> units = GameManager.instance.GetUnitList();
        LinkedListNode<GameObject> unitsList = units[IAPlayer].First;
        int i, j;
        float factor;
        for (; unitsList != null; unitsList = unitsList.Next)
        {
            if(unitsList.Value.GetComponent<Units>() != null)
            {
                i = GridMap.instance.CellCordFromWorldPoint(unitsList.Value.transform.position).x;
                j = GridMap.instance.CellCordFromWorldPoint(unitsList.Value.transform.position).y;
                for (int x = i - decayMapDistanceInCells >= 0 ? i - decayMapDistanceInCells : 0; x < GridMap.instance.GetGridSizeX() && x < i + decayMapDistanceInCells; x++)
                {
                    for (int y = j - decayMapDistanceInCells >= 0 ? j - decayMapDistanceInCells : 0; y < GridMap.instance.GetGridSizeY() && y < j + decayMapDistanceInCells; y++)
                    {
                        factor = Vector3.Distance(GridMap.instance.grid[i, j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition) <= GridMap.instance.GetCellRadius() * 2 * unitsList.Value.GetComponent<Units>().GetMaxMovementsAvaliable() ?
                             unitsList.Value.GetComponent<CombatStats>().GetForce() : unitsList.Value.GetComponent<CombatStats>().GetForce() / Mathf.Pow(1 + Vector3.Distance(GridMap.instance.grid[i, j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition) - GridMap.instance.GetCellRadius() * 2 * unitsList.Value.GetComponent<Units>().GetMaxMovementsAvaliable(), 2);
                        estrategyMapsList[18].SetValue((float)estrategyMapsList[18].GetValue(x, y) + factor, x, y);
                    }
                }
            }
            
        }
        unitsList = IAPlayer == 0? units[1].First : units[0].First;
        for (; unitsList != null; unitsList = unitsList.Next)
        {
            if (unitsList.Value.GetComponent<Units>() != null)
            {
                i = GridMap.instance.CellCordFromWorldPoint(unitsList.Value.transform.position).x;
                j = GridMap.instance.CellCordFromWorldPoint(unitsList.Value.transform.position).y;
                for (int x = i - decayMapDistanceInCells >= 0 ? i - decayMapDistanceInCells : 0; x < GridMap.instance.GetGridSizeX() && x < i + decayMapDistanceInCells; x++)
                {
                    for (int y = j - decayMapDistanceInCells >= 0 ? j - decayMapDistanceInCells : 0; y < GridMap.instance.GetGridSizeY() && y < j + decayMapDistanceInCells; y++)
                    {
                        factor = Vector3.Distance(GridMap.instance.grid[i, j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition) <= GridMap.instance.GetCellRadius() * 2 * unitsList.Value.GetComponent<Units>().GetMaxMovementsAvaliable() ?
                            -unitsList.Value.GetComponent<CombatStats>().GetForce() : -unitsList.Value.GetComponent<CombatStats>().GetForce() / Mathf.Pow(1 + Vector3.Distance(GridMap.instance.grid[i, j].GlobalPosition, GridMap.instance.grid[x, y].GlobalPosition) - GridMap.instance.GetCellRadius() * 2 * unitsList.Value.GetComponent<Units>().GetMaxMovementsAvaliable(), 2);
                        estrategyMapsList[18].SetValue((float)estrategyMapsList[18].GetValue(x, y) + factor, x, y);
                    }
                }
            }
        }
    }

    public void UpdateMaps()
    {
        SetBetterPositionGeneralMap();
        SetRelativeForceMap();
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
            UpdateMaps();
    }

    private void OnDrawGizmos()
    {
        if(debugView != DebugView.nothing && debugView != DebugView.relative_force)
        {
            float maxCost = float.MinValue;
            float minCost = float.MaxValue;
            foreach (float n in estrategyMapsList[(int)debugView - 1])
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
                    if ((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) == float.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / maxCost, (float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / maxCost, (float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / maxCost, 1); ;
                    }

                    Gizmos.DrawCube(GridMap.instance.grid[i, j].GlobalPosition, Vector3.one * (GridMap.instance.GetCellRadius() * 2 * 19 / 20));
                }
            }
        }
        else if(debugView == DebugView.relative_force)
        {
            float maxCost = float.MinValue;
            float minCost = float.MaxValue;
            float costDiference;
            foreach (float n in estrategyMapsList[(int)debugView - 1])
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
            costDiference = maxCost - minCost;
            for (int i = 0; i < GridMap.instance.GetGridSizeX(); i++)
            {
                for (int j = 0; j < GridMap.instance.GetGridSizeY(); j++)
                {
                    if ((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) == float.MaxValue)
                    {
                        Gizmos.color = Color.white;
                    }
                    else if ((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) == float.MinValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        if((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) > 0f)
                        {
                            Gizmos.color = new Color((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / (costDiference * 2) +0.5f, (float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / (costDiference * 2) + 0.5f, (float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / (costDiference * 2) + 0.5f, 1); 
                        }
                        else if((float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) < 0f)
                        {
                            Gizmos.color = new Color(0.5f +(float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / (costDiference * 2), 0.5f + (float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / (costDiference * 2), 0.5f + (float)estrategyMapsList[(int)debugView - 1].GetValue(i, j) / (costDiference * 2) , 1);
                        }
                        else
                        {
                            Gizmos.color = new Color(0.5f,0.5f,0.5f, 1);
                        }
                    }
                    Gizmos.DrawCube(GridMap.instance.grid[i, j].GlobalPosition, Vector3.one * (GridMap.instance.GetCellRadius() * 2 * 19 / 20));
                }
            }
        }
    }
}