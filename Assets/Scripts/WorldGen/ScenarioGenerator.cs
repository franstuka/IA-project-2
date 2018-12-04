using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenarioGenerator : MonoBehaviour {

    //Percentajes, adobe this values the cell will be setted
    [SerializeField] float MountainPercentaje = 0.9f;
    [SerializeField] float HillPercentaje = 0.5f;
    [SerializeField] float ForestPercenaje = 0.45f;
    [SerializeField] float ForestAdobeHillsPercenaje = 0.65f; //forest will override hills with less than this value
    [SerializeField] float BordersMineSecureZone = 0.05f;
    [SerializeField] float CastlePositionFromBorder = 0.15f;
    [SerializeField] float SecureZoneArroundCastle = 0.05f;
    [SerializeField] int CastleSize = 2;
    [SerializeField] float MidDivision = 0.3f;
    [SerializeField] float DistanceBetweenMines = 0.1f;
    
    [SerializeField] Color MountainColor;
    [SerializeField] Color ForestColor;
    [SerializeField] Color PlainColor;
    [SerializeField] Color MinesColor;
    [SerializeField] Color CastleColor;
    [SerializeField] Color HillsColor;
    [SerializeField] Color BlockedGoldColor;
    [SerializeField] Material material;
    [SerializeField] int textureResolution = 20;
    [SerializeField] float noiseScale = 0.1f;
    [SerializeField] bool isRandom = true;
    [SerializeField] int seed;

    //debug
    [SerializeField] bool SeeBlockedGoldZone = true;
    [SerializeField] bool ForceRandomSeed = true;

    private Noise noise;
    private Texture2D FinalTexture;
    private Texture2D ElevationTexture;


    private void Awake()
    {
        noise = new Noise(GetRandomGen());
    }
    private void Start()
    {
        CreateScenatio();
    }

    private void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            noise = new Noise(GetRandomGen());
            CreateScenatio();
        }
    }

    private void CreateScenatio()
    {
        float cellRadius = GridMap.instance.GetCellRadius();
        int numCellsX = GridMap.instance.GetGridSizeX();
        int numCellsY = GridMap.instance.GetGridSizeY();

        if(ForceRandomSeed || isRandom)
            SetSeedGen(seed); //set the seed

        //Get Difuse

        Texture2D difuseTexture = new Texture2D(numCellsX, numCellsY); //texture resolution = 1 because is the grid base layer

        Texture2D noiseLayer1 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, 1 , seed); //noise for mountains
        Texture2D noiseLayer2 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, 1 , seed + 1); //noise for woods
        Texture2D noiseLayer3 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale *10, 1 , seed + 3); //noise for resources

        SetCellsProceduralSimple(CellTypes.PLAIN, 0.0f, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer1); //set plain
        SetCellsProceduralSimple(CellTypes.HILLS, HillPercentaje, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer1); //set Hills
        SetCellsProceduralSimple(CellTypes.MOUNTAINS, MountainPercentaje, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer1); //set Mountains
        SetCellsProceduralReplace(CellTypes.FOREST, ForestPercenaje, ForestAdobeHillsPercenaje, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer2, ref noiseLayer1);
        SetMinesAndCastlePosition(ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer3);

        difuseTexture.Apply();
        FinalTexture = difuseTexture;
        material.mainTexture = FinalTexture;

        //Get elevation texture

        noiseLayer1 = CreateNewNoiseForSecundaryAlbedo(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, textureResolution, seed);
        ElevationTexture = noiseLayer1;
        material.SetTexture("_DetailAlbedoMap", noiseLayer1);
    }

    private Texture2D CreateNewNoiseTexture(ref int numCellsX, ref int numCellsY, ref float cellRadius, float localNoiseScale, int localTextureResolution, int seed) //Get Noise texture
    {
        Texture2D newTexture = new Texture2D(numCellsX * localTextureResolution, numCellsY * localTextureResolution);
        Debug.Log("Resolution = " + numCellsX * localTextureResolution + " , " + numCellsY * localTextureResolution);
        for (int x = 0; x < numCellsX * localTextureResolution; x++)
        {
            for (int y = 0; y < numCellsY * localTextureResolution; y++)
            {
                float color = noise.Evaluate(new Vector3(x * cellRadius * localNoiseScale / localTextureResolution, 0f, y * cellRadius * localNoiseScale / localTextureResolution));
                color = (color + 1) / 2;
                newTexture.SetPixel(x, y, new Color(color, color, color));
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    private Texture2D CreateNewNoiseForSecundaryAlbedo(ref int numCellsX, ref int numCellsY, ref float cellRadius, float localNoiseScale, int localTextureResolution, int seed) //Get Noise texture
    {
        Texture2D newTexture = new Texture2D(numCellsX * localTextureResolution, numCellsY * localTextureResolution);
        Debug.Log("Resolution = " + numCellsX * localTextureResolution + " , " + numCellsY * localTextureResolution);
        for (int x = 0; x < numCellsX * localTextureResolution; x++)
        {
            for (int y = 0; y < numCellsY * localTextureResolution; y++)
            {
                float color = noise.Evaluate(new Vector3(x * cellRadius * localNoiseScale / localTextureResolution, 0f, y * cellRadius * localNoiseScale / localTextureResolution));
                color = (color + 1) / 2;
                newTexture.SetPixel(x, y, new Color(0.5f +  color /2, 0.5f +  color /2, 0.5f + color/2));
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    private void SetCellsProceduralSimple(CellTypes cellTypes, float overridePercentaje , ref int numCellsX, ref int numCellsY, ref Texture2D finalTexture , ref Texture2D noiseTexture)
    {
        Color colorToSet = new Color();
        switch(cellTypes)
        {
            case CellTypes.PLAIN:
                {
                    colorToSet = PlainColor;
                    break;
                }
            case CellTypes.MOUNTAINS:
                {
                    colorToSet = MountainColor;
                    break;
                }
            case CellTypes.HILLS:
                {
                    colorToSet = HillsColor;
                    break;
                }
            case CellTypes.FOREST:
                {
                    colorToSet = ForestColor;
                    break;
                }
            case CellTypes.CASTLE:
                {
                    colorToSet = CastleColor;
                    break;
                }
            case CellTypes.MINE:
                {
                    colorToSet = MinesColor;
                    break;
                }
            default:
                {
                    Debug.LogError("CellType not setted");
                    break;
                }
        } //color setup

        for (int x = 0; x < numCellsX; x++)
        {
            for (int y = 0; y < numCellsY ; y++)
            {
                if(noiseTexture.GetPixel(x,y).grayscale > overridePercentaje)
                {
                    finalTexture.SetPixel(x, y, colorToSet);
                    GridMap.instance.grid[x, y].CellType = cellTypes;
                }         
            }
        }
    }

    private void SetCellsProceduralReplace(CellTypes cellTypes, float baseOverridePercentaje, float otherOverridePercentaje, ref int numCellsX, ref int numCellsY, ref Texture2D finalTexture, ref Texture2D noiseTexture,ref Texture2D noiseTextureReplaced)
    {
        Color colorToSet = new Color();
        switch (cellTypes)
        {
            case CellTypes.PLAIN:
                {
                    colorToSet = PlainColor;
                    break;
                }
            case CellTypes.MOUNTAINS:
                {
                    colorToSet = MountainColor;
                    break;
                }
            case CellTypes.HILLS:
                {
                    colorToSet = HillsColor;
                    break;
                }
            case CellTypes.FOREST:
                {
                    colorToSet = ForestColor;
                    break;
                }
            case CellTypes.CASTLE:
                {
                    colorToSet = CastleColor;
                    break;
                }
            case CellTypes.MINE:
                {
                    colorToSet = MinesColor;
                    break;
                }
            default:
                {
                    Debug.LogError("CellType not setted");
                    break;
                }
        } //color setup

        for (int x = 0; x < numCellsX; x++)
        {
            for (int y = 0; y < numCellsY; y++)
            {
                if (noiseTexture.GetPixel(x, y).grayscale > baseOverridePercentaje && noiseTextureReplaced.GetPixel(x, y).grayscale < otherOverridePercentaje)
                {
                    finalTexture.SetPixel(x, y, colorToSet);
                    GridMap.instance.grid[x, y].CellType = cellTypes;
                }
            }
        }
    }

    private bool SetMinesAndCastlePosition(ref int numCellsX, ref int numCellsY, ref Texture2D finalTexture , ref Texture2D noiseLayer)
    {
        //Get mayor axis
        int mayorAxis = numCellsX > numCellsY ? numCellsX : numCellsY;
        bool mayorAxisIsX = numCellsX > numCellsY ? true : false;
        byte[,] minesMatrix = new byte[numCellsX,numCellsY]; //code: 0 free, 1 invalid, 2 with mine
        List<Vector2Int> minesPosition = new List<Vector2Int>();

        #region delete borders

        byte distanceXToRemove = (byte)Mathf.CeilToInt(BordersMineSecureZone * numCellsX);
        byte distanceYToRemove = (byte)Mathf.CeilToInt(BordersMineSecureZone * numCellsY);
        Debug.Log(distanceXToRemove + " " + distanceYToRemove);
        //up x zone
        for (int i = 0; i < distanceXToRemove; i++)
        {
            for (int j = 0; j < numCellsY; j++)
            {
                minesMatrix[i, j] = 1;
                if(SeeBlockedGoldZone)
                    finalTexture.SetPixel(i, j, BlockedGoldColor);
            }
        }

        //up y zone
        for (int i = 0; i < numCellsX; i++)
        {
            for (int j = 0; j < distanceYToRemove; j++)
            {
                minesMatrix[i, j] = 1;
                if (SeeBlockedGoldZone)
                    finalTexture.SetPixel(i, j, BlockedGoldColor);
            }
        }

        //down x zone
        for (int i = numCellsX - distanceXToRemove; i < numCellsX; i++)
        {
            for (int j = 0; j < numCellsY; j++)
            {
                minesMatrix[i, j] = 1;
                if (SeeBlockedGoldZone)
                    finalTexture.SetPixel(i, j, BlockedGoldColor);
            }
        }
        
        //down y zone
        for (int i = 0; i < numCellsX; i++)
        {
            for (int j = numCellsY - distanceYToRemove; j < numCellsY; j++)
            {
                minesMatrix[i, j] = 1;
                if (SeeBlockedGoldZone)
                    finalTexture.SetPixel(i, j, BlockedGoldColor);
            }
        }

        #endregion

        #region PutCasttle in position and clean blocked terrain around

        // first from low values of matrix

        Vector2Int castleCenter = mayorAxisIsX ? new Vector2Int(Mathf.CeilToInt(numCellsX * CastlePositionFromBorder), numCellsY/2) : new Vector2Int(numCellsX/2, Mathf.CeilToInt(numCellsY * CastlePositionFromBorder));

        if(castleCenter.x - CastleSize < 0 || castleCenter.y - CastleSize < 0)
        {
            Debug.LogError("Castle don't have enought space");
            return false;
        }
        for (int i = castleCenter.x - CastleSize; i <= castleCenter.x + CastleSize; i++)
        {
            for (int j = castleCenter.y - CastleSize; j <= castleCenter.y + CastleSize; j++)
            {
                finalTexture.SetPixel(i, j, CastleColor);
                GridMap.instance.grid[i, j].CellType = CellTypes.CASTLE;
            }
        }
        //set blocked for gold distribution
        for (int i = castleCenter.x - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) >= 0? castleCenter.x - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * numCellsX) : 0;
            i <= castleCenter.x + CastleSize + Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) && i < numCellsX; i++)
        {
            for (int j = castleCenter.y - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) >= 0? castleCenter.y - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * numCellsY): 0;
                j <= castleCenter.y + CastleSize + Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) && j< numCellsY; j++)
            {
                minesMatrix[i, j] = 1;
                if (GridMap.instance.grid[i, j].CellType == CellTypes.MOUNTAINS) //not blocked terrain allowed arround castle
                {
                    GridMap.instance.grid[i, j].CellType = CellTypes.HILLS;
                    finalTexture.SetPixel(i, j, HillsColor);
                }
                if (SeeBlockedGoldZone)
                    finalTexture.SetPixel(i, j, BlockedGoldColor);
            }
        }

        // high matrix values

        if (mayorAxisIsX)//we need to work with them as diferent because if not they are not symetric
        {
            for (int i = numCellsX - castleCenter.x - CastleSize -1; i <= numCellsX - castleCenter.x + CastleSize -1 ; i++)
            {
                for (int j = numCellsY - castleCenter.y - CastleSize; j <= numCellsY - castleCenter.y + CastleSize; j++)
                {
                    finalTexture.SetPixel(i, j, CastleColor);
                    GridMap.instance.grid[i, j].CellType = CellTypes.CASTLE;
                }
            }
            //set blocked for gold distribution
            for (int i = numCellsX - castleCenter.x - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) -1>= 0 ? numCellsX - castleCenter.x - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * numCellsX) -1: numCellsX - 1;
                i <= numCellsX - castleCenter.x + CastleSize + Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) -1 && i < numCellsX; i++)
            {
                for (int j = numCellsY - castleCenter.y - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) >= 0 ? numCellsY - castleCenter.y - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * numCellsY) : numCellsY - 1;
                    j <= numCellsY - castleCenter.y + CastleSize + Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) && j < numCellsY; j++)
                {
                    minesMatrix[i, j] = 1;
                    if(GridMap.instance.grid[i, j].CellType == CellTypes.MOUNTAINS) //not blocked terrain allowed arround castle
                    {
                        GridMap.instance.grid[i, j].CellType = CellTypes.HILLS;
                        finalTexture.SetPixel(i, j, HillsColor);
                    }
                    if (SeeBlockedGoldZone)
                        finalTexture.SetPixel(i, j, BlockedGoldColor);
                }
            }
        }
        else
        {
            for (int i = numCellsX - castleCenter.x - CastleSize; i <= numCellsX - castleCenter.x + CastleSize; i++)
            {
                for (int j = numCellsY - castleCenter.y - CastleSize -1; j <= numCellsY - castleCenter.y + CastleSize -1; j++)
                {
                    finalTexture.SetPixel(i, j, CastleColor);
                    GridMap.instance.grid[i, j].CellType = CellTypes.CASTLE;
                }
            }
            //set blocked for gold distribution
            for (int i = numCellsX - castleCenter.x - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) >= 0 ? numCellsX - castleCenter.x - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * numCellsX) : numCellsX - 1;
                i <= numCellsX - castleCenter.x + CastleSize + Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) && i < numCellsX; i++)
            {
                for (int j = numCellsY - castleCenter.y - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) -1 >= 0 ? numCellsY - castleCenter.y - CastleSize - Mathf.CeilToInt(SecureZoneArroundCastle * numCellsY) -1 : numCellsY - 1;
                    j <= numCellsY - castleCenter.y + CastleSize + Mathf.CeilToInt(SecureZoneArroundCastle * mayorAxis) -1 && j < numCellsY; j++)
                {
                    minesMatrix[i, j] = 1;
                    if (GridMap.instance.grid[i, j].CellType == CellTypes.MOUNTAINS) //not blocked terrain allowed arround castle
                    {
                        GridMap.instance.grid[i, j].CellType = CellTypes.HILLS;
                        finalTexture.SetPixel(i, j, HillsColor);
                    }
                    if (SeeBlockedGoldZone)
                        finalTexture.SetPixel(i, j, BlockedGoldColor);
                }
            }
        }
        
        return true;

        #endregion
    }

    private int GetManhattanDistance(Vector2Int from,Vector2Int to)
    {  
        return Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.x);
    }

    private void SetSeedGen(int seed)
    {
        noise = new Noise(GetRandomGen());
        isRandom = false;
    }

    private int GetRandomGen()
    {
        if(isRandom || ForceRandomSeed)
        {
            seed = System.Convert.ToInt32(System.DateTime.Now.ToString("ddHHmmss"));
            return seed;
        }
        else
        {
            return seed;
        }
    }
}
