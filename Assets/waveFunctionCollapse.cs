using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveFunctionCollapse : MonoBehaviour
{
    public const int NUM_TILES = 3;
    public const int TILE_SIZE = 1;
    public const int MAP_SIZE = 30;

    public GameObject[] tiles = new GameObject[NUM_TILES]; //add as many tiles as you want
    string[] tileNames = new string[NUM_TILES];

    //public const string[][] RULESET = new string[][];

    int i = 0;

    GameObject[,] map = new GameObject[MAP_SIZE, MAP_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < NUM_TILES; i++) //have an array of tiles names for readability
        {
            tileNames[i] = tiles[i].name;
            Debug.Log(tileNames[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (i < MAP_SIZE) { 
            for (int i2 = 0; i2 < MAP_SIZE; i2++)
            {
                map[i, i2] = tiles[Random.Range(0, 3)];
                Instantiate(map[i, i2], new Vector3(TILE_SIZE * i, 0, TILE_SIZE * i2), Quaternion.Euler(new Vector3(-90, 0, 0)));
            }
            i++;
        }
    }
}
