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

    bool isNetEntropyZero = false;

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

            List<string> instantiateThis = new List<string>();
            instantiateThis = tileToCollapse.getPossibleTiles();
            int tileNumToInstantiate = 0;
            for (int i = 0; i < NUM_TILES; i++)
            {
                if (tileNames[i] == instantiateThis[0])
                    tileNumToInstantiate = i;
            }
            float instantiateHereX = TILE_SIZE * tileToCollapse.getX();
            float instantiateHereY = TILE_SIZE * tileToCollapse.getY();
            // Debug.Log(tileToCollapse.getX());
            // Instantiate(tiles[tileNumToInstantiate], new Vector3(instantiateHereX, 0, instantiateHereY), Quaternion.Euler(new Vector3(-90, 0, 0)));

            //UPDATE MATRIX-------------------------------------------------------------------------------------------------
            Stack<Tile> stack = new Stack<Tile>();
            stack.Push(tileToCollapse);

            while (stack.Count > 0)
            {
                Tile currentTile = stack.Pop();
                List<(int x, int y)> directions = new List<(int x, int y)>();
                directions = currentTile.getDirections(MAP_SIZE);

                for (int i=0; i<directions.Count; i++)
                {
                    //get the neighbor of that tile in given direction
                    int neighborX = currentTile.getX() + directions[i].x;
                    int neighborY = currentTile.getY() + directions[i].y;
                    Tile neighbor = mapTileInfo[neighborX, neighborY];
                    bool constrained = neighbor.constrain(currentTile.getPossibleTiles(), directions[i], rules.list);
                    if (constrained)
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
