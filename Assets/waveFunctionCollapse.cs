using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class waveFunctionCollapse : MonoBehaviour
{
    public const int NUM_TILES = 24;
    public const float TILE_SIZE = 2f;
    public const int MAP_SIZE = 30;

    public GameObject[] tiles = new GameObject[NUM_TILES]; //array of gameobjects
    string[] tileNames = new string[NUM_TILES]; //array of tile names

    Tile[,] mapTileInfo = new Tile[MAP_SIZE, MAP_SIZE];

    bool isFinished = false;

    private TileRuleList rules;
    private TileWeights weights;


    // Start is called before the first frame update
    void Start()
    {
        // create an array of tiles names for readability
        for (int i=0; i < NUM_TILES; i++) 
        {
            tileNames[i] = tiles[i].name;
        }

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                mapTileInfo[i, j] = new Tile(tileNames, i, j);
            }
        }

        string baseDirectory = Application.streamingAssetsPath;
        string rulesFilePath = Path.Combine(baseDirectory, "tileRules.txt");
        rules = new TileRuleList(rulesFilePath);
        weights = new TileWeights();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isFinished) {
            //GET SPACE WITH LOWEST ENTROPY--------------------------------------------------------------------------------
            //Iterate through, get lowest value that's not 0
            int lowestValue = NUM_TILES + 1;
            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    if (mapTileInfo[i, j].getEntropy() < lowestValue && mapTileInfo[i, j].getEntropy() != 0)
                        lowestValue = mapTileInfo[i, j].getEntropy();
                }
            }

            if (lowestValue > NUM_TILES) {
                GameObject player = GameObject.FindWithTag("Player");
                PlayerMove playerMove = player.GetComponent<PlayerMove>();
                playerMove.enableCharacter();
                GameObject camera = GameObject.FindWithTag("MainCamera");
                MoveCamera moveCamera = camera.GetComponent<MoveCamera>();
                moveCamera.enableCamera();
                isFinished = true;
            }

            //iterate again, getting list of all spaces of that value
            List<(int x, int y)> tilesWithLowestEntropy = new List<(int x, int y)>();
            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    if (mapTileInfo[i, j].getEntropy() == lowestValue)
                    {
                        //contains a variable length list of coordinate pairs, corresponding to the tiles with the lowest entropy.
                        tilesWithLowestEntropy.Add((i, j)); 
                    }
                }
            }

            //COLLAPSE THE TILE AND PLACE IT------------------------------------------------------------------------------------------
            int whichTileToCollapse = Random.Range(0, tilesWithLowestEntropy.Count); //choose which tile to collapse randomly from list
            (int x, int y) tileToCollapseLocation = tilesWithLowestEntropy[whichTileToCollapse];
            int ttcx = tileToCollapseLocation.x; //get the tile to collapse's coordinates
            int ttcy = tileToCollapseLocation.y;
            Tile tileToCollapse = mapTileInfo[ttcx, ttcy]; //get the tile object corresponding to the tile to collapse

            //need to get the array index of the game object to instantiate from the tile object
            string instantiateThis;
            instantiateThis = tileToCollapse.collapse(weights.dict);
            int tileNumToInstantiate = 0;

            //find the index corresponding to the name string of the tile type
            for (int i = 0; i < NUM_TILES; i++)
            {
                if (tileNames[i] == instantiateThis)
                    tileNumToInstantiate = i;
            }
           
            //instantiate tile a fixed multiple of the coordinates away
            float instantiateHereX = TILE_SIZE * ttcx;
            float instantiateHereY = TILE_SIZE * ttcy;
            //needs to be rotated because of how the models transfered to unity
            Instantiate(tiles[tileNumToInstantiate], 
                        new Vector3(instantiateHereX, 0, instantiateHereY), 
                        Quaternion.Euler(new Vector3(-90, 0, 0)));
                                        
            //UPDATE MATRIX-------------------------------------------------------------------------------------------------
            //update the entropy and possible tiles of each space on the board based on the tile just selected
            Stack<Tile> stack = new Stack<Tile>();
            stack.Push(tileToCollapse); //put the tile we just set on the stack

            while (stack.Count > 0)
            {
                Tile currentTile = stack.Pop();
                List<(int x, int y)> directions = currentTile.getDirections(MAP_SIZE);
                //iterate through each of the directions we just got
                for (int i = 0; i < directions.Count; i++)
                {
                    //get the neighbor of that tile in given direction
                    int currentX = currentTile.getX() + directions[i].x;
                    int currentY = currentTile.getY() + directions[i].y;
                    Tile neighbor = mapTileInfo[currentX, currentY]; 
                    bool constrained = neighbor.constrain(currentTile.getPossibleTiles(), directions[i], rules.dict);
                    if (constrained) //if the neighbor's entropy is reduced at all
                    {
                        stack.Push(neighbor); //push said neighbor onto the stack to continue updating everything
                    }
                }

            }

            //EVALUATE IF WE NEED TO CONTINUE----------------------------------------------------------------------------
            int highestValue = 0;
            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    if (mapTileInfo[i, j].getEntropy() > highestValue)
                        highestValue = mapTileInfo[i, j].getEntropy();
                }
            }
            if (highestValue == 0) isFinished = true;
        }
    }
}
