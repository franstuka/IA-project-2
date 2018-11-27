using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenarioGenerator : MonoBehaviour {

    //Percentajes, adobe this values the cell will be setted
    [SerializeField] float MountainPercentaje = 0.9f;
    [SerializeField] float HillPercentaje = 0.5f;
    [SerializeField] float ForestPercenaje = 0.45f;
    [SerializeField] float ForestAdobeHillsPercenaje = 0.65f; //forest will override hills with less than this value

    [SerializeField] Color MountainColor;
    [SerializeField] Color ForestColor;
    [SerializeField] Color PlainColor;
    [SerializeField] Color MinesColor;
    [SerializeField] Color CastleColor;
    [SerializeField] Color HillsColor;
    [SerializeField] Material material;
    [SerializeField] int textureResolution = 20;
    [SerializeField] float noiseScale = 0.1f;
    [SerializeField] bool isRandom = true;
    [SerializeField] int seed;
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

        //SetSeedGen(seed); //set the seed

        //Get Difuse

        Texture2D difuseTexture = new Texture2D(numCellsX, numCellsY); //texture resolution = 1 because is the grid base layer

        Texture2D noiseLayer1 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, 1 , seed); //noise for mountains
        Texture2D noiseLayer2 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, 1 , seed + 1); //noise for woods
        Texture2D noiseLayer3 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale *10, 1 , seed + 3); //noise for resources

        SetCellsProceduralSimple(CellTypes.PLAIN, 0.0f, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer1); //set plain
        SetCellsProceduralSimple(CellTypes.HILLS, HillPercentaje, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer1); //set Hills
        SetCellsProceduralSimple(CellTypes.MOUNTAINS, MountainPercentaje, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer1); //set Mountains
        SetCellsProceduralReplace(CellTypes.FOREST, ForestPercenaje, ForestAdobeHillsPercenaje, ref numCellsX, ref numCellsY, ref difuseTexture, ref noiseLayer2, ref noiseLayer1);

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

        for (int x = 0; x < numCellsX * textureResolution; x++)
        {
            for (int y = 0; y < numCellsY * textureResolution; y++)
            {
                if(noiseTexture.GetPixel(x,y).grayscale > overridePercentaje)
                {
                    finalTexture.SetPixel(x, y, colorToSet);
                    GridMap.instance.grid[x/ textureResolution, y/ textureResolution].CellType = cellTypes;
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

        for (int x = 0; x < numCellsX * textureResolution; x++)
        {
            for (int y = 0; y < numCellsY * textureResolution; y++)
            {
                if (noiseTexture.GetPixel(x, y).grayscale > baseOverridePercentaje && noiseTextureReplaced.GetPixel(x, y).grayscale < otherOverridePercentaje)
                {
                    finalTexture.SetPixel(x, y, colorToSet);
                    GridMap.instance.grid[x/ textureResolution, y/ textureResolution].CellType = cellTypes;
                }
            }
        }
    }

    private void SetSeedGen(int seed)
    {
        isRandom = false;
        noise = new Noise(GetRandomGen());
    }

    private int GetRandomGen()
    {
        if(isRandom)
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
