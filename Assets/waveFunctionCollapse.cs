using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class waveFunctionCollapse : MonoBehaviour
{
    public const int NUM_TILES = 3;
    public const float TILE_SIZE = 100 * 2f;
    public const int MAP_SIZE = 30;

    public GameObject[] tiles = new GameObject[NUM_TILES]; //array of gameobjects
    string[] tileNames = new string[NUM_TILES]; //array of tile names

    Tile[,] mapTileInfo = new Tile[MAP_SIZE, MAP_SIZE];
    GameObject[,] map = new GameObject[MAP_SIZE, MAP_SIZE];

    bool isNetEntropyZero = false;
    bool abort = false;

    private TileRuleList rules;


    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < NUM_TILES; i++) //have an array of tiles names for readability
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
    }


    // Update is called once per frame
    void Update()
    {
        if (!isNetEntropyZero) {
            //GET SPACE WITH LOWEST ENTROPY--------------------------------------------------------------------------------
            //Iterate through, get lowest value
            int lowestValue = NUM_TILES + 1;
            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    if (mapTileInfo[i, j].getEntropy() < lowestValue && mapTileInfo[i, j].getEntropy() != 0)
                        lowestValue = mapTileInfo[i, j].getEntropy();
                }
            }

            //iterate again, getting list of all spaces of that value
            List<(int x, int y)> tilesWithLowestEntropy = new List<(int x, int y)>();
            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    if (mapTileInfo[i, j].getEntropy() == lowestValue)
                    {
                        tilesWithLowestEntropy.Add((i, j)); //contains a variable length list of coordinate pairs, corresponding to the tiles with the lowest entropy.
                    }
                }
            }

            //COLLAPSE THE TILE AND PLACE IT------------------------------------------------------------------------------------------
            int whichTileToCollapse = Random.Range(0, tilesWithLowestEntropy.Count); //choose which tile to collapse randomly from list
            int ttcx = tilesWithLowestEntropy[whichTileToCollapse].x; //get the tile to collapse's coordinates
            int ttcy = tilesWithLowestEntropy[whichTileToCollapse].y;
            Tile tileToCollapse = mapTileInfo[ttcx, ttcy]; //get the tile object corresponding to the tile to collapse

            //need to get the array index of the game object to instantiate from the tile object
            string instantiateThis;
            instantiateThis = tileToCollapse.collapse();
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
            //save the gameobject in the map in case it needs to be deleted
            map[ttcx, ttcy] = Instantiate(tiles[tileNumToInstantiate], new Vector3(instantiateHereX, 0, instantiateHereY), Quaternion.Euler(new Vector3(-90, 0, 0))); //needs to be rotated because of how the models transfered to unity

            //UPDATE MATRIX-------------------------------------------------------------------------------------------------
            //update the entropy and possible tiles of each space on the board based on the tile just selected
            Stack<Tile> stack = new Stack<Tile>();
            stack.Push(tileToCollapse); //put the tile we just set on the stack

            while (stack.Count > 0)
            {
                Tile currentTile = stack.Pop();
                List<(int x, int y)> directions = new List<(int x, int y)>();
                directions = currentTile.getDirections(MAP_SIZE); //get the valid directions surrounding the tile. Tiles located on edges or corner will only return 2 or 3 directions

                for (int i = 0; i < directions.Count; i++) //iterate through each of the directions we just got
                {
                    Tile neighbor = mapTileInfo[currentTile.getX() + directions[i].x, currentTile.getY() + directions[i].y]; //get the neighbor of that tile in given direction
                    if (neighbor.constrain(currentTile.getPossibleTiles(), directions[i])) //if the neighbor's entropy is reduced at all
                    {
                        if (stack.Peek().getEntropy() == 0) //if the neighbor's entropy goes to 0 from this, we have a contradiction and need to restart
                            abort = true;
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
            if (highestValue == 0) isNetEntropyZero = true;

            //ABORT THE ALGORITHM IN CASE OF CONTRADICTION--------------------------------------------------------------------------
            if (abort)
            {
                isNetEntropyZero = false;
                abort = false;

                for (int i = 0; i < MAP_SIZE; i++)
                {
                    for (int j = 0; j < MAP_SIZE; j++)
                    {
                        mapTileInfo[i, j] = new Tile(tileNames, i, j); //reset map info
                        GameObject.Destroy(map[i, j]); //clear gameObjects
                    }
                }
            }
        }
    }
}
