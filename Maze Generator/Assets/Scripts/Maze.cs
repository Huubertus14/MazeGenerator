using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Cell {
	public bool visited;//visited or not
	public GameObject north;//Called as 1
	public GameObject east;//Called as 2
	public GameObject west;//Called as 3
	public GameObject south;//Called as 4
	public Cell (){
		north = null;
		east = null;
		west = null;
		south = null;
	}
}

public class Maze : MonoBehaviour {
    
	//wall prefab & variables
	public GameObject wall;
    private float wallLength = 1.0f;
    private GameObject tempWall = null;

    public int xSize = 10;
	public int ySize = 10;
    
	//position of the grid
	private Vector3 initialPos;

	//GameObject that holds the walls
	public GameObject wallHolder;

	//holds all cells & variables for the cells
	private Cell[] cells;
	private int totalCells = 0;
	private int visitedCells = 0;
	private int currentCell = 0;
    private int cellProcess = 0;
    
    private int currentNeighbour = 0;
	private bool startedBuilding = false;
	private int wallToBreak = 0;
	private List<int> lastCell;
	private int backingUp = 0;

    //Slider vars
    //These var are used to make the width and height of the maze
    private Slider xSlider;
    private Slider ySlider;

    //Floor/Tiles variables
    public GameObject firstFloorTile;
    public GameObject lastFloorTile;
    private Vector3 firstTilePosition;
    private Vector3 lastTilePosition;
    

	// Use this for initialization
	void Start () {
        xSlider = GameObject.FindGameObjectWithTag("xSlider").GetComponent<Slider>();
        ySlider = GameObject.FindGameObjectWithTag("ySlider").GetComponent<Slider>();

        wallHolder = new GameObject();
        wallHolder.name = "WallHolder";
        wallHolder.transform.position = new Vector3(-0.7f,0,0);
        CreateMaze();
    }

    public void GenerateButtonClick()
    {
       // Destroy(wallHolder);
       // Destroy(this.gameObject);
    }

    void CreateMaze()
    {
        xSize = (int) xSlider.value;
        ySize = (int) ySlider.value;

        //Setting the default values
        //currentCell = 0;
        backingUp = 0;
        visitedCells = 0;
        lastCell = new List<int>();
        lastCell.Clear();
        initialPos = new Vector3((-xSize / 2) + wallLength / 2, wallLength / 2, (-ySize / 2) + wallLength);
        //this will create the grid of walls
        CreateWalls();
    }

	void CreateWalls () {
		Vector3 myPos = initialPos;
		totalCells = xSize * ySize;
		cells = new Cell[totalCells];

		//For X
		//Generates walls on the X axis
		for (int j = 0; j < ySize; j++){
			for (int i = 0; i <= xSize; i++) {

				myPos = new Vector3(initialPos.x+(i*wallLength)-wallLength/2,0.5f,initialPos.z+(j*wallLength)-wallLength/2);
				tempWall = Instantiate(wall,myPos,Quaternion.identity) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}
		//For Y
		//Generates walls on the Z axis
		for (int l = 0; l <= ySize; l++){
			for (int k = 0; k < xSize;k++) {
				myPos = new Vector3(initialPos.x+(k*wallLength),0.5f,initialPos.z+(l*wallLength)-wallLength);
				tempWall = Instantiate(wall,myPos,Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}
		//Assignes all the walls for all the cells
		CreateCells ();
	}

	void CreateCells (){
		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		int termCount = 0;
		int childProcess = 0;
		int eastWestProcess = 0;

        


		allWalls = new GameObject[children];
		//Get all the walls in an array
		for (int j = 0; j < children; ++j) {
			allWalls[j] = wallHolder.transform.GetChild (j).gameObject;
		}

		//Assigning all the walls
		for (cellProcess = 0; cellProcess < cells.Length; cellProcess++) {

			if (termCount == xSize){
				eastWestProcess ++;
				termCount = 0;
			}

			cells[cellProcess] = new Cell();
			cells[cellProcess].east = allWalls[eastWestProcess];
			cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];

				eastWestProcess++;

			termCount++;
			childProcess++;
			cells[cellProcess].west = allWalls[eastWestProcess];
			cells[cellProcess].north = allWalls[(childProcess+(xSize+1)*ySize)+xSize-1];

		}
        
		//Create the maze
		CreateDFSMaze ();

        //Get the bottom left tile/wall
        firstTilePosition = allWalls[0].transform.position;
        lastTilePosition = allWalls[allWalls.Length - 1].transform.position;
        CreateFloor();
    }

    void CreateFloor()
    {
        Instantiate(firstFloorTile);
        firstFloorTile.transform.position = new Vector3((firstTilePosition.x + wallLength / 2) , firstTilePosition.y, firstTilePosition.z );

        Instantiate(lastFloorTile);
        lastFloorTile.transform.position = new Vector3((lastTilePosition.x ), lastTilePosition.y, lastTilePosition.z - (wallLength/2));
    }

	void CreateDFSMaze (){

		while (visitedCells < totalCells) {
			if (startedBuilding){
				//gives a random neighbour
				GiveMeNieghbour();

				if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true){
					//breaks the wall in betweel currentCell and neighbourCell
					BreakWall();
					cells[currentNeighbour].visited = true;
					visitedCells++;
					lastCell.Add(currentCell);
					currentCell = currentNeighbour;
					if (lastCell.Count > 0)
					backingUp = lastCell.Count-1;
				}
			}

			if (!startedBuilding){
				currentCell = Random.Range(0,cells.Length);
				startedBuilding = true;
				cells[currentCell].visited = true;
				visitedCells++;

			}
		}
	}

	void BreakWall(){
		switch (wallToBreak){
		case 1 : DestroyImmediate(cells[currentCell].north); break;
		case 2 : DestroyImmediate(cells[currentCell].east); break;
		case 3 : DestroyImmediate(cells[currentCell].west); break;
		case 4 : DestroyImmediate(cells[currentCell].south); break;
		}
	}

	void GiveMeNieghbour (){
		int length = 0;
		int[] neighbour = new int[4];
		int[] connectingWall = new int[4];
		int check =0;
		check = ((currentCell+1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;
		//get the west wall
		if (currentCell + 1 < totalCells && (currentCell+1) != check && xSize > 1) {
			//Debug.Log(currentCell + 1);
			if (cells[currentCell+1].visited == false){
				neighbour[length] = currentCell+1;
				connectingWall[length] = 3;
				length++;
			}
		}
		//get the east wall
		if (currentCell - 1 >= 0 && currentCell != check && xSize > 1) {
			//Debug.Log(currentCell - 1);
			if (cells[currentCell-1].visited == false){
				neighbour[length] = currentCell-1;
				connectingWall[length] = 2;
				length++;
			}
		}
		//get the north
		if (currentCell + xSize < totalCells && ySize > 1) {
			//Debug.Log(currentCell+xSize);
			if (cells[currentCell+xSize].visited == false){
				neighbour[length] = currentCell+xSize;
				connectingWall[length] = 1;
				length++;
			}
		}
		//get the south
		if (currentCell - xSize >= 0 && ySize > 1) {
			//Debug.Log(currentCell-xSize);
			if (cells[currentCell-xSize].visited == false){
				neighbour[length] = currentCell-xSize;
				connectingWall[length] = 4;
				length++;
			}
		}
		if (length != 0){
			int theChosenOne = Random.Range (0, length);
			currentNeighbour = neighbour[theChosenOne];
			wallToBreak = connectingWall[theChosenOne];
		}
		else {

			if (backingUp >= 0){
				currentCell = lastCell[backingUp];
				backingUp--;
			}

		}
	
	}

    
}


