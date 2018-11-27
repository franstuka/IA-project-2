using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    [SerializeField] private Button changeTurnButton;
    [SerializeField] private TextMeshProUGUI changeTurnText;


	// Use this for initialization
	void Start () {
        changeTurnButton.onClick.AddListener(NewTurn);
        changeTurnText.color = new Color32(0, 0, 0, 255);
    }
	
	// Update is called once per frame
	void Update () {            
        
    }  

    void NewTurn()
    {
        gameManager.SetTurn((byte)Mathf.Abs(gameManager.GetTurn() - 1));
    }


}
