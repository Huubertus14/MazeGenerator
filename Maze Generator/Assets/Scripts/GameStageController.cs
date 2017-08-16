using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageController : MonoBehaviour {

    public GameObject player;
    public GameObject[] spawnPoint;
    

    private bool isGameStarted;
    
    
	// Use this for initialization
	void Start () {
        //There are multiple spawn points.
        //But the player always needs to get the first one


        isGameStarted = false;
	}
	
    public void StartGame()
    {
        //Methode is called when the game starts
        //initiate the player and removes the UI
        isGameStarted = true;

        //Find the spawnpoints
        spawnPoint = GameObject.FindGameObjectsWithTag("FloorTile");

        //Makes the player and set him to its position
        Instantiate(player);
        player.transform.position = spawnPoint[0].transform.position;
    }

	public bool GetIsGameStarted() { return isGameStarted; }
}
