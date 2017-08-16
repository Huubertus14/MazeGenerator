using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * 
 * 
 * 
 * */

public class ButtonController : MonoBehaviour {

    public GameObject maze;

	public void NewMazePressed()
    {
        //Destroys old mazes
        DestroyMazes();

        //Instantiate a new maze
        Instantiate(maze);
    }

    void DestroyMazes()
    {
        //Destroys every other maze that exist
        //Also desgtroys the wall holders within the mazes
        GameObject[] mMazes = GameObject.FindGameObjectsWithTag("Maze");
        foreach (var mazes in mMazes)
        {
            Maze i = mazes.GetComponent <Maze> ();
            Destroy(i.wallHolder);
            Destroy(mazes);
        }

        //Destroy the floor of the maze
        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("FloorTile");
        foreach (var tile in floorTiles)
        {
            Destroy(tile);
        }
    }
}
