using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenarioGenerator : MonoBehaviour {

    ////GOOOOO FOR IT

    [SerializeField] Color MontainColor;
    [SerializeField] Color WoodsColor;
    [SerializeField] Color PlainColor;
    [SerializeField] Color MinesColor;
    [SerializeField] Color CastleColor;
    [SerializeField] Color HillsColor;
    [SerializeField] Material material;
    [SerializeField] int textureResolution = 1;
    [SerializeField] float noiseScale;
    [SerializeField] bool isRandom;
    [SerializeField] int seed;
    private Texture2D mapTexture;
    private Noise noise;


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
        mapTexture = new Texture2D(numCellsX * textureResolution, numCellsY* textureResolution);
        Debug.Log("Resolution = " + numCellsX * textureResolution + " , " + numCellsY * textureResolution);
        for (int x = 0; x < numCellsX* textureResolution; x++)
        {
            for (int y = 0; y < numCellsY* textureResolution; y++)
            {
                float color = noise.Evaluate(new Vector3(x * cellRadius * noiseScale/ textureResolution, 0f, y * cellRadius * noiseScale/ textureResolution));
                color = (color + 1) / 2;
                mapTexture.SetPixel(x, y, new Color(color, color, color));
            }
        }
        mapTexture.Apply();
        material.mainTexture = mapTexture;
    }

    private Texture2D CreateNewTexture(ref int numCellsX, ref int numCellsY, ref float cellRadius)
    {
        mapTexture = new Texture2D(numCellsX * textureResolution, numCellsY * textureResolution);
        Debug.Log("Resolution = " + numCellsX * textureResolution + " , " + numCellsY * textureResolution);
        for (int x = 0; x < numCellsX * textureResolution; x++)
        {
            for (int y = 0; y < numCellsY * textureResolution; y++)
            {

                float color = noise.Evaluate(new Vector3(x * cellRadius * noiseScale / textureResolution, 0f, y * cellRadius * noiseScale / textureResolution));
                color = (color + 1) / 2;
                mapTexture.SetPixel(x, y, new Color(color, color, color));
            }
        }
        mapTexture.Apply();
        return mapTexture;
    }

    private void CreateScenatio()
    {
        float cellRadius = GridMap.instance.GetCellRadius();
        int numCellsX = GridMap.instance.GetGridSizeX();
        int numCellsY = GridMap.instance.GetGridSizeY();
        CreateTexture(ref numCellsX, ref numCellsY, ref cellRadius);


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
