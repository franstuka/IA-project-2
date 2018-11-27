using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI goldText;    

	// Use this for initialization
	void Start () {          
        goldText.text = "" + gameManager.playersGold[0];        
    }
	
	// Update is called once per frame
	void Update () {       

    }
}
