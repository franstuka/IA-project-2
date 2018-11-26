using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenarioGenerator : MonoBehaviour {

    //Percentajes, adobe this values the cell will be setted
    [SerializeField] float MountainPercentaje = 0.9f;
    [SerializeField] float HillPercentaje = 0.5f;
    [SerializeField] float ForestPercenaje = 0.4f;
    [SerializeField] float ForestAdobeHillsPercenaje = 0.65f; //forest will override hills with less than this value

    [SerializeField] Color MountainColor;
    [SerializeField] Color ForestColor;
    [SerializeField] Color PlainColor;
    [SerializeField] Color MinesColor;
    [SerializeField] Color CastleColor;
    [SerializeField] Color HillsColor;
    [SerializeField] Material material;
    [SerializeField] int textureResolution = 1;
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

    private void CreateTexture(ref int numCellsX, ref int numCellsY, ref float cellRadius)
    {
        Texture2D newTexture = new Texture2D(numCellsX * textureResolution, numCellsY* textureResolution);
        Debug.Log("Resolution = " + numCellsX * textureResolution + " , " + numCellsY * textureResolution);
        for (int x = 0; x < numCellsX* textureResolution; x++)
        {
            for (int y = 0; y < numCellsY* textureResolution; y++)
            {
                float color = noise.Evaluate(new Vector3(x * cellRadius * noiseScale/ textureResolution, 0f, y * cellRadius * noiseScale/ textureResolution));
                color = (color + 1) / 2;
                newTexture.SetPixel(x, y, new Color(color, color, color));
            }
        }
        newTexture.Apply();
        material.mainTexture = newTexture;
    }

    private Texture2D CreateNewNoiseTexture(ref int numCellsX, ref int numCellsY, ref float cellRadius, float localNoiseScale , int seed) //Get Noise texture
    {
        Texture2D newTexture = new Texture2D(numCellsX * textureResolution, numCellsY * textureResolution);
        Debug.Log("Resolution = " + numCellsX * textureResolution + " , " + numCellsY * textureResolution);
        for (int x = 0; x < numCellsX * textureResolution; x++)
        {
            for (int y = 0; y < numCellsY * textureResolution; y++)
            {
                float color = noise.Evaluate(new Vector3(x * cellRadius * localNoiseScale / textureResolution, 0f, y * cellRadius * localNoiseScale / textureResolution));
                color = (color + 1) / 2;
                newTexture.SetPixel(x, y, new Color(color, color, color));
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    private void CreateScenatio()
    {
        float cellRadius = GridMap.instance.GetCellRadius();
        int numCellsX = GridMap.instance.GetGridSizeX();
        int numCellsY = GridMap.instance.GetGridSizeY();

        //SetSeedGen(seed); //set the seed

        Texture2D temporalTexture = new Texture2D(numCellsX * textureResolution, numCellsY * textureResolution);
        CreateTexture(ref numCellsX, ref numCellsY, ref cellRadius);

        Texture2D noiseLayer1 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, seed); //noise for mountains
        Texture2D noiseLayer2 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale, seed + 1); //noise for woods
        Texture2D noiseLayer3 = CreateNewNoiseTexture(ref numCellsX, ref numCellsY, ref cellRadius, noiseScale *10, seed + 3); //noise for resources

        SetCellsProceduralSimple(CellTypes.PLAIN, 0.0f, ref numCellsX, ref numCellsY, ref temporalTexture, ref noiseLayer1); //set plain
        SetCellsProceduralSimple(CellTypes.HILLS, HillPercentaje, ref numCellsX, ref numCellsY, ref temporalTexture, ref noiseLayer1); //set Hills
        SetCellsProceduralSimple(CellTypes.MOUNTAINS, MountainPercentaje, ref numCellsX, ref numCellsY, ref temporalTexture, ref noiseLayer1); //set Mountains
        SetCellsProceduralReplace(CellTypes.FOREST, ForestPercenaje, ForestAdobeHillsPercenaje, ref numCellsX, ref numCellsY, ref temporalTexture, ref noiseLayer2, ref noiseLayer1);

        temporalTexture.Apply();
        FinalTexture = temporalTexture;
        material.mainTexture = FinalTexture;
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
                    finalTexture.SetPixel(x, y, colorToSet);
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
