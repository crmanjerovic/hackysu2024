using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Tile(string[] tileNames)
    {

    }

    public int getEntropy()
    {
        return 0;
    }

    public int collapse()
    {
        return 0;
    }

    public bool constrain()
    {
        return false;
    }

    public int[] getDirections()
    {
        return 0;
    }
}



public class waveFunctionCollapse : MonoBehaviour
{
    public const int NUM_TILES = 20;
    public const float TILE_SIZE = 2f;
    public const int MAP_SIZE = 30;

    public GameObject[] tiles = new GameObject[NUM_TILES]; //array of gameobjects
    string[] tileNames = new string[NUM_TILES]; //array of tile names

    public Dictionary<string, GameObject> tileTypes = new Dictionary<string, GameObject>(); //associates names with gameObjects

    GameObject[,] map = new GameObject[MAP_SIZE, MAP_SIZE]; //stores gameobjects to be instantiated in the scene

    Tile[,] mapTileInfo = new Tile[MAP_SIZE, MAP_SIZE];

    bool isNetEntropyZero = false;




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
                mapTileInfo[i, j] = new Tile(tileNames);
                map[i, j] = tiles[Random.Range(0, NUM_TILES)];
                Instantiate(map[i, j], new Vector3(TILE_SIZE * i, 0, TILE_SIZE * j), Quaternion.Euler(new Vector3(-90, 0, 0)));
            }
        }
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
                    if (mapTileInfo[i, j].getEntropy() < lowestValue)
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
                        tilesWithLowestEntropy.Add((i, j));
                    }
                }
            }

            //CHOOSE AND COLLAPSE THE TILE------------------------------------------------------------------------------------------
            int whichTileToCollapse = Random.Range(0, tilesWithLowestEntropy.Count);
            int ttcx = tilesWithLowestEntropy[whichTileToCollapse].x;
            int ttcy = tilesWithLowestEntropy[whichTileToCollapse].y;
            Tile tileToCollapse = mapTileInfo[ttcx, ttcy];

            //Collapse tile (decide on a type) and place it in the scene
            tileToCollapse.collapse();
            //Instantiate(, new Vector3(TILE_SIZE * i, 0, TILE_SIZE * j), Quaternion.Euler(new Vector3(-90, 0, 0)));

            //UPDATE MATRIX-------------------------------------------------------------------------------------------------
            Stack<Tile> stack = new Stack<Tile>();
            stack.Push(tileToCollapse);

            while (stack.Count > 0)
            {
                Tile currentTile = stack.Pop();
                List<(int x, int y)> directions = new List<(int x, int y)>();
                currentTile.addDirections(ttcx, ttcy, MAP_SIZE);
                directions = currentTile.getDirections();

                for (int i = 0; i < directions.Count; i++)
                {
                    Tile neighbor = currentTile.getNeighbor(directions[i]);
                    if (neighbor.constrain())
                        stack.Push(neighbor);
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
        }
    }
}
