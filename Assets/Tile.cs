using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private int entropy;
    private List<string> possibleTiles;
    private List<(int x, int y)> directions = new List<(int x, int y)>();

    public Tile(string[] tileNames) 
    {
        possibleTiles = new List<string>(tileNames);
        entropy = tileNames.Length;
    }

    public int getEntropy() 
    {
        return entropy;
    }

    public void addDirections(int x, int y, int MAP_SIZE)
    {
        if (x+1 <= MAP_SIZE) {
            directions.Add((1,0));
        }
        if (x-1 >= 0) {
            directions.Add((-1,0));
        }
        if (y+1 <= MAP_SIZE) {
            directions.Add((0,1));
        }
        if (y-1 >= 0) {
            directions.Add((0,-1));
        }
    }

    public List<(int x, int y)> getDirections() {
        return directions;
    }

    public string collapse() {
        // select weighted random from possibleTiles
        string tileName = possibleTiles[Random.Range(0, possibleTiles.Count)];
        entropy = 0;
        return tileName;
    }

    // public bool constrain(neighborPossibleTiles, direction)
    // {
    //     bool modified = false;

    //     if (entropy > 0)
    //     {
            
    //     }

    //     return modified;
    // }

}