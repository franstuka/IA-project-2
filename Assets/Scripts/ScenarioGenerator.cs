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
    [SerializeField] float textureResolution;
    private Texture2D mapTexture;

    private void CreateTexture()
    {

    }
    
    private void CreateScenatio()
    {
        float cellRadius = GridMap.instance.GetCellRadius();
        int numCellsX = GridMap.instance.GetGridSizeX();
        int numCellsY = GridMap.instance.GetGridSizeY();

        
    }
}
