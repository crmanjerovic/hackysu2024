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
}


a
public class waveFunctionCollapse : MonoBehaviour
{
    public const int NUM_TILES = 20;
    public const float TILE_SIZE = 2f;
    public const int MAP_SIZE = 30;

    public GameObject[] tiles = new GameObject[NUM_TILES]; //add as many tiles as you want
    string[] tileNames = new string[NUM_TILES];

    public Dictionary<string, GameObject> tileTypes = new Dictionary<string, GameObject>();

    GameObject[,] map = new GameObject[MAP_SIZE, MAP_SIZE]; //stores gameobjects to be instantiated in the scene

    Tile[,] mapInfo = new Tile[MAP_SIZE, MAP_SIZE];



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
                mapInfo[i, j] = new Tile(tileNames);
                map[i, j] = tiles[Random.Range(0, NUM_TILES)];
                Instantiate(map[i, j], new Vector3(TILE_SIZE * i, 0, TILE_SIZE * j), Quaternion.Euler(new Vector3(-90, 0, 0)));
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        //GET SPACE WITH LOWEST ENTROPY
        //Iterate through, get lowest value
        int lowestValue = NUM_TILES + 1;
        for (int i=0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (mapInfo[i, j].getEntropy() < lowestValue)
                    lowestValue = mapInfo[i, j].getEntropy();
            }
        }

        //iterate again, getting list of all spaces of that value
        List<(int x, int y)> tilesWithLowestEntropy = new List<(int x, int y)>();
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (mapInfo[i, j].getEntropy() == lowestValue)
                {
                    tilesWithLowestEntropy.Add((i, j));
                }
            }
        }

        //choose one at random
        Tile tileToCollapse = tilesWithLowestEntropy[Random.Range(0, tilesWithLowestEntropy.length)];

        //Collapse tile (decide on a type) and place it in the scene
        tileToCollapse.collapse();
        //Instantiate(, new Vector3(TILE_SIZE * i, 0, TILE_SIZE * j), Quaternion.Euler(new Vector3(-90, 0, 0)));

        //UPDATE MATRIX
        Stack<Tile> stack = new Stack<Tile>();
        stack.push(tileToCollapse);

        while(!stack.is_empty())
        {
            Tile tile = stack.pop();
            List<(int x, int y)> directions = new List<(int x, int y)>();
            directions = tile.getDirections();

            for (int i=0; i < directions.length; i++)
            {
                Tile neighbor = tile.getNeighbor(directions[i])
                if (neighbor.constrain())
                    stack.push(neighbor);
            }
        }
    }
}
