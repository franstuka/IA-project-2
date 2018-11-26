using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour { //By default this is for a quad grid

    #region Singleton

    public static GridMap instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of grid is trying to active");
            return;
        }
        instance = this;
        temporalGridObjects = new List<Vector2Int>();
        cellDiameter = CellRadius * 2;
        gridSizeX = Mathf.RoundToInt(WorldSize.x / cellDiameter);
        gridSizeY = Mathf.RoundToInt(WorldSize.y / cellDiameter);
        //Debug.Log(gridSizeX);
        CreateGrid();
    }
    #endregion

    public Cell[,] grid;
    public bool seeTypes = false;
    public bool seePathCost = false;
    public bool seePathFromStartCost = false;
    public bool seePathFromEndCost = false;
    public bool seeVisitedCells = false;
    public bool seeNumberOfAdjacents = false;
    public bool seeEnemyPath = false;
    public bool seeAStarChilds = false;
    //public Unit enemySelected;

    [SerializeField] private Vector2 WorldSize;
    [SerializeField] private float CellRadius;
    private List<Vector2Int> temporalGridObjects;
    private LayerMask enemyMask;
    private float cellDiameter;
    private int gridSizeX;
    private int gridSizeY;

    
    private void Start()
    {
        UpdateEnemyPositions();
    }

    private void Update()
    {
        UpdateEnemyPositions();
    }

    private void CreateGrid()
    {
        grid = new Cell[gridSizeX, gridSizeY];
        Vector3 gridBottonLeft = transform.position - Vector3.right * WorldSize.x / 2 - Vector3.forward * WorldSize.y / 2;
        
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridBottonLeft + Vector3.right * (x * cellDiameter + CellRadius) + Vector3.forward * (y * cellDiameter + CellRadius);
                grid[x, y] = new Cell(CellTypes.PLAIN, worldPoint);
            }
        }
    }

    private void UpdateEnemyPositions()
    {
        RaycastHit[] enemies = Physics.BoxCastAll(transform.position, Vector3.one * Mathf.Max(WorldSize.x, WorldSize.y), Vector3.one, 
            Quaternion.identity, Mathf.Max(WorldSize.x, WorldSize.y) * 2, enemyMask, QueryTriggerInteraction.Ignore);
        CleanNonStaticElementsOnGrid();
        Vector2Int pos;
        for (int i = 0; i<enemies.Length; i++)
        {
            pos = CellCordFromWorldPoint(enemies[i].collider.gameObject.transform.position);
            if(grid[pos.x, pos.y].CellType == CellTypes.PLAIN) //if cell is void
            {
                grid[pos.x, pos.y].CellType = CellTypes.PLAIN;//set enemie
                temporalGridObjects.Add(new Vector2Int(pos.x, pos.y));
            }   
        }
    }

    private void CleanNonStaticElementsOnGrid()
    {
        for(int i = 0; i< temporalGridObjects.Count; i++)
        {
            grid[temporalGridObjects[i].x, temporalGridObjects[i].y].unityOrConstructionOnCell = null;  
        }
        temporalGridObjects = new List<Vector2Int>();
    }

    public Cell CellFromWorldPoint(Vector3 worldPosition) 
    {
        
        float percentX = (worldPosition.x + WorldSize.x / 2) / WorldSize.x;
        float percentY = (worldPosition.z + WorldSize.y / 2) / WorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt((gridSizeX) * percentX);
        int y = Mathf.FloorToInt((gridSizeY) * percentY);
        return grid[x, y];
    }

    public Vector2Int CellCordFromWorldPoint(Vector3 worldPosition) 
    {
        
        float percentX = (worldPosition.x + WorldSize.x / 2) / WorldSize.x;
        float percentY = (worldPosition.z + WorldSize.y / 2) / WorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt((gridSizeX) * percentX);
        int y = Mathf.FloorToInt((gridSizeY) * percentY);
        return new Vector2Int(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(WorldSize.x, 1, WorldSize.y));

        if (grid != null)
        {
            /*if (seePathCost && enemySelected != null) //PATH COST
            {
                int maxCost = int.MinValue;
                int minCost = int.MaxValue;
                foreach (Cell n in grid)
                {

                    if (n.Node.NodeFinalCost != int.MaxValue)
                    {
                        if (maxCost < n.Node.NodeFinalCost)
                        {
                            maxCost = n.Node.NodeFinalCost;
                        }
                        else if (minCost > n.Node.NodeFinalCost)
                        {
                            minCost = n.Node.NodeFinalCost;
                        }
                    }
                }
                foreach (Cell n in grid)
                {
                    if (n.Node.NodeFinalCost == int.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)n.Node.NodeFinalCost / maxCost, (float)n.Node.NodeFinalCost / maxCost, (float)n.Node.NodeFinalCost / maxCost, 1); ;
                    }

                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seePathFromStartCost && enemySelected != null) //PATH INITIAL COST
            {
                int maxCost = 1;
                int minCost = 1;
                float factor;

                foreach (Cell n in grid)
                {

                    if (n.Node.FromInitialCost != int.MaxValue)
                    {
                        if (maxCost < n.Node.FromInitialCost)
                        {
                            maxCost = n.Node.FromInitialCost;
                        }
                        else if (minCost > n.Node.FromInitialCost)
                        {
                            minCost = n.Node.FromInitialCost;
                        }
                    }
                }
                factor = minCost / maxCost;
                foreach (Cell n in grid)
                {
                    if (n.Node.FromInitialCost == int.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)n.Node.FromInitialCost / maxCost, (float)n.Node.FromInitialCost / maxCost, (float)n.Node.FromInitialCost / maxCost, 1);
                    }

                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seePathFromEndCost && enemySelected != null) // PATH FROM FINAL COST
            {
                int maxCost = int.MinValue;
                int minCost = 1;
                foreach (Cell n in grid)
                {

                    if (n.Node.FromFinalCost != int.MaxValue)
                    {
                        if (maxCost < n.Node.FromFinalCost)
                        {
                            maxCost = n.Node.FromFinalCost;
                        }
                        else if (minCost > n.Node.FromFinalCost)
                        {
                            minCost = n.Node.FromFinalCost;
                        }
                    }
                }
                foreach (Cell n in grid)
                {
                    if (n.Node.FromFinalCost == int.MaxValue)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = new Color((float)n.Node.FromFinalCost / maxCost, (float)n.Node.FromFinalCost / maxCost, (float)n.Node.FromFinalCost / maxCost, 1);
                    }

                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seeVisitedCells && enemySelected != null) // PATH FROM FINAL COST
            {
                foreach (Cell n in grid)
                {
                    if (n.Node.visited)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }
                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seeNumberOfAdjacents && enemySelected != null) // PATH FROM FINAL COST
            {
                foreach (Cell n in grid)
                {
                    switch (n.Node.AvaibleAdjacentNodes)
                    {
                        case 0:
                            {
                                Gizmos.color = Color.black;
                                break;
                            }
                        case 1:
                            {
                                Gizmos.color = Color.red;
                                break;
                            }
                        case 2:
                            {
                                Gizmos.color = Color.yellow;
                                break;
                            }
                        case 3:
                            {
                                Gizmos.color = Color.green;
                                break;
                            }
                        case 4:
                            {
                                Gizmos.color = Color.cyan;
                                break;
                            }
                        case 5:
                            {
                                Gizmos.color = Color.blue;
                                break;
                            }
                        case 6:
                            {
                                Gizmos.color = Color.magenta;
                                break;
                            }
                        case 7:
                            {
                                Gizmos.color = Color.grey;
                                break;
                            }
                        case 8:
                            {
                                Gizmos.color = Color.clear;
                                break;
                            }
                        default:
                            {
                                Gizmos.color = Color.white;
                                break;
                            }
                    }
                    Gizmos.DrawCube(n.GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                }
            }
            else if (seeEnemyPath && enemySelected != null)
            {
                int size = enemySelected.GetSavedPath().Count;
                LinkedList<Vector2Int> path = enemySelected.GetSavedPath();
                LinkedListNode<Vector2Int> element = path.First;
                for (int i = 0; i < size; i++)
                {
                    Gizmos.color = new Color((float)i / size, (float)i / size, (float)i / size, 1);
                    Gizmos.DrawCube(grid[element.Value.x,element.Value.y].GlobalPosition, Vector3.one * (cellDiameter * 19 / 20));
                    element = element.Next;
                }
            }
            else if (seeAStarChilds && enemySelected != null)
            {
                Vector2Int initialPos = new Vector2Int(0,0);
                bool end = false;
                for (int x = 0; x < gridSizeX && !end; x++)
                {
                    for (int y = 0; y < gridSizeY && !end; y++)
                    {
                        if (grid[x,y].Node.FromInitialCost == 0)
                        {
                            end = true;
                            initialPos = new Vector2Int(x, y);
                        }
                    }
                }
                Gizmos.color = Color.red;
                PrintBranchRecursive(initialPos.x, initialPos.y);
            }*/
        }
    } 

    private void PrintBranchRecursive(int x, int y)
    {
        LinkedList<Vector2Int> childList = grid[x, y].Node.GetChillds();
        LinkedListNode<Vector2Int> node = childList.First;
       
        while (node != null)
        {
            Gizmos.DrawLine(grid[node.Value.x, node.Value.y].GlobalPosition, grid[x, y].GlobalPosition);
            PrintBranchRecursive(node.Value.x, node.Value.y);
            node = node.Next;
        }
    }

    public int GetGridSizeX()
    {
        return gridSizeX;
    }

    public int GetGridSizeY()
    {
        return gridSizeY;
    }

    public float GetCellRadius()
    {
        return CellRadius;
    }
}
