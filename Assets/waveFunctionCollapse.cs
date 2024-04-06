using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{

    public const int NUM_TILES = 3;
    public const float TILE_SIZE = 2f;
    public const int MAP_SIZE = 30;

    public GameObject[] tiles = new GameObject[NUM_TILES]; //add as many tiles as you want
    string[] tileNames = new string[NUM_TILES];

    //public const string[][] RULESET = new string[][];

    GameObject[,] map = new GameObject[MAP_SIZE, MAP_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < NUM_TILES; i++) //have an array of tiles names for readability
        {
            tileNames[i] = tiles[i].name;
        }

        // for (int i = 0; i < MAP_SIZE; i++)
        // {
        //     for (int j = 0; j < MAP_SIZE; j++)
        //     {
        //         map[i, j] = tiles[Random.Range(0, NUM_TILES)];
        //         Instantiate(map[i, j], new Vector3(TILE_SIZE * i, 0, TILE_SIZE * j), Quaternion.Euler(new Vector3(-90, 0, 0)));
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
